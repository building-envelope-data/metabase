import { useMutation } from "@apollo/client/react";
import { useRouter } from "next/router";
import { apolloClient } from "../../../lib/apollo";
import { LoginUserWithTwoFactorCodeDocument } from "../../../queries/currentUser.generated";
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
import SingleSignOnLayout from "../../../components/SingleSignOnLayout";
import Link from "next/link";
import paths from "../../../paths";
import { useState } from "react";
import { handleFormErrors } from "../../../lib/form";
import { isLocalUrl } from "../../../lib/url";

function LoginWithTwoFactorCode() {
  const router = useRouter();
  const returnTo = router.query.returnTo;
  const [loginUserWithTwoFactorCodeMutation] = useMutation(
    LoginUserWithTwoFactorCodeDocument,
  );
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
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
        const { error, data } = await loginUserWithTwoFactorCodeMutation({
          variables: {
            input: {
              authenticatorCode: authenticatorCode,
              rememberMachine: rememberMachine,
            },
          },
        });
        handleFormErrors(
          error,
          data?.loginUserWithTwoFactorCode?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form,
        );
        if (
          !error &&
          !data?.loginUserWithTwoFactorCode?.errors &&
          data?.loginUserWithTwoFactorCode?.user
        ) {
          await apolloClient.resetStore();
          await fetch(paths.antiforgeryToken);
          await router.push(
            typeof returnTo === "string" && isLocalUrl(returnTo)
              ? returnTo
              : paths.home,
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
    <SingleSignOnLayout>
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
                Don&apos;t have access to your authenticator device? You can{" "}
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
    </SingleSignOnLayout>
  );
}

export default LoginWithTwoFactorCode;
