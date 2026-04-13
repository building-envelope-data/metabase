import { useMutation } from "@apollo/client/react";
import { useEffect, useRef } from "react";
import { useRouter } from "next/router";
import {
  ConfirmUserEmailDocument,
  ConfirmUserEmailMutation,
} from "../../queries/users.generated";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { App, Typography } from "antd";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

function ConfirmUserEmail() {
  const router = useRouter();
  const { email, confirmationCode, returnTo } = router.query;
  const { message } = App.useApp();
  const hasCalledMutation = useRef(false);

  const [confirmUserEmailMutation] = useMutation(ConfirmUserEmailDocument);

  const { withMutationHandler, messageErrors } =
    useMutationHandler<ConfirmUserEmailMutation>({
      getErrors: (data) => data.confirmUserEmail.errors,
    });

  useEffect(() => {
    if (!router.isReady) return;
    if (hasCalledMutation.current) return;
    const confirm = async () => {
      hasCalledMutation.current = true;
      if (typeof email !== "string" || typeof confirmationCode !== "string") {
        message.error("Invalid email or confirmation code");
        return;
      }
      withMutationHandler(
        () =>
          confirmUserEmailMutation({
            variables: {
              input: {
                email: email,
                confirmationCode: confirmationCode,
              },
            },
          }),
        {
          onSuccess: () => {
            message.success("Email address confirmed!");
            router.push({
              pathname: paths.openIdConnectClientLogin,
              query: returnTo ? { returnTo: returnTo } : {},
            });
          },
          onError: messageErrors,
        },
      );
    };
    confirm();
  }, [
    email,
    confirmationCode,
    returnTo,
    router,
    confirmUserEmailMutation,
    withMutationHandler,
    messageErrors,
    message,
  ]);

  return (
    <Layout>
      <Typography.Paragraph>Confirming email ...</Typography.Paragraph>
    </Layout>
  );
}

export default ConfirmUserEmail;
