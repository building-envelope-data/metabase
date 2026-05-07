import { useMutation } from "@apollo/client/react";
import {
  ApplicationDocument,
  RevokeTokenDocument,
  RevokeTokenMutation,
  TokensDocument,
} from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/graphql";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";
import DeleteButton from "../../DeleteButton";

interface RevokeTokenProps {
  tokenId: Scalars["Uuid"]["input"];
  applicationId: Scalars["Uuid"]["input"];
}

export default function RevokeOpenIdConnectToken({
  tokenId,
  applicationId,
}: RevokeTokenProps) {
  const [revokeTokenMutation] = useMutation(RevokeTokenDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: ApplicationDocument,
        variables: {
          uuid: applicationId,
        },
      },
      TokensDocument,
    ],
  });

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<RevokeTokenMutation>({
      getErrors: (data) => data.revokeOpenIdConnectToken.errors,
    });

  const mutate = async () => {
    withMutationHandler(
      () =>
        revokeTokenMutation({
          variables: {
            input: {
              tokenId: tokenId,
            },
          },
        }),
      {
        onError: messageErrors,
      },
    );
  };

  return (
    <DeleteButton deleting={mutating} onClick={mutate}>
      Revoke
    </DeleteButton>
  );
}
