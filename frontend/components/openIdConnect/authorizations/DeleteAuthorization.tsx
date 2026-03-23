import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  DeleteAuthorizationDocument,
  DeleteAuthorizationMutation,
} from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/graphql";
import { DocumentNode } from "graphql";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";

export type DeleteAuthorizationProps = {
  authorizationId: Scalars["Uuid"]["input"];
  refetchQueries: { query: DocumentNode; variables: { [key: string]: any } }[];
};

export default function DeleteAuthorization({
  authorizationId,
  refetchQueries,
}: DeleteAuthorizationProps) {
  const [deleteAuthorizationMutation] = useMutation(
    DeleteAuthorizationDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: refetchQueries,
    },
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<DeleteAuthorizationMutation>({
      getErrors: (data) => data.deleteOpenIdConnectAuthorization.errors,
    });

  const mutate = async () => {
    withMutationHandler(
      () =>
        deleteAuthorizationMutation({
          variables: {
            input: {
              authorizationId: authorizationId,
            },
          },
        }),
      {
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
