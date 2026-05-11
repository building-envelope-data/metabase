import { useMutation } from "@apollo/client/react";
import { useRouter } from "next/router";
import { apolloClient } from "../../../lib/apollo";
import {
  LoginUserWithRecoveryCodeDocument,
  LoginUserWithRecoveryCodeMutation,
} from "../../../queries/currentUser.generated";
import { Form, Input, Button, Card, Typography } from "antd";
import SingleSignOnLayout from "../../../components/SingleSignOnLayout";
import paths from "../../../paths";
import { useState } from "react";
import { isLocalUrl } from "../../../lib/url";
import Link from "next/link";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";
import ErrorAlert from "../../../components/ErrorAlert";

interface FormValues {
  recoveryCode: string;
}

function LoginWithRecoveryCode() {
  const router = useRouter();
  const returnTo = router.query.returnTo;
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();

  const [loginUserWithRecoveryCodeMutation] = useMutation(
    LoginUserWithRecoveryCodeDocument,
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<LoginUserWithRecoveryCodeMutation>({
      getErrors: (data) => data.loginUserWithRecoveryCode.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        loginUserWithRecoveryCodeMutation({
          variables: {
            input: {
              recoveryCode: values.recoveryCode,
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
      <Card title="Login">
        <ErrorAlert messages={globalErrorMessages} />
        <Typography.Paragraph style={{ maxWidth: "75ch" }}>
          You have requested to log in with a recovery code. This login will not
          be remembered until you provide an authenticator app code at log in or
          disable two-factor authentication and log in again.
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
              loading={mutating}
              style={{ width: "100%" }}
            >
              Login
            </Button>
            Don&apos;t have access to your recovery code? You can{" "}
            <Link
              href={{
                pathname: paths.userLoginWithTwoFactorCode,
                query: returnTo ? { returnTo: returnTo } : null,
              }}
            >
              login with a two-factor code
            </Link>
            .
          </Form.Item>
        </Form>
      </Card>
    </SingleSignOnLayout>
  );
}

export default LoginWithRecoveryCode;
