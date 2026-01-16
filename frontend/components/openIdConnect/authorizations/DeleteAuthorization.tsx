import { useMutation } from "@apollo/client/react";
import { Button, App } from "antd";
import { useState } from "react";
import { DeleteAuthorizationDocument } from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/graphql";
import { DocumentNode } from "graphql";

export type DeleteAuthorizationProps = {
  authorizationId: Scalars["Uuid"]["input"];
  refetchQueries: { query: DocumentNode; variables: { [key: string]: any } }[];
};

export default function DeleteAuthorization({
  authorizationId,
  refetchQueries,
}: DeleteAuthorizationProps) {
  const [deleting, setDeleting] = useState(false);
  const { message } = App.useApp();

  const [deleteAuthorizationMutation] = useMutation(
    DeleteAuthorizationDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: refetchQueries,
    },
  );

  const deleteAuthorization = async () => {
    try {
      setDeleting(true);
      const { error, data } = await deleteAuthorizationMutation({
        variables: {
          authorizationId: authorizationId,
        },
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.deleteOpenIdConnectAuthorization?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.deleteOpenIdConnectAuthorization?.errors
            .map((error) => error.message)
            .join(" "),
        );
      }
    } finally {
      setDeleting(false);
    }
  };

  return (
    <Button
      danger
      type="primary"
      onClick={deleteAuthorization}
      loading={deleting}
    >
      Delete
    </Button>
  );
}
