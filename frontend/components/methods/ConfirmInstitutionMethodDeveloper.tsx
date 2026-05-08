import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import { Scalars } from "../../__generated__/graphql";
import {
  ConfirmInstitutionMethodDeveloperDocument,
  ConfirmInstitutionMethodDeveloperMutation,
} from "../../queries/institutionMethodDevelopers.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

interface Props {
  methodId: Scalars["Uuid"]["input"];
  institutionId: Scalars["Uuid"]["input"];
}

export default function ConfirmInstitutionMethodDeveloper({
  methodId,
  institutionId,
}: Props) {
  const [confirmInstitutionMethodDeveloperMutation] = useMutation(
    ConfirmInstitutionMethodDeveloperDocument,
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<ConfirmInstitutionMethodDeveloperMutation>({
      getErrors: (data) => data.confirmInstitutionMethodDeveloper.errors,
    });

  const confirm = async () => {
    withMutationHandler(
      () =>
        confirmInstitutionMethodDeveloperMutation({
          variables: {
            input: {
              methodId: methodId,
              institutionId: institutionId,
            },
          },
        }),
      {
        onError: messageErrors,
      },
    );
  };

  return (
    <Button type="primary" onClick={confirm} loading={mutating}>
      Confirm
    </Button>
  );
}
