import { useRouter } from "next/router";
import { useRequestUserPasswordResetMutation } from "../../queries/users.graphql";
import { Alert, Form, Input, Button, Row, Col, Card, message } from "antd";
import Layout from "../../components/Layout";
import { UserOutlined } from "@ant-design/icons";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";

function Page() {
  const router = useRouter();
  const [
    requestUserPasswordResetMutation,
  ] = useRequestUserPasswordResetMutation();
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [requesting, setLoggingIn] = useState(false);

  const onFinish = ({ email }: { email: string }) => {
    const login = async () => {
      try {
        setLoggingIn(true);
        const { errors, data } = await requestUserPasswordResetMutation({
          variables: {
            email: email,
          },
        });
        handleFormErrors(
          errors,
          data?.requestUserPasswordReset?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!errors && !data?.requestUserPasswordReset?.errors) {
          message.success("Please check your email to reset your password.");
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
                  Login
                </Button>
              </Form.Item>
            </Form>
          </Card>
        </Col>
      </Row>
    </Layout>
  );
}

export default Page;
