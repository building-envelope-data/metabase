import { useMutation } from "@apollo/client/react";
import { useRouter } from "next/router";
import { apolloClient } from "../../lib/apollo";
import {
  RegisterUserDocument,
  RegisterUserMutation,
} from "../../queries/users.generated";
import { Form, Input, Button, Card, Typography, Divider } from "antd";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { useState } from "react";
import Link from "next/link";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../../components/ErrorAlert";

interface FormValues {
  name: string;
  email: string;
  password: string;
  passwordConfirmation: string;
}

export default function Page() {
  const router = useRouter();
  const { returnTo } = router.query;
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();

  const [registerUserMutation] = useMutation(RegisterUserDocument);

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<RegisterUserMutation>({
      getErrors: (data) => data.registerUser.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      async () => {
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        await apolloClient.resetStore();
        return await registerUserMutation({
          variables: {
            input: {
              name: values.name,
              email: values.email,
              password: values.password,
              passwordConfirmation: values.passwordConfirmation,
              returnTo: returnTo?.toString(),
            },
          },
        });
      },
      {
        onSuccess: () =>
          router.push({
            pathname: paths.userCheckYourInboxAfterRegistration,
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
    <Layout>
      <Card title="Register">
        <Typography.Paragraph style={{ maxWidth: "75ch" }}>
          No account is needed to query the{" "}
          <Link href={paths.allData}>data</Link> for free! However, if you want
          to change information about{" "}
          <Link href={paths.institutions}>institutions</Link>,{" "}
          <Link href={paths.dataFormats}>data formats</Link>,{" "}
          <Link href={paths.methods}>methods</Link>,{" "}
          <Link href={paths.components}>components</Link> or{" "}
          <Link href={paths.databases}>databases</Link>, you can register here.
        </Typography.Paragraph>
        <ErrorAlert messages={globalErrorMessages} />
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
              Register
            </Button>
          </Form.Item>
        </Form>
        <Divider />
        <div style={{ textAlign: "center" }}>
          <Link
            href={{
              pathname: paths.userResendEmailConfirmation,
              query: returnTo ? { returnTo: returnTo } : null,
            }}
          >
            Have you registered but not received a confirmation email?
          </Link>
        </div>
      </Card>
    </Layout>
  );
}
