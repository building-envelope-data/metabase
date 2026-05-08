import { useMutation } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  RemoveUserMethodDeveloperDocument,
  RemoveUserMethodDeveloperMutation,
} from "../../queries/userMethodDevelopers.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";

interface Props {
  methodId: Scalars["Uuid"]["input"];
  userId: Scalars["Uuid"]["input"];
  children?: React.ReactNode;
}

export default function RemoveUserMethodDeveloper({
  methodId,
  userId,
  children,
}: Props) {
  const [removeUserMethodDeveloperMutation] = useMutation(
    RemoveUserMethodDeveloperDocument,
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<RemoveUserMethodDeveloperMutation>({
      getErrors: (data) => data.removeUserMethodDeveloper.errors,
    });

  const mutate = async () => {
    withMutationHandler(
      () =>
        removeUserMethodDeveloperMutation({
          variables: {
            input: {
              methodId: methodId,
              userId: userId,
            },
          },
        }),
      {
        onError: messageErrors,
      },
    );
  };

  return (
    <SafeDeleteButton
      type="icon"
      kind="remove"
      onConfirm={mutate}
      deleting={mutating}
    >
      {children}
    </SafeDeleteButton>
  );
}
