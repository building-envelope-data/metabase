import { useMutation } from "@apollo/client/react";
import { Form, Input, Button, Modal } from "antd";
import {
  UpdateDatabaseDocument,
  DatabasesDocument,
  DatabaseDocument,
  UpdateDatabaseMutation,
} from "../../queries/databases.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";

type FormValues = {
  name: string;
  description: string;
  locator: Scalars["Url"]["input"];
};

export type UpdateDatabaseProps = {
  databaseId: Scalars["Uuid"]["input"];
  name: string;
  description: string;
  locator: Scalars["Url"]["input"];
};

export default function UpdateDatabase({
  databaseId,
  name,
  description,
  locator,
}: UpdateDatabaseProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [updateDatabaseMutation] = useMutation(UpdateDatabaseDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: DatabaseDocument,
        variables: {
          uuid: databaseId,
        },
      },
      {
        query: DatabasesDocument,
      },
    ],
  });

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
              databaseId: databaseId,
              name: values.name,
              description: values.description,
              locator: values.locator,
            },
          },
        }),
      {
        onSuccess: () => {
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
      <Button onClick={() => setOpen(true)}>Edit</Button>
      <Modal
        open={open}
        title="Edit Database"
        // onOk={handleOk}
        onCancel={() => setOpen(false)}
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
            initialValue={name}
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
            initialValue={description}
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
            initialValue={locator}
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
