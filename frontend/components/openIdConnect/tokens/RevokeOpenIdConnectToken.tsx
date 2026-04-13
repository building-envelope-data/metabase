import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  RevokeTokenDocument,
  RevokeTokenMutation,
} from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/graphql";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";

interface RevokeTokenProps {
  tokenId: Scalars["Uuid"]["input"];
}

export default function RevokeOpenIdConnectToken({
  tokenId,
}: RevokeTokenProps) {
  const [revokeTokenMutation] = useMutation(RevokeTokenDocument, {});

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<RevokeTokenMutation>({
      getErrors: (data) => data.revokeOpenIdConnectToken.errors,
    });

  const revoke = async () => {
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
    <Button danger type="primary" onClick={revoke} loading={mutating}>
      Revoke
    </Button>
  );
}
