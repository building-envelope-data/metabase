import { useEffect } from "react";
import { useRouter } from "next/router";
import { useConfirmUserEmailMutation } from "../../queries/users.graphql";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { message, Typography } from "antd";

function ConfirmUserEmail() {
  const router = useRouter();
  const { email, confirmationCode } = router.query;
  const [confirmUserEmailMutation] = useConfirmUserEmailMutation();

  useEffect(() => {
    const confirmUserEmail = async () => {
      if (router.isReady) {
        if (typeof email === "string" && typeof confirmationCode === "string") {
          const { errors } = await confirmUserEmailMutation({
            variables: {
              email: email,
              confirmationCode: confirmationCode,
            },
          });
          if (errors) {
            // TODO Report errors properly.
            message.error("Something went wrong!");
          }
          message.success("Email address confirmed!");
          await router.push(paths.userLogin);
        }
      }
    };
    confirmUserEmail();
  }, [email, confirmationCode]);

  return (
    <Layout>
      <Typography.Paragraph>Confirming email ...</Typography.Paragraph>
    </Layout>
  );
}

export default ConfirmUserEmail;
