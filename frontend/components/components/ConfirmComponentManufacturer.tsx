import { useMutation } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  ConfirmComponentManufacturerDocument,
  ConfirmComponentManufacturerMutation,
} from "../../queries/componentManufacturers.generated";
import { ComponentDocument } from "../../queries/components.generated";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import { Button } from "antd";

interface Props {
  componentId: Scalars["Uuid"]["input"];
  institutionId: Scalars["Uuid"]["input"];
}

export function ConfirmComponentManufacturer({
  componentId,
  institutionId,
}: Props) {
  const [confirmComponentManufacturerMutation] = useMutation(
    ConfirmComponentManufacturerDocument,
    {
      refetchQueries: [
        {
          query: ComponentDocument,
          variables: {
            uuid: componentId,
          },
        },
        {
          query: InstitutionDocument,
          variables: {
            uuid: institutionId,
          },
        },
      ],
    },
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

  return (
    <Button onClick={() => confirm()} loading={mutating}>
      Confirm
    </Button>
  );
}
