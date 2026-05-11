import { useMutation } from "@apollo/client/react";
import { useRouter } from "next/router";
import { apolloClient } from "../../../lib/apollo";
import {
  LoginUserDocument,
  LoginUserMutation,
} from "../../../queries/currentUser.generated";
import { Form, Input, Button, Card, Divider } from "antd";
import SingleSignOnLayout from "../../../components/SingleSignOnLayout";
import Link from "next/link";
import { UserOutlined, LockOutlined } from "@ant-design/icons";
import paths from "../../../paths";
import { isLocalUrl } from "../../../lib/url";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";
import ErrorAlert from "../../../components/ErrorAlert";
import { useState } from "react";

type FormValues = {
  email: string;
  password: string;
};

function Login() {
  const router = useRouter();
  const returnTo = router.query.returnTo;
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [loginUserMutation] = useMutation(LoginUserDocument);

  const {
    mutating,
    withMutationHandler,
    messageMissingModel,
    augmentFormWithErrors,
  } = useMutationHandler<LoginUserMutation>({
    getErrors: (data) => data.loginUser.errors,
  });

  const onFinish = ({ email, password }: FormValues) => {
    withMutationHandler(
      () =>
        loginUserMutation({
          variables: {
            input: {
              email: email,
              password: password,
            },
          },
        }),
      {
        onSuccess: async (data) => {
          const payload = data?.loginUser;
          if (!payload?.requiresTwoFactor && !payload?.user) {
            setGlobalErrorMessages([]);
            messageMissingModel();
          } else {
            if (payload.requiresTwoFactor) {
              await router.push({
                pathname: paths.userLoginWithTwoFactorCode,
                query: returnTo ? { returnTo: returnTo } : {},
              });
            } else {
              await apolloClient.resetStore();
              await fetch(paths.antiforgeryToken);
              await router.push(
                typeof returnTo === "string" && isLocalUrl(returnTo)
                  ? returnTo
                  : paths.home,
              );
            }
          }
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
      <Card title="Login">
        <ErrorAlert messages={globalErrorMessages} />
        <Form
          form={form}
          name="basic"
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
        >
          <Form.Item
            name="email"
            rules={[
              {
                required: true,
                message: "Please input your email address",
              },
              {
                type: "email",
                message: "Invalid email address",
              },
            ]}
          >
            <Input prefix={<UserOutlined />} placeholder="Email" />
          </Form.Item>
          <Form.Item
            name="password"
            rules={[
              {
                required: true,
                message: "Please input your password",
              },
            ]}
          >
            <Input.Password prefix={<LockOutlined />} placeholder="Password" />
            <Link
              href={{
                pathname: paths.userForgotPassword,
                query: returnTo ? { returnTo: returnTo } : null,
              }}
              style={{ float: "right" }}
            >
              Forgot password?
            </Link>
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
            <div style={{ float: "right" }}>
              or{" "}
              <Link
                href={{
                  pathname: paths.userRegister,
                  query: returnTo ? { returnTo: returnTo } : null,
                }}
              >
                register now!
              </Link>
            </div>
          </Form.Item>
        </Form>
        <Divider />
        <div style={{ textAlign: "center" }}>
          <Link
            href={{
              pathname: paths.userResendEmailConfirmation,
              query: returnTo ? { returnTo: returnTo } : null,
            }}
          >
            Have you registered but not received a confirmation email?
          </Link>
        </div>
      </Card>
    </SingleSignOnLayout>
  );
}

export default Login;
