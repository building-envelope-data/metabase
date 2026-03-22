import { useMutation } from "@apollo/client/react";
import { useRouter } from "next/router";
import { apolloClient } from "../../../lib/apollo";
import {
  LoginUserWithTwoFactorCodeDocument,
  LoginUserWithTwoFactorCodeMutation,
} from "../../../queries/currentUser.generated";
import {
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
import { isLocalUrl } from "../../../lib/url";
import ErrorAlert from "../../../components/ErrorAlert";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";

interface FormValues {
  authenticatorCode: string;
  rememberMachine: boolean;
}

function LoginWithTwoFactorCode() {
  const router = useRouter();
  const returnTo = router.query.returnTo;
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();

  const [loginUserWithTwoFactorCodeMutation] = useMutation(
    LoginUserWithTwoFactorCodeDocument,
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<LoginUserWithTwoFactorCodeMutation>({
      getErrors: (data) => data.loginUserWithTwoFactorCode.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        loginUserWithTwoFactorCodeMutation({
          variables: {
            input: {
              authenticatorCode: values.authenticatorCode,
              rememberMachine: values.rememberMachine,
            },
          },
        }),
      {
        onSuccess: async () => {
          await apolloClient.resetStore();
          await fetch(paths.antiforgeryToken);
          await router.push(
            typeof returnTo === "string" && isLocalUrl(returnTo)
              ? returnTo
              : paths.home,
          );
        },
        onError: (graphQlErrors, userErrors) =>
          setGlobalErrorMessages(
            augmentFormWithErrors(graphQlErrors, userErrors, form),
          ),
      },
    );
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <SingleSignOnLayout>
      <Row justify="center">
        <Col>
          <Card title="Login">
            <ErrorAlert messages={globalErrorMessages} />
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
                  loading={mutating}
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
