import { Button, message } from "antd";
import { useState } from "react";
import {
  useDeleteAuthorizationMutation,
} from "../../../queries/openIdConnect.graphql";
import { Scalars } from "../../../__generated__/__types__";
import { DocumentNode } from "graphql";

export type DeleteAuthorizationProps = {
  authorizationId: Scalars["Uuid"];
  refetchQueries: { query: DocumentNode, variables: { [key: string]: any } }[];
};

export default function DeleteAuthorization({
  authorizationId,
  refetchQueries,
}: DeleteAuthorizationProps) {
  const [deleting, setDeleting] = useState(false);

  const [deleteAuthorizationMutation] = useDeleteAuthorizationMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: refetchQueries,
  });

  const deleteAuthorization = async () => {
    try {
      setDeleting(true);
      const { errors, data } = await deleteAuthorizationMutation({
        variables: {
          authorizationId: authorizationId,
        },
      });
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.deleteOpenIdConnectAuthorization?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.deleteOpenIdConnectAuthorization?.errors
            .map((error) => error.message)
            .join(" ")
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
