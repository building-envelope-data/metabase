import { useMutation } from "@apollo/client/react";
import {
  AuthorizationsDocument,
  DeleteAuthorizationDocument,
  DeleteAuthorizationMutation,
} from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/graphql";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../../SafeDeleteButton";

interface DeleteAuthorizationProps {
  authorizationId: Scalars["Uuid"]["input"];
}

export default function DeleteOpenIdConnectAuthorization({
  authorizationId,
}: DeleteAuthorizationProps) {
  const [deleteAuthorizationMutation] = useMutation(
    DeleteAuthorizationDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [AuthorizationsDocument],
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
    <SafeDeleteButton kind="delete" onConfirm={mutate} deleting={mutating} />
  );
}
