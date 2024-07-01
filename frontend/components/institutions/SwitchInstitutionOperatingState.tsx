import { Button, message } from "antd";
import { useRouter } from "next/router";
import { useState } from "react";
import paths from "../../paths";
import {
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
  const [notOperating, setNotOperating] = useState(false);

  const [switchInstitutionOperatingStateMutation] = useSwitchInstitutionOperatingStateMutation({
    refetchQueries: [
      {
        query: InstitutionsDocument,
      },
    ],
  });

  const switchOperatingState = async () => {
    try {
      setNotOperating(true);
      const { errors, data } = await switchInstitutionOperatingStateMutation({
        variables: {
          institutionId: institutionId,
        },
      });
      if (errors) {
        console.log(errors);
      } else if (data?.switchOperatingState?.errors) {
        message.error(
          data?.switchOperatingState?.errors
            .map((error: { message: any; }) => error.message)
            .join(" ")
        );
      } else {
        await router.push(paths.institutions);
      }
    } finally {
      setNotOperating(false);
    }

  };

  return (
    <Button
      danger
      type="primary"
      onClick={switchOperatingState}
      loading={notOperating}
    >
      Switch Operating State
    </Button>
  );

}