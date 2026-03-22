import { useMutation } from "@apollo/client/react";
import {
  UserDocument,
  UsersDocument,
  AddUserRoleDocument,
  AddUserRoleMutation,
} from "../../queries/users.generated";
import { Scalars, UserRole } from "../../__generated__/graphql";
import { Form, Button, Select } from "antd";
import { useState } from "react";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";

type FormValues = { role: UserRole };

export type AddUserRoleProps = {
  userId: Scalars["Uuid"]["input"];
  roles: UserRole[];
};

export default function AddUserRole({ userId, roles }: AddUserRoleProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [addUserRoleMutation] = useMutation(AddUserRoleDocument, {
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
    <>
      <ErrorAlert messages={globalErrorMessages} />
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
          <Button type="primary" htmlType="submit" loading={mutating}>
            Add
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
