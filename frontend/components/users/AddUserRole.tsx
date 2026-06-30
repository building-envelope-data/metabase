import { useMutation } from "@apollo/client/react";
import {
  AddUserRoleDocument,
  AddUserRoleMutation,
} from "../../queries/users.generated";
import { Scalars, UserRole } from "../../__generated__/graphql";
import { Form, Button, Select, Space } from "antd";
import { useState } from "react";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";
import { humanize } from "../../lib/string";

type FormValues = { role: UserRole };

interface AddUserRoleProps {
  userId: Scalars["Uuid"]["input"];
  roles: UserRole[];
}

export default function AddUserRole({ userId, roles }: AddUserRoleProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [addUserRoleMutation] = useMutation(AddUserRoleDocument);

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<AddUserRoleMutation>({
      getErrors: (data) => data.addUserRole.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        addUserRoleMutation({
          variables: {
            input: {
              userId: userId,
              role: values.role,
            },
          },
        }),
      {
        onSuccess: () => {
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

  if (roles.length == 0) {
    return null;
  }

  return (
    <>
      <ErrorAlert messages={globalErrorMessages} />
      <Form form={form} name="basic" onFinish={onFinish}>
        <Space.Compact>
          <Form.Item
            noStyle
            label="Role"
            name="role"
            rules={[
              {
                required: true,
              },
            ]}
            initialValue={roles[0]}
          >
            <Select
              options={roles.map((role) => ({
                label: humanize(role, "all-upper"),
                value: role,
              }))}
            />
          </Form.Item>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Add
          </Button>
        </Space.Compact>
      </Form>
    </>
  );
}
