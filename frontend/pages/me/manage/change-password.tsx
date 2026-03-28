import { useMutation } from "@apollo/client/react";
import ManageLayout from "../../../components/me/ManageLayout";
import { Input, Button, App, Form } from "antd";
import {
  ChangeUserPasswordDocument,
  ChangeUserPasswordMutation,
} from "../../../queries/currentUser.generated";
import { useState } from "react";
import { layout, tailLayout } from "../../../lib/form";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";
import ErrorAlert from "../../../components/ErrorAlert";

type FormValues = {
  currentPassword: string;
  newPassword: string;
  newPasswordConfirmation: string;
};

function Page() {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();
  const { message } = App.useApp();

  const [changeUserPasswordMutation] = useMutation(ChangeUserPasswordDocument);

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<ChangeUserPasswordMutation>({
      getErrors: (data) => data.changeUserPassword.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        changeUserPasswordMutation({
          variables: {
            input: {
              currentPassword: values.currentPassword,
              newPassword: values.newPassword,
              newPasswordConfirmation: values.newPasswordConfirmation,
            },
          },
        }),
      {
        onSuccess: () => {
          message.success("Your password has been changed.");
          setGlobalErrorMessages([]);
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
      <ErrorAlert messages={globalErrorMessages} />
      <Form
        {...layout}
        form={form}
        name="basic"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Current Password"
          name="currentPassword"
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
          label="New Password"
          name="newPassword"
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
          label="Confirm New Password"
          name="newPasswordConfirmation"
          dependencies={["newPassword"]}
          rules={[
            {
              required: true,
              message: "Please input your password!",
            },
            ({ getFieldValue }) => ({
              validator(_, value) {
                if (!value || getFieldValue("newPassword") === value) {
                  return Promise.resolve();
                }
                return Promise.reject(
                  "New password and confirmation do not match!",
                );
              },
            }),
          ]}
        >
          <Input.Password />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Change Password
          </Button>
        </Form.Item>
      </Form>
    </ManageLayout>
  );
}

export default Page;
