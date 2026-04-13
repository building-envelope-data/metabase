import { ApolloClient, CombinedGraphQLErrors, ErrorLike } from "@apollo/client";
import { App, FormInstance } from "antd";
import { useState } from "react";
import { UserError } from "../../__generated__/graphql";
import { GraphQLFormattedError } from "graphql";

interface UseMutationHandlerProps<TMutation> {
  getErrors: (data: TMutation) => UserError[] | null;
}

interface Callbacks<TMutation> {
  onSuccess?: (data: TMutation | null) => void | Promise<any>;
  onError?: (
    graphQlErrors: readonly GraphQLFormattedError[] | null,
    userErrors: UserError[] | null,
  ) => void | Promise<any>;
}

export function useMutationHandler<TMutation>({
  getErrors,
}: UseMutationHandlerProps<TMutation>) {
  const [mutating, setMutating] = useState(false);
  const { message } = App.useApp();

  const withMutationHandler = async (
    mutationFunction: () => Promise<ApolloClient.MutateResult<TMutation>>,
    callbacks?: Callbacks<TMutation>,
  ) => {
    try {
      setMutating(true);
      const result = await mutationFunction();
      handleMutationResult(result.data, result.error, callbacks);
      return result;
    } catch (error) {
      message.error(
        error instanceof Error ? error.message : "An unexpected error occurred",
      );
    } finally {
      setMutating(false);
    }
  };

  const handleMutationResult = async (
    data: TMutation | undefined,
    apolloError: ErrorLike | undefined,
    callbacks?: Callbacks<TMutation>,
  ) => {
    if (apolloError && !CombinedGraphQLErrors.is(apolloError)) {
      message.error(apolloError.message);
      return;
    }
    const graphQlErrors = CombinedGraphQLErrors.is(apolloError)
      ? apolloError.errors
      : null;
    const userErrors = data ? getErrors(data) : null;
    if (graphQlErrors?.length || userErrors?.length) {
      callbacks?.onError?.(graphQlErrors, userErrors);
    } else {
      await callbacks?.onSuccess?.(data ?? null);
    }
  };

  const messageErrors = async (
    graphQlErrors: readonly GraphQLFormattedError[] | null,
    userErrors: UserError[] | null,
  ) => {
    const errors = [...(graphQlErrors ?? []), ...(userErrors ?? [])];
    if (errors?.length) {
      message.error(errors.map((error) => error.message).join("\n"));
    }
  };

  const messageMissingModel = async () => {
    message.error("The mutation did not return the new model.");
  };

  return {
    mutating,
    withMutationHandler,
    messageErrors,
    augmentFormWithErrors,
    messageMissingModel,
  };
}

function augmentFormWithErrors(
  graphQlErrors: readonly GraphQLFormattedError[] | undefined | null,
  userErrors: UserError[] | undefined | null,
  form: FormInstance<any>,
) {
  const globalErrorMessages = new Array<string>();
  const transformedGraphQlErrors = graphQlErrors?.map((error) => ({
    message: error.message ?? "",
    // TODO Do not hardcode the fallback path `["input"]`
    path: error.path ?? ["input"],
  }));
  const errors = [...(transformedGraphQlErrors ?? []), ...(userErrors ?? [])];
  if (errors?.length) {
    globalErrorMessages.push("The form contains errors.");
  }
  const errorPathToMessage = errors.reduce((accumulator, error) => {
    // We use strings as keys instead of path arrays because the
    // latter are compared by reference.
    const pathAsString = error.path.join(".");
    if (!accumulator.has(pathAsString)) {
      accumulator.set(pathAsString, [error.path, []]);
    }
    accumulator.get(pathAsString)?.[1]?.push(error.message);
    return accumulator;
  }, new Map<string, [readonly (string | number)[], string[]]>());
  for (let [, [path, messages]] of errorPathToMessage) {
    if (path.length === 1 || form.getFieldInstance(path.slice(1)) == null) {
      globalErrorMessages.push(...messages);
    } else {
      form.setFields([
        {
          name: path.slice(1),
          errors: messages,
        },
      ]);
    }
  }
  return globalErrorMessages;
}
