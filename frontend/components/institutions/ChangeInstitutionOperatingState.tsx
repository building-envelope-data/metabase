import { Button, message } from "antd";
import { useRouter } from "next/router";
import { useState } from "react";
import paths from "../../paths";
import {
  InstitutionsDocument,
  useChangeInstitutionOperatingStateMutation,
} from "../../queries/institutions.graphql";
import { Scalars } from "../../__generated__/__types__";

export type changeInstitutionOperatingStateProps = {
  institutionId: Scalars["Uuid"];
};

export default function ChangeInstitutionOperatingState({
  institutionId,
}: changeInstitutionOperatingStateProps) {
  const router = useRouter();
  const [notOperating, setNotOperating] = useState(false);

  const [changeInstitutionOperatingStateMutation] = useChangeInstitutionOperatingStateMutation({
    refetchQueries: [
      {
        query: InstitutionsDocument,
      },
    ],
  });

  const changeOperatingState = async () => {
    try {
      setNotOperating(true);
      const { errors, data } = await changeInstitutionOperatingStateMutation({
        variables: {
          institutionId: institutionId,
        },
      });
      if (errors) {
        console.log(errors);
      } else if (data?.changeOperatingState?.errors) {
        message.error(
          data?.changeOperatingState?.errors
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
      onClick={changeOperatingState}
      loading={notOperating}
    >
      MODIFY STATE
    </Button>
  );

}