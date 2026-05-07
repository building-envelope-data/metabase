import { useMutation } from "@apollo/client/react";
import { Form, Input, Button, Modal } from "antd";
import {
  UpdateDatabaseDocument,
  UpdateDatabaseMutation,
  DatabasePartialFragment,
} from "../../queries/databases.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";
import EditButton from "../EditButton";

type FormValues = {
  name: string;
  description: string;
  locator: Scalars["Url"]["input"];
};

interface UpdateDatabaseProps {
  database: DatabasePartialFragment;
}

export default function UpdateDatabase({ database }: UpdateDatabaseProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [updateDatabaseMutation] = useMutation(UpdateDatabaseDocument);

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<UpdateDatabaseMutation>({
      getErrors: (data) => data.updateDatabase.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        updateDatabaseMutation({
          variables: {
            input: {
              databaseId: database.uuid,
              name: values.name,
              description: values.description,
              locator: values.locator,
            },
          },
        }),
      {
        onSuccess: () => {
          setGlobalErrorMessages([]);
          setOpen(false);
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
      <EditButton onClick={() => setOpen(true)} />
      <Modal
        open={open}
        title="Edit Database"
        // onOk={handleOk}
        onCancel={() => {
          setGlobalErrorMessages([]);
          form.resetFields();
          setOpen(false);
        }}
        footer={false}
      >
        <ErrorAlert messages={globalErrorMessages} />
        <Form
          {...layout}
          form={form}
          name="updateDatabase"
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
            initialValue={database.name}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Description"
            name="description"
            rules={[
              {
                required: true,
              },
            ]}
            initialValue={database.description}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Locator"
            name="locator"
            rules={[
              {
                required: true,
              },
              {
                type: "url",
              },
            ]}
            initialValue={database.locator}
          >
            <Input />
          </Form.Item>
          <Form.Item {...tailLayout}>
            <Button type="primary" htmlType="submit" loading={mutating}>
              Update
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
