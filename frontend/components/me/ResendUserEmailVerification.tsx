import { Button, App } from "antd";
import {
  ResendUserEmailVerificationDocument,
  ResendUserEmailVerificationMutation,
} from "../../queries/currentUser.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import { useMutation } from "@apollo/client/react";

export function ResendUserEmailVerification() {
  const { message } = App.useApp();

  const [resendUserEmailVerificationMutation] = useMutation(
    ResendUserEmailVerificationDocument,
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<ResendUserEmailVerificationMutation>({
      getErrors: (data) => data.resendUserEmailVerification.errors,
    });

  const resendUserEmailVerification = async () => {
    withMutationHandler(() => resendUserEmailVerificationMutation(), {
      onSuccess: () => {
        message.success("Verification email sent. Please check your email.");
      },
      onError: messageErrors,
    });
  };

  return (
    <Button onClick={resendUserEmailVerification} loading={mutating}>
      Resend verification email
    </Button>
  );
}
