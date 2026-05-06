import { useMutation } from "@apollo/client/react";
import {
  RevokeTokenDocument,
  RevokeTokenMutation,
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
  const [revokeTokenMutation] = useMutation(RevokeTokenDocument, {});

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

  return <DeleteButton title="Revoke" deleting={mutating} onClick={mutate} />;
}
