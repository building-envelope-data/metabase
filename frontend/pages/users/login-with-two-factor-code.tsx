import { useRouter } from "next/router";
import { initializeApollo } from "../../lib/apollo";
import { useLoginUserWithTwoFactorCodeMutation } from "../../queries/currentUser.graphql";
import {
  Alert,
  Form,
  Input,
  Button,
  Checkbox,
  Row,
  Col,
  Card,
  Typography,
} from "antd";
import Layout from "../../components/Layout";
import Link from "next/link";
import paths from "../../paths";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";

function LoginWithTwoFactorCode() {
  const router = useRouter();
  const returnTo = router.query.returnTo;
  const rememberMe = router.query.rememberMe;
  const apolloClient = initializeApollo();
  const [
    loginUserWithTwoFactorCodeMutation,
  ] = useLoginUserWithTwoFactorCodeMutation();
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [loggingIn, setLoggingIn] = useState(false);

  const onFinish = ({
    authenticatorCode,
    rememberMachine,
  }: {
    authenticatorCode: string;
    rememberMachine: boolean;
  }) => {
    const loginWithTwoFactorCode = async () => {
      try {
        setLoggingIn(true);
        const { errors, data } = await loginUserWithTwoFactorCodeMutation({
          variables: {
            authenticatorCode: authenticatorCode,
            rememberMachine: rememberMachine,
            rememberMe: rememberMe === "true",
          },
        });
        handleFormErrors(
          errors,
          data?.loginUserWithTwoFactorCode?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (
          !errors &&
          !data?.loginUserWithTwoFactorCode?.errors &&
          data?.loginUserWithTwoFactorCode?.user
        ) {
          await apolloClient.resetStore();
          await router.push(
            typeof returnTo === "string" ? returnTo : paths.home
          );
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setLoggingIn(false);
      }
    };
    loginWithTwoFactorCode();
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
              Your login is protected with an authenticator app. Enter your
              authenticator code below.
            </Typography.Paragraph>
            <Form
              form={form}
              name="basic"
              initialValues={{ rememberMachine: true }}
              onFinish={onFinish}
              onFinishFailed={onFinishFailed}
            >
              <Form.Item
                name="authenticatorCode"
                rules={[
                  {
                    required: true,
                    message: "Please input your authenticator code!",
                  },
                ]}
              >
                <Input placeholder="Authenticator Code" />
              </Form.Item>

              <Form.Item name="rememberMachine" valuePropName="checked" noStyle>
                <Checkbox>Remember machine</Checkbox>
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
                Don't have access to your authenticator device? You can{" "}
                <Link
                  href={{
                    pathname: paths.userLoginWithRecoveryCode,
                    query: returnTo ? { returnTo: returnTo } : null,
                  }}
                >
                  login with a recovery code
                </Link>
                .
              </Form.Item>
            </Form>
          </Card>
        </Col>
      </Row>
    </Layout>
  );
}

export default LoginWithTwoFactorCode;
