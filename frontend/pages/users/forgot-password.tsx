import { useMutation } from "@apollo/client/react";
import {
  RequestUserPasswordResetDocument,
  RequestUserPasswordResetMutation,
} from "../../queries/users.generated";
import { Form, Input, Button, Row, Col, Card } from "antd";
import SingleSignOnLayout from "../../components/SingleSignOnLayout";
import { UserOutlined } from "@ant-design/icons";
import { useState } from "react";
import Link from "next/link";
import paths from "../../paths";
import { useRouter } from "next/router";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../../components/ErrorAlert";

interface FormValues {
  email: string;
}

function Page() {
  const router = useRouter();
  const returnTo = router.query.returnTo;
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
              returnTo: returnTo,
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
      <Row justify="center">
        <Col>
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
                    message: "Please input your email!",
                  },
                  {
                    type: "email",
                    message: "Invalid email!",
                  },
                ]}
              >
                <Input prefix={<UserOutlined />} placeholder="Email" />
              </Form.Item>
              <Form.Item>
                <Button
                  type="primary"
                  htmlType="submit"
                  loading={mutating}
                  style={{ width: "100%" }}
                >
                  Reset Password
                </Button>
                Or{" "}
                <Link
                  href={{
                    pathname: paths.openIdConnectClientLogin,
                    query: returnTo ? { returnTo: returnTo } : null,
                  }}
                >
                  Login instead!
                </Link>
              </Form.Item>
            </Form>
          </Card>
        </Col>
      </Row>
    </SingleSignOnLayout>
  );
}

export default Page;
