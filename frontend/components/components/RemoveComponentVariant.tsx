import { useMutation } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  RemoveComponentVariantDocument,
  RemoveComponentVariantMutation,
} from "../../queries/componentVariants.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";

interface Props {
  oneComponentId: Scalars["Uuid"]["input"];
  otherComponentId: Scalars["Uuid"]["input"];
}

export default function RemoveComponentVariant({
  oneComponentId,
  otherComponentId,
}: Props) {
  const [removeComponentVariantMutation] = useMutation(
    RemoveComponentVariantDocument,
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<RemoveComponentVariantMutation>({
      getErrors: (data) => data.removeComponentVariant.errors,
    });

  const remove = () =>
    withMutationHandler(
      () =>
        removeComponentVariantMutation({
          variables: {
            input: {
              oneComponentId: oneComponentId,
              otherComponentId: otherComponentId,
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
