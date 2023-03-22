import * as React from "react";
import { Alert, Form, Input, Button, Modal } from "antd";
import {
  useUpdateDatabaseMutation,
  DatabasesDocument,
  DatabaseDocument,
} from "../../queries/databases.graphql";
import { Scalars } from "../../__generated__/__types__";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export type UpdateDatabaseProps = {
  databaseId: Scalars["Uuid"];
  name: string;
  description: string;
  locator: Scalars["Url"];
};

export default function UpdateDatabase({
  databaseId,
  name,
  description,
  locator,
}: UpdateDatabaseProps) {
  const [open, setOpen] = useState(false);
  const [updateDatabaseMutation] = useUpdateDatabaseMutation({
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
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [updating, setUpdating] = useState(false);

  const onFinish = ({
    newName,
    newDescription,
    newLocator,
  }: {
    newName: string;
    newDescription: string;
    newLocator: Scalars["Url"];
  }) => {
    const update = async () => {
      try {
        setUpdating(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await updateDatabaseMutation({
          variables: {
            databaseId: databaseId,
            name: newName,
            description: newDescription,
            locator: newLocator,
          },
        });
        handleFormErrors(
          errors,
          data?.updateDatabase?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!errors && !data?.updateDatabase?.errors)
          data?.updateDatabase?.database;
        {
          setOpen(false);
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setUpdating(false);
      }
    };
    update();
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
        {globalErrorMessages.length > 0 ? (
          <Alert type="error" message={globalErrorMessages.join(" ")} />
        ) : (
          <></>
        )}
        <Form
          {...layout}
          form={form}
          name="updateDatabase"
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
        >
          <Form.Item
            label="Name"
            name="newName"
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
            name="newDescription"
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
            name="newLocator"
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
            <Button type="primary" htmlType="submit" loading={updating}>
              Update
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
