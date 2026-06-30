import { useMutation } from "@apollo/client/react";
import {
  RevokeTokenDocument,
  RevokeTokenMutation,
  TokensDocument,
} from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/graphql";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";
import DeleteButton from "../../DeleteButton";

interface RevokeTokenProps {
  tokenId: Scalars["Uuid"]["input"];
}

export default function RevokeOpenIdConnectToken({
  tokenId,
}: RevokeTokenProps) {
  const [revokeTokenMutation] = useMutation(RevokeTokenDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [TokensDocument],
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
    <DeleteButton type="default" deleting={mutating} onClick={mutate}>
      Revoke
    </DeleteButton>
  );
}
