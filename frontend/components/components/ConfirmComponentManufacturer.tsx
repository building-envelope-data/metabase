import { useMutation } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  ConfirmComponentManufacturerDocument,
  ConfirmComponentManufacturerMutation,
} from "../../queries/componentManufacturers.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ConfirmButton from "../ConfirmButton";

interface Props {
  componentId: Scalars["Uuid"]["input"];
  institutionId: Scalars["Uuid"]["input"];
}

export default function ConfirmComponentManufacturer({
  componentId,
  institutionId,
}: Props) {
  const [confirmComponentManufacturerMutation] = useMutation(
    ConfirmComponentManufacturerDocument,
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<ConfirmComponentManufacturerMutation>({
      getErrors: (data) => data.confirmComponentManufacturer.errors,
    });

  const confirm = () =>
    withMutationHandler(
      () =>
        confirmComponentManufacturerMutation({
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

  return <ConfirmButton onClick={confirm} loading={mutating} />;
}
