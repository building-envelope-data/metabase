import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";
import { UserDocument } from "../../queries/users.generated";
import {
  ConfirmInstitutionRepresentativeDocument,
  ConfirmInstitutionRepresentativeMutation,
} from "../../queries/institutionRepresentatives.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

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

  return (
    <Button type="primary" onClick={confirm} loading={mutating}>
      Confirm
    </Button>
  );
}
