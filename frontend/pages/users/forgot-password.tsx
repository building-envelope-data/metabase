import { useMutation } from "@apollo/client/react";
import {
  RequestUserPasswordResetDocument,
  RequestUserPasswordResetMutation,
} from "../../queries/users.generated";
import { Form, Input, Button, Card } from "antd";
import SingleSignOnLayout from "../../components/SingleSignOnLayout";
import { UserOutlined } from "@ant-design/icons";
import { useState } from "react";
import Link from "next/link";
import paths from "../../paths";
import { useRouter } from "next/router";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../../components/ErrorAlert";
import { Scalars } from "../../__generated__/graphql";

interface FormValues {
  email: Scalars["EmailAddress"]["input"];
}

function Page() {
  const router = useRouter();
  const { returnTo } = router.query;
  const [requestUserPasswordResetMutation] = useMutation(
    RequestUserPasswordResetDocument,
  );
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<RequestUserPasswordResetMutation>({
      getErrors: (data) => data.requestUserPasswordReset.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        requestUserPasswordResetMutation({
          variables: {
            input: {
              email: values.email,
              returnTo: returnTo?.toString(),
            },
          },
        }),
      {
        onSuccess: () =>
          router.push({
            pathname: paths.userCheckYourInboxAfterPasswordResetRequest,
            query: returnTo ? { returnTo: returnTo } : {},
          }),
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
      <Card title="Forgot Password">
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
          <Form.Item>
            <Form.Item>
              <Button
                type="primary"
                htmlType="submit"
                loading={mutating}
                style={{ width: "100%" }}
              >
                Request Password Reset
              </Button>
            </Form.Item>
            <div style={{ float: "right" }}>
              or{" "}
              <Link
                href={{
                  pathname: paths.openIdConnectClientLogin,
                  query: returnTo ? { returnTo: returnTo } : null,
                }}
              >
                login instead!
              </Link>
            </div>
          </Form.Item>
        </Form>
      </Card>
    </SingleSignOnLayout>
  );
}

export default Page;
