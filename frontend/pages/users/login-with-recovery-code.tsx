import { useRouter } from "next/router";
import { initializeApollo } from "../../lib/apollo";
import { useLoginUserWithRecoveryCodeMutation } from "../../queries/currentUser.graphql";
import { Alert, Form, Input, Button, Row, Col, Card, Typography } from "antd";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { isLocalUrl } from "../../lib/url";

function LoginWithRecoveryCode() {
  const router = useRouter();
  const returnTo = router.query.returnTo;
  const apolloClient = initializeApollo();
  const [loginUserWithRecoveryCodeMutation] =
    useLoginUserWithRecoveryCodeMutation();
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [loggingIn, setLoggingIn] = useState(false);

  const onFinish = ({ recoveryCode }: { recoveryCode: string }) => {
    const loginWithRecoveryCode = async () => {
      try {
        setLoggingIn(true);
        const { errors, data } = await loginUserWithRecoveryCodeMutation({
          variables: {
            recoveryCode: recoveryCode,
          },
        });
        handleFormErrors(
          errors,
          data?.loginUserWithRecoveryCode?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (
          !errors &&
          !data?.loginUserWithRecoveryCode?.errors &&
          data?.loginUserWithRecoveryCode?.user
        ) {
          await apolloClient.resetStore();
          await fetch(paths.antiforgeryToken);
          await router.push(
            typeof returnTo === "string" && isLocalUrl(returnTo)
              ? returnTo
              : paths.home
          );
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setLoggingIn(false);
      }
    };
    loginWithRecoveryCode();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <Layout>
      <Row justify="center">
        <Col>
          <Card title="Login">
            {/* Display error messages in a list? */}
            {globalErrorMessages.length > 0 ? (
              <Alert type="error" message={globalErrorMessages.join(" ")} />
            ) : (
              <></>
            )}
            <Typography.Paragraph>
              You have requested to log in with a recovery code. This login will
              not be remembered until you provide an authenticator app code at
              log in or disable two-factor authentication and log in again.
            </Typography.Paragraph>
            <Form
              form={form}
              name="basic"
              onFinish={onFinish}
              onFinishFailed={onFinishFailed}
            >
              <Form.Item
                name="recoveryCode"
                rules={[
                  {
                    required: true,
                    message: "Please input your recovery code!",
                  },
                ]}
              >
                <Input placeholder="Recovery Code" />
              </Form.Item>

              <Form.Item>
                <Button
                  type="primary"
                  htmlType="submit"
                  loading={loggingIn}
                  style={{ width: "100%" }}
                >
                  Login
                </Button>
              </Form.Item>
            </Form>
          </Card>
        </Col>
      </Row>
    </Layout>
  );
}

export default LoginWithRecoveryCode;
