import { useMutation } from "@apollo/client/react";
import { App, Button } from "antd";
import {
  GenerateUserTwoFactorRecoveryCodesDocument,
  GenerateUserTwoFactorRecoveryCodesMutation,
} from "../../queries/currentUser.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import { recoveryCodesModal } from "../../lib/recoveryCodesModal";
import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}

export default function GenerateUserTwoFactorRecoveryCodes({
  children,
}: Props) {
  const { modal } = App.useApp();
  const [generateUserTwoFactorRecoveryCodes] = useMutation(
    GenerateUserTwoFactorRecoveryCodesDocument,
    {
      refetchQueries: [
        {
          query: GenerateUserTwoFactorRecoveryCodesDocument,
        },
      ],
    },
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<GenerateUserTwoFactorRecoveryCodesMutation>({
      getErrors: (data) => data.generateUserTwoFactorRecoveryCodes.errors,
    });

  const mutate = async () => {
    withMutationHandler(generateUserTwoFactorRecoveryCodes, {
      onSuccess: (data) =>
        recoveryCodesModal(
          modal,
          data?.generateUserTwoFactorRecoveryCodes?.twoFactorRecoveryCodes ||
            [],
        ),
      onError: messageErrors,
    });
  };

  return (
    <Button type="primary" onClick={mutate} loading={mutating}>
      {children}
    </Button>
  );
}
