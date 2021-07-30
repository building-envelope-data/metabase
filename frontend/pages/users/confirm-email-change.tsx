import { useEffect } from "react";
import { useRouter } from "next/router";
import { useConfirmUserEmailChangeMutation } from "../../queries/users.graphql";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { message, Typography } from "antd";

function Page() {
  const router = useRouter();
  const { currentEmail, newEmail, confirmationCode } = router.query;
  const [confirmUserEmailChangeMutation] = useConfirmUserEmailChangeMutation();

  useEffect(() => {
    const confirm = async () => {
      if (router.isReady) {
        if (
          typeof currentEmail == "string" &&
          typeof newEmail === "string" &&
          typeof confirmationCode === "string"
        ) {
          const { errors, data } = await confirmUserEmailChangeMutation({
            variables: {
              currentEmail: currentEmail,
              newEmail: newEmail,
              confirmationCode: confirmationCode,
            },
          });
          if (errors) {
            // TODO Report errors properly.
            console.log(errors);
          } else if (data?.confirmUserEmailChange?.errors) {
            // TODO Is this how we want to display errors?
            message.error(
              data?.confirmUserEmailChange?.errors
                .map((error) => error.message)
                .join(" ")
            );
          } else {
            message.success("Email address change confirmed!");
            // TODO Only redirect to login page when user is currently logged out. Otherwise redirect to manage account page?
            await router.push(paths.userLogin);
          }
        }
      }
    };
    confirm();
  }, [
    router,
    confirmUserEmailChangeMutation,
    currentEmail,
    newEmail,
    confirmationCode,
  ]);

  return (
    <Layout>
      <Typography.Paragraph>Confirming email change ...</Typography.Paragraph>
    </Layout>
  );
}

export default Page;
