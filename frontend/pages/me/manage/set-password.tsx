import ManageLayout from "../../../components/me/ManageLayout";
import { Alert, Input, Button, message, Form, Typography } from "antd";
import { useSetUserPasswordMutation } from "../../../queries/currentUser.graphql";
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
  const [changeUserPasswordMutation] = useSetUserPasswordMutation();

  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [setting, setSetting] = useState(false);

  const onFinish = ({
    password,
    passwordConfirmation,
  }: {
    password: string;
    passwordConfirmation: string;
  }) => {
    const change = async () => {
      try {
        setSetting(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await changeUserPasswordMutation({
          variables: {
            password: password,
            passwordConfirmation: passwordConfirmation,
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
          message.success("Your password has been set.");
          form.resetFields();
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setSetting(false);
      }
    };
    change();
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
                  "password and confirmation do not match!"
                );
              },
            }),
          ]}
        >
          <Input.Password />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={setting}>
            Set Password
          </Button>
        </Form.Item>
      </Form>
    </ManageLayout>
  );
}

export default Page;
