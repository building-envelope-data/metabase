import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  InstitutionsDocument,
  PendingInstitutionsDocument,
  VerifyInstitutionDocument,
  VerifyInstitutionMutation,
} from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

interface Props {
  institutionId: Scalars["Uuid"]["input"];
}

export default function VerifyInstitution({ institutionId }: Props) {
  const [verifyInstitutionMutation] = useMutation(VerifyInstitutionDocument, {
    refetchQueries: [InstitutionsDocument, PendingInstitutionsDocument],
  });

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<VerifyInstitutionMutation>({
      getErrors: (data) => data.verifyInstitution.errors,
    });

  const mutate = () =>
    withMutationHandler(
      () =>
        verifyInstitutionMutation({
          variables: {
            institutionId: institutionId,
          },
        }),
      {
        onError: messageErrors,
      },
    );

  return (
    <Button onClick={mutate} loading={mutating}>
      Verify
    </Button>
  );
}
