import { useRouter } from "next/router";
import { useResetUserPasswordMutation } from "../../queries/users.graphql";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { Button, Alert, Form, Input, message, Card, Col, Row } from "antd";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { Scalars } from "../../__generated__/__types__";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

function Page() {
  const router = useRouter();
  const { resetCode } = router.query;
  const [resetUserPasswordMutation] = useResetUserPasswordMutation();

  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [resetting, setResetting] = useState(false);

  const onFinish = ({
    email,
    password,
    passwordConfirmation,
  }: {
    email: Scalars["EmailAddress"];
    password: string;
    passwordConfirmation: string;
  }) => {
    const reset = async () => {
      // TODO Report error when `resetCode` is not a string!
      if (typeof resetCode == "string") {
        try {
          setResetting(true);
          const { errors, data } = await resetUserPasswordMutation({
            variables: {
              email: email,
              resetCode: resetCode,
              password: password,
              passwordConfirmation: passwordConfirmation,
            },
          });
          handleFormErrors(
            errors,
            data?.resetUserPassword?.errors?.map((x) => {
              return { code: x.code, message: x.message, path: x.path };
            }),
            setGlobalErrorMessages,
            form
          );
          if (!errors && !data?.resetUserPassword?.errors) {
            message.success("Your password was reset.");
            await router.push(paths.userLogin);
          }
        } catch (error) {
          // TODO Handle properly.
          console.log("Failed:", error);
        } finally {
          setResetting(false);
        }
      }
    };
    reset();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <Layout>
      <Row justify="center">
        <Col>
          <Card title="Register">
            {/* TODO Display error messages in a list? */}
            {globalErrorMessages.length > 0 ? (
              <Alert type="error" message={globalErrorMessages.join(" ")} />
            ) : (
              <></>
            )}
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
                        "Password and confirmation do not match!"
                      );
                    },
                  }),
                ]}
              >
                <Input.Password />
              </Form.Item>

              <Form.Item {...tailLayout}>
                <Button type="primary" htmlType="submit" loading={resetting}>
                  Reset password
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
