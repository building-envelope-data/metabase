import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import { MethodDocument } from "../../queries/methods.generated";
import { Scalars } from "../../__generated__/graphql";
import { InstitutionDocument } from "../../queries/institutions.generated";
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
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: MethodDocument,
          variables: {
            uuid: methodId,
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
