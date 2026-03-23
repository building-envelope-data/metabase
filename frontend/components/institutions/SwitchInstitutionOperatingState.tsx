import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  SwitchInstitutionOperatingStateDocument,
  SwitchInstitutionOperatingStateMutation,
} from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

export type switchInstitutionOperatingStateProps = {
  institutionId: Scalars["Uuid"]["input"];
};

export default function SwitchInstitutionOperatingState({
  institutionId,
}: switchInstitutionOperatingStateProps) {
  const [switchInstitutionOperatingStateMutation] = useMutation(
    SwitchInstitutionOperatingStateDocument,
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<SwitchInstitutionOperatingStateMutation>({
      getErrors: (data) => data.switchInstitutionOperatingState.errors,
    });

  const doSwitch = async () => {
    withMutationHandler(
      () =>
        switchInstitutionOperatingStateMutation({
          variables: {
            institutionId: institutionId,
          },
        }),
      {
        onError: messageErrors,
      },
    );
  };

  return (
    <Button type="primary" onClick={doSwitch} loading={mutating}>
      Switch Operating State
    </Button>
  );
}
