import { useMutation } from "@apollo/client/react";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";
import { UserDocument } from "../../queries/users.generated";
import {
  RemoveInstitutionRepresentativeDocument,
  RemoveInstitutionRepresentativeMutation,
} from "../../queries/institutionRepresentatives.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";

interface Props {
  institutionId: Scalars["Uuid"]["input"];
  userId: Scalars["Uuid"]["input"];
}

export default function RemoveInstitutionRepresentative({
  institutionId,
  userId,
}: Props) {
  const [removeInstitutionRepresentativeMutation] = useMutation(
    RemoveInstitutionRepresentativeDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: InstitutionDocument,
          variables: {
            uuid: institutionId,
          },
        },
        {
          query: UserDocument,
          variables: {
            uuid: userId,
          },
        },
      ],
    },
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
      kind="remove"
      type="icon"
      deleting={mutating}
      onConfirm={remove}
    />
  );
}
