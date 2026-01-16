import { useMutation } from "@apollo/client/react";
import { Button, App } from "antd";
import { useState } from "react";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";
import { UserDocument } from "../../queries/users.generated";
import { RemoveInstitutionRepresentativeDocument } from "../../queries/institutionRepresentatives.generated";

export type RemoveInstitutionRepresentativeProps = {
  institutionId: Scalars["Uuid"]["input"];
  userId: Scalars["Uuid"]["input"];
};

export default function RemoveInstitutionRepresentative({
  institutionId,
  userId,
}: RemoveInstitutionRepresentativeProps) {
  const [removing, setRemoving] = useState(false);
  const { message } = App.useApp();

  const [removeInstitutionRepresentativeMutation] = useMutation(
    RemoveInstitutionRepresentativeDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: InstitutionDocument,
          variables: {
            uuid: institutionId,
          },
        },
        {
          query: UserDocument,
          variables: {
            uuid: userId,
          },
        },
      ],
    },
  );

  const removeInstitutionRepresentative = async () => {
    try {
      setRemoving(true);
      const { error, data } = await removeInstitutionRepresentativeMutation({
        variables: {
          input: {
            institutionId: institutionId,
            userId: userId,
          },
        },
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.removeInstitutionRepresentative?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.removeInstitutionRepresentative?.errors
            .map((error) => error.message)
            .join(" "),
        );
      }
    } finally {
      setRemoving(false);
    }
  };

  return (
    <Button
      danger
      type="primary"
      onClick={removeInstitutionRepresentative}
      loading={removing}
    >
      Remove
    </Button>
  );
}
