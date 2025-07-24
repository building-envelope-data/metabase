import { Button, message } from "antd";
import { useState } from "react";
import {
  useRevokeTokenMutation,
} from "../../../queries/openIdConnectTokens.graphql";
import { Scalars } from "../../../__generated__/__types__";
import { DocumentNode } from "graphql";

export type RevokeTokenProps = {
  tokenId: Scalars["Uuid"];
  refetchQueries: {query: DocumentNode, variables: {[key: string]: any}}[];
};

export default function RevokeToken({
  tokenId,
  refetchQueries,
}: RevokeTokenProps) {
  const [revoking, setRevoking] = useState(false);

  const [revokeTokenMutation] = useRevokeTokenMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: refetchQueries,
  });

  const revokeToken = async () => {
    try {
      setRevoking(true);
      const { errors, data } = await revokeTokenMutation({
        variables: {
          tokenId: tokenId,
        },
      });
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.revokeOpenIdConnectToken?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.revokeOpenIdConnectToken?.errors
            .map((error) => error.message)
            .join(" ")
        );
      }
    } finally {
      setRevoking(false);
    }
  };

  return (
    <Button
      danger
      type="primary"
      onClick={revokeToken}
      loading={revoking}
    >
      Revoke
    </Button>
  );
}
