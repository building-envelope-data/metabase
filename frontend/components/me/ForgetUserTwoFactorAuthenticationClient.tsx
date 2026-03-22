import { useMutation } from "@apollo/client/react";
import { App, Button } from "antd";
import {
  ForgetUserTwoFactorAuthenticationClientDocument,
  ForgetUserTwoFactorAuthenticationClientMutation,
} from "../../queries/currentUser.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

export default function ForgetUserTwoFactorAuthenticationClient() {
  const { message } = App.useApp();
  const [forgetUserTwoFactorAuthenticationClient] = useMutation(
    ForgetUserTwoFactorAuthenticationClientDocument,
    {
      refetchQueries: [
        {
          query: ForgetUserTwoFactorAuthenticationClientDocument,
        },
      ],
    },
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<ForgetUserTwoFactorAuthenticationClientMutation>({
      getErrors: (data) => data.forgetUserTwoFactorAuthenticationClient.errors,
    });

  const mutate = async () => {
    withMutationHandler(forgetUserTwoFactorAuthenticationClient, {
      onSuccess: () => {
        message.success(
          "The current browser has been forgotten. When you login again from this browser you will be prompted for your two-factor authentication code.",
        );
      },
      onError: messageErrors,
    });
  };

  return (
    <Button type="primary" onClick={mutate} loading={mutating}>
      Forget this browser
    </Button>
  );
}
