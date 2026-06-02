import { useMutation } from "@apollo/client/react";
import { useRouter } from "next/router";
import {
  ResendUserEmailConfirmationDocument,
  ResendUserEmailConfirmationMutation,
} from "../../queries/users.generated";
import paths from "../../paths";
import { Button, Form, Input, Card } from "antd";
import { useState } from "react";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../../components/ErrorAlert";
import SingleSignOnLayout from "../../components/SingleSignOnLayout";

interface FormValues {
  email: string;
}

export default function Page() {
  const router = useRouter();
  const { returnTo } = router.query;

  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();

  const [resendUserEmailConfirmationMutation] = useMutation(
    ResendUserEmailConfirmationDocument,
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<ResendUserEmailConfirmationMutation>({
      getErrors: (data) => data.resendUserEmailConfirmation.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        resendUserEmailConfirmationMutation({
          variables: {
            input: {
              email: values.email,
            },
          },
        }),
      {
        onSuccess: () => {
          return router.push({
            pathname: paths.userCheckYourInboxAfterResendingEmailConfirmation,
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
      <Card title="Resend Email Confirmation">
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
                message: "Please input your email address",
              },
              {
                type: "email",
                message: "Invalid email address",
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item {...tailLayout}>
            <Button type="primary" htmlType="submit" loading={mutating}>
              Resend email confirmation
            </Button>
          </Form.Item>
        </Form>
      </Card>
    </SingleSignOnLayout>
  );
}
