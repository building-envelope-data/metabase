import { useRouter } from "next/router";
import { initializeApollo } from "../../lib/apollo";
import { useLoginUserMutation } from "../../queries/currentUser.graphql";
import { Alert, Form, Input, Button, Checkbox, Row, Col, Card } from "antd";
import Layout from "../../components/Layout";
import Link from "next/link";
import { UserOutlined, LockOutlined } from "@ant-design/icons";
import paths from "../../paths";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";

function Login() {
  const router = useRouter();
  const returnTo = router.query.returnTo;
  const apolloClient = initializeApollo();
  const [loginUserMutation] = useLoginUserMutation();
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [loggingIn, setLoggingIn] = useState(false);

  const onFinish = ({
    email,
    password,
  }: {
    email: string;
    password: string;
  }) => {
    // TODO `rememberMe`
    const login = async () => {
      try {
        setLoggingIn(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await loginUserMutation({
          variables: {
            email: email,
            password: password,
          },
        });
        handleFormErrors(
          errors,
          data?.loginUser?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!errors && !data?.loginUser?.errors) {
          if (data?.loginUser?.requiresTwoFactor) {
            await router.push({
              pathname: paths.userSendTwoFactorCode,
              query: returnTo ? { returnTo: returnTo } : null,
            });
          }
          if (data?.loginUser?.user) {
            await apolloClient.resetStore();
            await router.push(
              typeof returnTo === "string" ? returnTo : paths.home
            );
          }
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
              initialValues={{ remember: true }}
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

              <Form.Item
                name="password"
                rules={[
                  {
                    required: true,
                    message: "Please input your password!",
                  },
                ]}
              >
                <Input.Password
                  prefix={<LockOutlined />}
                  placeholder="Password"
                />
              </Form.Item>

              <Form.Item>
                <Form.Item name="rememberMe" valuePropName="checked" noStyle>
                  <Checkbox>Remember me</Checkbox>
                </Form.Item>

                <Link
                  href={{
                    pathname: paths.userForgotPassword,
                    query: returnTo ? { returnTo: returnTo } : null,
                  }}
                  passHref
                >
                  <a style={{ float: "right" }}>Forgot password</a>
                </Link>
              </Form.Item>

              <Form.Item>
                <Button
                  type="primary"
                  htmlType="submit"
                  loading={loggingIn}
                  style={{ width: "100%" }}
                >
                  Login
                </Button>
                Or{" "}
                <Link
                  href={{
                    pathname: paths.userRegister,
                    query: returnTo ? { returnTo: returnTo } : null, // TODO Return to login page with `returnTo` parameter set as before? (because after registration user is not logged-in automatically)
                  }}
                >
                  Register now!
                </Link>
              </Form.Item>
            </Form>
          </Card>
        </Col>
      </Row>
    </Layout>
  );
}

export default Login;
