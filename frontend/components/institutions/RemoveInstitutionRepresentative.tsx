import { useMutation } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  RemoveInstitutionRepresentativeDocument,
  RemoveInstitutionRepresentativeMutation,
} from "../../queries/institutionRepresentatives.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";

interface Props {
  institutionId: Scalars["Uuid"]["input"];
  userId: Scalars["Uuid"]["input"];
  children?: React.ReactNode;
}

export default function RemoveInstitutionRepresentative({
  institutionId,
  userId,
  children,
}: Props) {
  const [removeInstitutionRepresentativeMutation] = useMutation(
    RemoveInstitutionRepresentativeDocument,
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<RemoveInstitutionRepresentativeMutation>({
      getErrors: (data) => data.removeInstitutionRepresentative.errors,
    });

  const remove = async () => {
    withMutationHandler(
      () =>
        removeInstitutionRepresentativeMutation({
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

  return (
    <SafeDeleteButton
      type="icon"
      kind="remove"
      deleting={mutating}
      onConfirm={remove}
    >
      {children}
    </SafeDeleteButton>
  );
}
