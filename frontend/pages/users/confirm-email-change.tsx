import { useMutation } from "@apollo/client/react";
import { useEffect, useRef } from "react";
import { useRouter } from "next/router";
import {
  ConfirmUserEmailChangeDocument,
  ConfirmUserEmailChangeMutation,
} from "../../queries/users.generated";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { App, Typography } from "antd";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

function Page() {
  const router = useRouter();
  const { currentEmail, newEmail, confirmationCode } = router.query;
  const { message } = App.useApp();
  const hasCalledMutation = useRef(false);

  const [confirmUserEmailChangeMutation] = useMutation(
    ConfirmUserEmailChangeDocument,
  );

  const { withMutationHandler, messageErrors } =
    useMutationHandler<ConfirmUserEmailChangeMutation>({
      getErrors: (data) => data.confirmUserEmailChange.errors,
    });

  useEffect(() => {
    if (!router.isReady) return;
    if (hasCalledMutation.current) return;
    const confirm = async () => {
      hasCalledMutation.current = true;
      if (
        typeof currentEmail !== "string" ||
        typeof newEmail !== "string" ||
        typeof confirmationCode !== "string"
      ) {
        message.error("Invalid current email, new email, or confirmation code");
        return;
      }
      withMutationHandler(
        () =>
          confirmUserEmailChangeMutation({
            variables: {
              input: {
                currentEmail: currentEmail,
                newEmail: newEmail,
                confirmationCode: confirmationCode,
              },
            },
          }),
        {
          onSuccess: () => {
            message.success("Email address change confirmed!");
            // TODO Only redirect to login page when user is currently logged out. Otherwise redirect to manage account page?
            return router.push(paths.openIdConnectClientLogin);
          },
          onError: messageErrors,
        },
      );
    };
    confirm();
  }, [
    confirmationCode,
    currentEmail,
    messageErrors,
    newEmail,
    router,
    withMutationHandler,
    message,
    confirmUserEmailChangeMutation,
  ]);

  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: "75ch" }}>Confirming email change ...</Typography.Paragraph>
    </Layout>
  );
}

export default Page;
