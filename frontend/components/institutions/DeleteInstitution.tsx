import { Button, message } from "antd";
import { useRouter } from "next/router";
import { useState } from "react";
import paths from "../../paths";
import {
  InstitutionsDocument,
  useDeleteInstitutionMutation,
} from "../../queries/institutions.graphql";
import { Scalars } from "../../__generated__/__types__";

export type DeleteInstitutionProps = {
  institutionId: Scalars["Uuid"];
};

export default function DeleteInstitution({
  institutionId,
}: DeleteInstitutionProps) {
  const router = useRouter();

  const [deleting, setDeleting] = useState(false);

  const [deleteInstitutionMutation] = useDeleteInstitutionMutation({
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
      const { errors, data } = await deleteInstitutionMutation({
        variables: {
          institutionId: institutionId,
        },
      });
      if (errors) {
        console.log(errors); // TODO What to do?
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
