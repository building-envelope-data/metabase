import { useMutation } from '@apollo/client/react';
import { Button, message } from "antd";
import { useState } from "react";
import {
  InstitutionDocument,
  InstitutionsDocument,
  SwitchInstitutionOperatingStateDocument,
} from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";

export type switchInstitutionOperatingStateProps = {
  institutionId: Scalars["Uuid"]["input"];
};

export default function SwitchInstitutionOperatingState({
  institutionId,
}: switchInstitutionOperatingStateProps) {
  const [switching, setSwitching] = useState(false);

  const [switchInstitutionOperatingStateMutation] = useMutation(SwitchInstitutionOperatingStateDocument);

  const switchInstitutionOperatingState = async () => {
    try {
      setSwitching(true);
      const { error, data } = await switchInstitutionOperatingStateMutation({
        variables: {
          institutionId: institutionId,
        },
        refetchQueries: [
          {
            query: InstitutionsDocument,
          },
          {
            query: InstitutionDocument,
            variables: {
              uuid: institutionId,
            },
          },
        ],
      });
      if (error) {
        console.log(error);
      } else if (data?.switchInstitutionOperatingState?.errors) {
        message.error(
          data?.switchInstitutionOperatingState?.errors
            .map((error: { message: any; }) => error.message)
            .join(" ")
        );
      }
    } finally {
      setSwitching(false);
    }
  };

  return (
    <Button
      type="primary"
      onClick={switchInstitutionOperatingState}
      loading={switching}
    >
      Switch Operating State
    </Button>
  );

}