import { Button, message } from "antd";
import { useState } from "react";
import {
  ApplicationDocument,
  ApplicationsDocument,
  useDeleteApplicationMutation,
} from "../../../queries/openIdConnect.graphql";
import { Scalars } from "../../../__generated__/__types__";

export type DeleteApplicationProps = {
  applicationId: Scalars["Uuid"];
};

export default function DeleteApplication({
  applicationId
}: DeleteApplicationProps) {
  const [deleting, setDeleting] = useState(false);

  const [deleteApplicationMutation] = useDeleteApplicationMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: ApplicationsDocument,
      },
      {
        query: ApplicationDocument,
        variables: {
          uuid: applicationId,
        }
      },
    ]
  });

  const deleteApplication = async () => {
    try {
      setDeleting(true);
      const { errors, data } = await deleteApplicationMutation({
        variables: {
          applicationId: applicationId,
        },
      });
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.deleteOpenIdConnectApplication?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.deleteOpenIdConnectApplication?.errors
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
      onClick={deleteApplication}
      loading={deleting}
    >
      Delete
    </Button>
  );
}
