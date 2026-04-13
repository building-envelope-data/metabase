import { useMutation } from "@apollo/client/react";
import ManageLayout from "../../../components/me/ManageLayout";
import { Input, Button, App, Form, Typography } from "antd";
import {
  SetUserPasswordDocument,
  SetUserPasswordMutation,
} from "../../../queries/currentUser.generated";
import { useState } from "react";
import { layout, tailLayout } from "../../../lib/form";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";
import ErrorAlert from "../../../components/ErrorAlert";

interface FormValues {
  password: string;
  passwordConfirmation: string;
}

function Page() {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();
  const { message } = App.useApp();

  const [setUserPasswordMutation] = useMutation(SetUserPasswordDocument);

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<SetUserPasswordMutation>({
      getErrors: (data) => data.setUserPassword.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        setUserPasswordMutation({
          variables: {
            input: {
              password: values.password,
              passwordConfirmation: values.passwordConfirmation,
            },
          },
        }),
      {
        onSuccess: () => {
          message.success("Your password has been set.");
          form.resetFields();
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
    <ManageLayout>
      <Typography.Paragraph>
        You do not have a local username/password for this site. Add a local
        account so you can log in without an external login.
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
                  "password and confirmation do not match!",
                );
              },
            }),
          ]}
        >
          <Input.Password />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Set Password
          </Button>
        </Form.Item>
      </Form>
    </ManageLayout>
  );
}

export default Page;
