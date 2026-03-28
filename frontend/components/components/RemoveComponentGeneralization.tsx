import { useMutation } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  RemoveComponentGeneralizationDocument,
  RemoveComponentGeneralizationMutation,
} from "../../queries/componentGeneralizations.generated";
import {
  ComponentDocument,
  ComponentsDocument,
} from "../../queries/components.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";

interface Props {
  generalComponentId: Scalars["Uuid"]["input"];
  concreteComponentId: Scalars["Uuid"]["input"];
}

export function RemoveComponentGeneralization({
  generalComponentId,
  concreteComponentId,
}: Props) {
  const [removeComponentGeneralizationMutation] = useMutation(
    RemoveComponentGeneralizationDocument,
    {
      refetchQueries: [
        {
          query: ComponentsDocument,
        },
        {
          query: ComponentDocument,
          variables: {
            uuid: generalComponentId,
          },
        },
        {
          query: ComponentDocument,
          variables: {
            uuid: concreteComponentId,
          },
        },
      ],
    },
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<RemoveComponentGeneralizationMutation>({
      getErrors: (data) => data.removeComponentGeneralization.errors,
    });

  const remove = () =>
    withMutationHandler(
      () =>
        removeComponentGeneralizationMutation({
          variables: {
            input: {
              generalComponentId: generalComponentId,
              concreteComponentId: concreteComponentId,
            },
          },
        }),
      {
        onError: messageErrors,
      },
    );

  return (
    <SafeDeleteButton
      type="text"
      kind="remove"
      onConfirm={remove}
      deleting={mutating}
    />
  );
}
