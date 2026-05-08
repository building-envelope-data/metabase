import { useMutation } from "@apollo/client/react";
import { useRouter } from "next/router";
import paths from "../../paths";
import {
  InstitutionsDocument,
  DeleteInstitutionDocument,
  DeleteInstitutionMutation,
} from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";

interface DeleteInstitutionProps {
  institutionId: Scalars["Uuid"]["input"];
}

export default function DeleteInstitution({
  institutionId,
}: DeleteInstitutionProps) {
  const router = useRouter();

  const [deleteInstitutionMutation] = useMutation(DeleteInstitutionDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [InstitutionsDocument],
  });

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<DeleteInstitutionMutation>({
      getErrors: (data) => data.deleteInstitution.errors,
    });

  const mutate = async () => {
    withMutationHandler(
      () =>
        deleteInstitutionMutation({
          variables: {
            institutionId: institutionId,
          },
        }),
      {
        onSuccess: () => router.push(paths.institutions),
        onError: messageErrors,
      },
    );
  };

  return (
    <SafeDeleteButton kind="delete" onConfirm={mutate} deleting={mutating} />
  );
}
