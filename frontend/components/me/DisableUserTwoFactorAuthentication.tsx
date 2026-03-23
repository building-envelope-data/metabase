import { useMutation } from "@apollo/client/react";
import { App, Button } from "antd";
import {
  DisableUserTwoFactorAuthenticationDocument,
  DisableUserTwoFactorAuthenticationMutation,
} from "../../queries/currentUser.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

export default function DisableUserTwoFactorAuthentication() {
  const { message } = App.useApp();
  const [disableUserTwoFactorAuthentication] = useMutation(
    DisableUserTwoFactorAuthenticationDocument,
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<DisableUserTwoFactorAuthenticationMutation>({
      getErrors: (data) => data.disableUserTwoFactorAuthentication.errors,
    });

  const mutate = async () => {
    withMutationHandler(disableUserTwoFactorAuthentication, {
      onSuccess: () => {
        message.success(
          "Two-factor authentication has been disabled. You can reenable it when you setup an authenticator app.",
        );
      },
      onError: messageErrors,
    });
  };

  return (
    <Button type="primary" onClick={mutate} loading={mutating}>
      Disable two-factor authentication
    </Button>
  );
}
