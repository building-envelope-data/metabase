import { useMutation } from '@apollo/client/react';
import { Button, message } from "antd";
import { useState } from "react";
import {
  ApplicationDocument,
  ApplicationsDocument,
  DeleteApplicationDocument,
} from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/graphql";

export type DeleteApplicationProps = {
  applicationId: Scalars["Uuid"]["input"];
};

export default function DeleteApplication({
  applicationId
}: DeleteApplicationProps) {
  const [deleting, setDeleting] = useState(false);

  const [deleteApplicationMutation] = useMutation(DeleteApplicationDocument, {
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
      const { error, data } = await deleteApplicationMutation({
        variables: {
          applicationId: applicationId,
        },
      });
      if (error) {
        console.log(error); // TODO What to do?
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
