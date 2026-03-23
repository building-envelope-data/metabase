import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  ApplicationDocument,
  ApplicationsDocument,
  DeleteApplicationDocument,
  DeleteApplicationMutation,
} from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/graphql";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";
import { useRouter } from "next/router";
import { Route } from "next";

export type DeleteApplicationProps = {
  applicationId: Scalars["Uuid"]["input"];
  redirectTo: Route;
};

export default function DeleteApplication({
  applicationId,
  redirectTo,
}: DeleteApplicationProps) {
  const router = useRouter();

  const [deleteApplicationMutation] = useMutation(DeleteApplicationDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: ApplicationsDocument,
      },
      {
        query: ApplicationDocument,
        variables: {
          uuid: applicationId,
        },
      },
    ],
  });

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<DeleteApplicationMutation>({
      getErrors: (data) => data.deleteOpenIdConnectApplication.errors,
    });

  const mutate = async () => {
    withMutationHandler(
      () =>
        deleteApplicationMutation({
          variables: {
            input: {
              applicationId: applicationId,
            },
          },
        }),
      {
        onSuccess: () => router.push(redirectTo),
        onError: messageErrors,
      },
    );
  };

  return (
    <Button danger type="primary" onClick={mutate} loading={mutating}>
      Delete
    </Button>
  );
}
