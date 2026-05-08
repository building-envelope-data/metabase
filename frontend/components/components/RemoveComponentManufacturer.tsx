import { useMutation } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  RemoveComponentManufacturerDocument,
  RemoveComponentManufacturerMutation,
} from "../../queries/componentManufacturers.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";

interface Props {
  componentId: Scalars["Uuid"]["input"];
  institutionId: Scalars["Uuid"]["input"];
}

export default function RemoveComponentManufacturer({
  componentId,
  institutionId,
}: Props) {
  const [removeComponentManufacturerMutation] = useMutation(
    RemoveComponentManufacturerDocument,
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<RemoveComponentManufacturerMutation>({
      getErrors: (data) => data.removeComponentManufacturer.errors,
    });

  const remove = () =>
    withMutationHandler(
      () =>
        removeComponentManufacturerMutation({
          variables: {
            input: {
              componentId: componentId,
              institutionId: institutionId,
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
