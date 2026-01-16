import { useMutation } from "@apollo/client/react";
import { Button, App } from "antd";
import { useState } from "react";
import { RevokeTokenDocument } from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/graphql";
import { DocumentNode } from "graphql";

export type RevokeTokenProps = {
  tokenId: Scalars["Uuid"]["input"];
  refetchQueries: { query: DocumentNode; variables: { [key: string]: any } }[];
};

export default function RevokeToken({
  tokenId,
  refetchQueries,
}: RevokeTokenProps) {
  const [revoking, setRevoking] = useState(false);
  const { message } = App.useApp();

  const [revokeTokenMutation] = useMutation(RevokeTokenDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: refetchQueries,
  });

  const revokeToken = async () => {
    try {
      setRevoking(true);
      const { error, data } = await revokeTokenMutation({
        variables: {
          tokenId: tokenId,
        },
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.revokeOpenIdConnectToken?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.revokeOpenIdConnectToken?.errors
            .map((error) => error.message)
            .join(" "),
        );
      }
    } finally {
      setRevoking(false);
    }
  };

  return (
    <Button danger type="primary" onClick={revokeToken} loading={revoking}>
      Revoke
    </Button>
  );
}
