import { useMutation } from "@apollo/client/react";
import { useEffect } from "react";
import { useRouter } from "next/router";
import { ConfirmUserEmailDocument } from "../../queries/users.generated";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { App, Typography } from "antd";

function ConfirmUserEmail() {
  const router = useRouter();
  const { email, confirmationCode, returnTo } = router.query;
  const [confirmUserEmailMutation] = useMutation(ConfirmUserEmailDocument);

  const { message } = App.useApp();

  useEffect(() => {
    const confirmUserEmail = async () => {
      if (router.isReady) {
        if (typeof email === "string" && typeof confirmationCode === "string") {
          const { error, data } = await confirmUserEmailMutation({
            variables: {
              input: {
                email: email,
                confirmationCode: confirmationCode,
              },
            },
          });
          if (error) {
            // TODO Report errors properly.
            console.log(error);
          } else if (data?.confirmUserEmail?.errors) {
            // TODO Is this how we want to display errors?
            message.error(
              data?.confirmUserEmail?.errors
                .map((error) => error.message)
                .join(" "),
            );
          } else {
            message.success("Email address confirmed!");
            await router.push({
              pathname: paths.openIdConnectClientLogin,
              query: returnTo ? { returnTo: returnTo } : {},
            });
          }
        }
      }
    };
    confirmUserEmail();
  }, [email, confirmationCode, returnTo, router, confirmUserEmailMutation]);

  return (
    <Layout>
      <Typography.Paragraph>Confirming email ...</Typography.Paragraph>
    </Layout>
  );
}

export default ConfirmUserEmail;
