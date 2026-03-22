import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  RevokeTokenDocument,
  RevokeTokenMutation,
} from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/graphql";
import { DocumentNode } from "graphql";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";

export type RevokeTokenProps = {
  tokenId: Scalars["Uuid"]["input"];
  refetchQueries: { query: DocumentNode; variables: { [key: string]: any } }[];
};

export default function RevokeToken({
  tokenId,
  refetchQueries,
}: RevokeTokenProps) {
  const [revokeTokenMutation] = useMutation(RevokeTokenDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: refetchQueries,
  });

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<RevokeTokenMutation>({
      getErrors: (data) => data.revokeOpenIdConnectToken.errors,
    });

  const revoke = async () => {
    withMutationHandler(
      () =>
        revokeTokenMutation({
          variables: {
            tokenId: tokenId,
          },
        }),
      {
        onError: messageErrors,
      },
    );
  };

  return (
    <Button danger type="primary" onClick={revoke} loading={mutating}>
      Revoke
    </Button>
  );
}
