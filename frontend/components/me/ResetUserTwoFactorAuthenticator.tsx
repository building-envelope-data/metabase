import { useMutation } from "@apollo/client/react";
import { App, Button } from "antd";
import {
  ResetUserTwoFactorAuthenticatorDocument,
  ResetUserTwoFactorAuthenticatorMutation,
} from "../../queries/currentUser.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

export default function ResetUserTwoFactorAuthenticator() {
  const { message } = App.useApp();
  const [resetUserTwoFactorAuthenticator] = useMutation(
    ResetUserTwoFactorAuthenticatorDocument,
    {
      refetchQueries: [
        {
          query: ResetUserTwoFactorAuthenticatorDocument,
        },
      ],
    },
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<ResetUserTwoFactorAuthenticatorMutation>({
      getErrors: (data) => data.resetUserTwoFactorAuthenticator.errors,
    });

  const mutate = async () => {
    withMutationHandler(resetUserTwoFactorAuthenticator, {
      onSuccess: () => {
        message.success(
          "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.",
        );
      },
      onError: messageErrors,
    });
  };

  return (
    <Button type="primary" onClick={mutate} loading={mutating}>
      Reset authenticator app
    </Button>
  );
}
