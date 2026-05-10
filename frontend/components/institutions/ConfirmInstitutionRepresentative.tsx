import { useMutation } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  ConfirmInstitutionRepresentativeDocument,
  ConfirmInstitutionRepresentativeMutation,
} from "../../queries/institutionRepresentatives.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ConfirmButton from "../ConfirmButton";

interface Props {
  institutionId: Scalars["Uuid"]["input"];
  userId: Scalars["Uuid"]["input"];
}

export default function ConfirmInstitutionRepresentative({
  institutionId,
  userId,
}: Props) {
  const [confirmInstitutionRepresentativeMutation] = useMutation(
    ConfirmInstitutionRepresentativeDocument,
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<ConfirmInstitutionRepresentativeMutation>({
      getErrors: (data) => data.confirmInstitutionRepresentative.errors,
    });

  const confirm = async () => {
    withMutationHandler(
      () =>
        confirmInstitutionRepresentativeMutation({
          variables: {
            input: {
              institutionId: institutionId,
              userId: userId,
            },
          },
        }),
      {
        onError: messageErrors,
      },
    );
  };

  return <ConfirmButton type="icon" onClick={confirm} loading={mutating} />;
}
