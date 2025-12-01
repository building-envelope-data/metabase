import { useMutation } from "@apollo/client/react";
import { useEffect } from "react";
import { useRouter } from "next/router";
import { ConfirmUserEmailChangeDocument } from "../../queries/users.generated";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { message, Typography } from "antd";

function Page() {
  const router = useRouter();
  const { currentEmail, newEmail, confirmationCode } = router.query;
  const [confirmUserEmailChangeMutation] = useMutation(
    ConfirmUserEmailChangeDocument,
  );
  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    const confirm = async () => {
      if (router.isReady) {
        if (
          typeof currentEmail == "string" &&
          typeof newEmail === "string" &&
          typeof confirmationCode === "string"
        ) {
          const { error, data } = await confirmUserEmailChangeMutation({
            variables: {
              input: {
                currentEmail: currentEmail,
                newEmail: newEmail,
                confirmationCode: confirmationCode,
              },
            },
          });
          if (error) {
            // TODO Report errors properly.
            console.log(error);
          } else if (data?.confirmUserEmailChange?.errors) {
            // TODO Is this how we want to display errors?
            messageApi.error(
              data?.confirmUserEmailChange?.errors
                .map((error) => error.message)
                .join(" "),
            );
          } else {
            messageApi.success("Email address change confirmed!");
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
      {contextHolder}
      <Typography.Paragraph>Confirming email change ...</Typography.Paragraph>
    </Layout>
  );
}

export default Page;
