import { useMutation } from '@apollo/client/react';
import { Button, message } from "antd";
import { useRouter } from "next/router";
import { useState } from "react";
import paths from "../../paths";
import {
  InstitutionsDocument,
  DeleteInstitutionDocument,
} from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";

export type DeleteInstitutionProps = {
  institutionId: Scalars["Uuid"]["input"];
};

export default function DeleteInstitution({
  institutionId,
}: DeleteInstitutionProps) {
  const router = useRouter();

  const [deleting, setDeleting] = useState(false);

  const [deleteInstitutionMutation] = useMutation(DeleteInstitutionDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: InstitutionsDocument,
      },
    ],
  });

  const deleteInstitution = async () => {
    try {
      setDeleting(true);
      const { error, data } = await deleteInstitutionMutation({
        variables: {
          institutionId: institutionId,
        },
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.deleteInstitution?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.deleteInstitution?.errors
            .map((error) => error.message)
            .join(" ")
        );
      } else {
        await router.push(paths.institutions);
      }
    } finally {
      setDeleting(false);
    }
  };

  return (
    <Button
      danger
      type="primary"
      onClick={deleteInstitution}
      loading={deleting}
    >
      Delete
    </Button>
  );
}
