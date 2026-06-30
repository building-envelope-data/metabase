import { useMutation } from "@apollo/client/react";
import { useRouter } from "next/router";
import {
  ResetUserPasswordDocument,
  ResetUserPasswordMutation,
} from "../../queries/users.generated";
import SingleSignOnLayout from "../../components/SingleSignOnLayout";
import paths from "../../paths";
import { Button, Form, Input, App, Card } from "antd";
import { useState } from "react";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../../components/ErrorAlert";
import { Scalars } from "../../__generated__/graphql";

interface FormValues {
  email: Scalars["EmailAddress"]["input"];
  password: string;
  passwordConfirmation: string;
}

function Page() {
  const router = useRouter();
  const { resetCode, returnTo } = router.query;

  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();
  const { message } = App.useApp();

  const [resetUserPasswordMutation] = useMutation(ResetUserPasswordDocument);

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<ResetUserPasswordMutation>({
      getErrors: (data) => data.resetUserPassword.errors,
    });

  const onFinish = (values: FormValues) => {
    if (typeof resetCode != "string") {
      message.error("Invalid reset code");
      return;
    }
    withMutationHandler(
      () =>
        resetUserPasswordMutation({
          variables: {
            input: {
              email: values.email,
              resetCode: String(resetCode),
              password: values.password,
              passwordConfirmation: values.passwordConfirmation,
            },
          },
        }),
      {
        onSuccess: () => {
          message.success("Your password was reset.");
          return router.push({
            pathname: paths.openIdConnectClientLogin,
            query: returnTo ? { returnTo: returnTo } : {},
          });
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
      <Card title="Reset Password">
        <ErrorAlert messages={globalErrorMessages} />
        <Form
          {...layout}
          form={form}
          name="basic"
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
        >
          <Form.Item
            label="Email"
            name="email"
            rules={[
              {
                required: true,
                message: "Please input your email!",
              },
              {
                type: "email",
                message: "Invalid email!",
              },
            ]}
          >
            <Input />
          </Form.Item>

          <Form.Item
            label="Password"
            name="password"
            rules={[
              {
                required: true,
                message: "Please input your password!",
              },
            ]}
          >
            <Input.Password />
          </Form.Item>

          <Form.Item
            label="Confirm Password"
            name="passwordConfirmation"
            dependencies={["password"]}
            rules={[
              {
                required: true,
                message: "Please input your password!",
              },
              ({ getFieldValue }) => ({
                validator(_, value) {
                  if (!value || getFieldValue("password") === value) {
                    return Promise.resolve();
                  }
                  return Promise.reject(
                    "Password and confirmation do not match!",
                  );
                },
              }),
            ]}
          >
            <Input.Password />
          </Form.Item>

          <Form.Item {...tailLayout}>
            <Button type="primary" htmlType="submit" loading={mutating}>
              Reset password
            </Button>
          </Form.Item>
        </Form>
      </Card>
    </SingleSignOnLayout>
  );
}

export default Page;
