import { Button, message } from "antd";
import { useRouter } from "next/router";
import { useState } from "react";
import paths from "../../paths";
import {
  InstitutionDocument,
  InstitutionsDocument,
  useSwitchInstitutionOperatingStateMutation,
} from "../../queries/institutions.graphql";
import { Scalars } from "../../__generated__/__types__";

export type switchInstitutionOperatingStateProps = {
  institutionId: Scalars["Uuid"];
};

export default function SwitchInstitutionOperatingState({
  institutionId,
}: switchInstitutionOperatingStateProps) {
  const router = useRouter();
  const [switching, setSwitching] = useState(false);

  const [switchInstitutionOperatingStateMutation] = useSwitchInstitutionOperatingStateMutation();

  const switchInstitutionOperatingState = async () => {
    try {
      setSwitching(true);
      const { errors, data } = await switchInstitutionOperatingStateMutation({
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
      if (errors) {
        console.log(errors);
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