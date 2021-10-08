import { useRouter } from "next/router";
import { initializeApollo } from "../../lib/apollo";
import { useRegisterUserMutation } from "../../queries/users.graphql";
import { Alert, Form, Input, Button, Row, Col, Card, Typography } from "antd";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import Link from "next/link";
import { Scalars } from "../../__generated__/__types__";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

function Register() {
  const router = useRouter();
  const apolloClient = initializeApollo();
  const [registerUserMutation] = useRegisterUserMutation();
  const returnTo = router.query.returnTo;
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [registering, setRegistering] = useState(false);

  const onFinish = ({
    name,
    email,
    password,
    passwordConfirmation,
  }: {
    name: string;
    email: Scalars["EmailAddress"];
    password: string;
    passwordConfirmation: string;
  }) => {
    const register = async () => {
      try {
        setRegistering(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        await apolloClient.resetStore();
        const { errors, data } = await registerUserMutation({
          variables: {
            name: name,
            email: email,
            password: password,
            passwordConfirmation: passwordConfirmation,
          },
        });
        handleFormErrors(
          errors,
          data?.registerUser?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (
          !errors &&
          !data?.registerUser?.errors &&
          data?.registerUser?.user
        ) {
          // TODO return to does not work like this because user is not automatically logged-in after registering. Return-to would need to be in the confirmation link sent via email ...
          await router.push(
            typeof returnTo === "string" ? returnTo : paths.userCheckYourInbox
          );
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setRegistering(false);
      }
    };
    register();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <Layout>
      <Row justify="center">
        <Col>
          <Card title="Register">
            <Typography.Paragraph style={{ maxWidth: 768 }}>
              No account is needed to query the{" "}
              <Link href={paths.data}>data</Link> for free! However, if you want
              to change information about{" "}
              <Link href={paths.institutions}>institutions</Link>,{" "}
              <Link href={paths.dataFormats}>data formats</Link>,{" "}
              <Link href={paths.methods}>methods</Link>,{" "}
              <Link href={paths.components}>components</Link> or{" "}
              <Link href={paths.databases}>databases</Link>, you can register
              here.
            </Typography.Paragraph>
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
                label="Name"
                name="name"
                rules={[
                  {
                    required: true,
                  },
                ]}
              >
                <Input />
              </Form.Item>

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
                <Button type="primary" htmlType="submit" loading={registering}>
                  Register
                </Button>
              </Form.Item>
            </Form>
            <Typography.Paragraph style={{ maxWidth: 768 }}>
              With sufficient privileges, the{" "}
              <Typography.Link
                href={`${process.env.NEXT_PUBLIC_METABASE_URL}/graphql/`}
              >
                GraphQL endpoint
              </Typography.Link>{" "}
              can also be used to register users.
            </Typography.Paragraph>
          </Card>
        </Col>
      </Row>
    </Layout>
  );
}

export default Register;
