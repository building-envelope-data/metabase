import { useMutation } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  RemoveComponentGeneralizationDocument,
  RemoveComponentGeneralizationMutation,
} from "../../queries/componentGeneralizations.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";

interface Props {
  generalComponentId: Scalars["Uuid"]["input"];
  concreteComponentId: Scalars["Uuid"]["input"];
}

export default function RemoveComponentGeneralization({
  generalComponentId,
  concreteComponentId,
}: Props) {
  const [removeComponentGeneralizationMutation] = useMutation(
    RemoveComponentGeneralizationDocument,
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
      type="icon"
      kind="remove"
      onConfirm={remove}
      deleting={mutating}
    />
  );
}
