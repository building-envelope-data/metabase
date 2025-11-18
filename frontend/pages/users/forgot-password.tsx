import { useMutation } from '@apollo/client/react';
import { RequestUserPasswordResetDocument } from "../../queries/users.generated";
import { Alert, Form, Input, Button, Row, Col, Card } from "antd";
import SingleSignOnLayout from "../../components/SingleSignOnLayout";
import { UserOutlined } from "@ant-design/icons";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import Link from "next/link";
import paths from "../../paths";
import { useRouter } from "next/router";

function Page() {
  const router = useRouter();
  const returnTo = router.query.returnTo;
  const [requestUserPasswordResetMutation] = useMutation(RequestUserPasswordResetDocument);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [requesting, setLoggingIn] = useState(false);

  const onFinish = ({ email }: { email: string }) => {
    const login = async () => {
      try {
        setLoggingIn(true);
        const { error, data } = await requestUserPasswordResetMutation({
          variables: {
            email: email,
            returnTo: returnTo,
          },
        });
        handleFormErrors(
          error,
          data?.requestUserPasswordReset?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!error && !data?.requestUserPasswordReset?.errors) {
          await router.push({
            pathname: paths.userCheckYourInboxAfterPasswordResetRequest,
            query: returnTo ? { returnTo: returnTo } : {},
          });
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setLoggingIn(false);
      }
    };
    login();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <SingleSignOnLayout>
      <Row justify="center">
        <Col>
          <Card title="Forgot Password">
            {/* Display error messages in a list? */}
            {globalErrorMessages.length > 0 ? (
              <Alert type="error" message={globalErrorMessages.join(" ")} />
            ) : (
              <></>
            )}
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
                  loading={requesting}
                  style={{ width: "100%" }}
                >
                  Reset Password
                </Button>
                Or{" "}
                <Link
                  href={{
                    pathname: paths.userLogin,
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
