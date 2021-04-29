import ManageLayout from "../../../components/me/ManageLayout";
import { Alert, Input, Button, message, Form } from "antd";
import { useChangeUserPasswordMutation } from "../../../queries/currentUser.graphql";
import { handleFormErrors } from "../../../lib/form";
import { useState } from "react";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

function Page() {
  const [changeUserPasswordMutation] = useChangeUserPasswordMutation();

  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [changing, setChanging] = useState(false);

  const onFinish = ({
    currentPassword,
    newPassword,
    newPasswordConfirmation,
  }: {
    currentPassword: string;
    newPassword: string;
    newPasswordConfirmation: string;
  }) => {
    const change = async () => {
      try {
        setChanging(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await changeUserPasswordMutation({
          variables: {
            currentPassword: currentPassword,
            newPassword: newPassword,
            newPasswordConfirmation: newPasswordConfirmation,
          },
        });
        handleFormErrors(
          errors,
          data?.changeUserPassword?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!errors && !data?.changeUserPassword?.errors) {
          message.success("Your password has been changed.");
          form.resetFields();
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setChanging(false);
      }
    };
    change();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <ManageLayout>
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
                  "New password and confirmation do not match!"
                );
              },
            }),
          ]}
        >
          <Input.Password />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={changing}>
            Change Password
          </Button>
        </Form.Item>
      </Form>
    </ManageLayout>
  );
}

export default Page;
