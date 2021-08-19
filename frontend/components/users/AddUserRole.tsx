import {
  UserDocument,
  UsersDocument,
  useAddUserRoleMutation,
} from "../../queries/users.graphql";
import { Scalars, UserRole } from "../../__generated__/__types__";
import { Alert, Form, Button, Select } from "antd";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export type AddUserRoleProps = {
  userId: Scalars["Uuid"];
  roles: UserRole[];
};

export default function AddUserRole({ userId, roles }: AddUserRoleProps) {
  const [addUserRoleMutation] = useAddUserRoleMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: UsersDocument,
      },
      {
        query: UserDocument,
        variables: { uuid: userId },
      },
    ],
  });
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [adding, setAdding] = useState(false);

  const onFinish = ({ role }: { role: UserRole }) => {
    const add = async () => {
      try {
        setAdding(true);
        const { errors, data } = await addUserRoleMutation({
          variables: {
            userId: userId,
            role: role,
          },
        });
        handleFormErrors(
          errors,
          data?.addUserRole?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setAdding(false);
      }
    };
    add();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <>
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
          label="Role"
          name="role"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Select
            options={roles.map((role) => ({
              label: role,
              value: role,
            }))}
          />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={adding}>
            Add
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
