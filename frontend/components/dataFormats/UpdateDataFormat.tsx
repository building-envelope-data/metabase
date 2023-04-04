import * as React from "react";
import { Alert, Form, Input, Button, Divider, Modal } from "antd";
import {
  useUpdateDataFormatMutation,
  DataFormatsDocument,
} from "../../queries/dataFormats.graphql";
import {
  UpdatePublicationInput,
  UpdateStandardInput,
  Scalars,
  Reference,
} from "../../__generated__/__types__";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { InstitutionDocument } from "../../queries/institutions.graphql";
import { ReferenceForm } from "../ReferenceForm";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

type FormValues = {
  newName: string;
  newExtension: string | null | undefined;
  newDescription: string;
  newMediaType: string;
  newSchemaLocator: Scalars["Url"] | null | undefined;
  standard: UpdateStandardInput | null | undefined;
  publication: UpdatePublicationInput | null | undefined;
};

export type UpdateDataFormatProps = {
  dataFormatId: Scalars["Uuid"];
  name: string;
  extension: string | null | undefined;
  description: string;
  mediaType: string;
  schemaLocator: Scalars["Url"] | null | undefined;
  reference: Reference | null | undefined;
  managerId: Scalars["Uuid"];
};

export default function UpdateDataFormat({
  dataFormatId,
  name,
  extension,
  description,
  mediaType,
  schemaLocator,
  reference,
  managerId,
}: UpdateDataFormatProps) {
  const [open, setOpen] = useState(false);
  const [updateDataFormatMutation] = useUpdateDataFormatMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: InstitutionDocument,
        variables: {
          uuid: managerId,
        },
      },
      {
        query: DataFormatsDocument,
      },
    ],
  });
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm<FormValues>();
  const [updating, setUpdating] = useState(false);

  const onFinish = ({
    newName,
    newExtension,
    newDescription,
    newMediaType,
    newSchemaLocator,
    standard: newStandard,
    publication: newPublication,
  }: FormValues) => {
    const update = async () => {
      try {
        setUpdating(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await updateDataFormatMutation({
          variables: {
            dataFormatId: dataFormatId,
            name: newName,
            extension: newExtension,
            description: newDescription,
            mediaType: newMediaType,
            schemaLocator: newSchemaLocator,
            standard: newStandard,
            publication: newPublication,
          },
        });
        handleFormErrors(
          errors,
          data?.updateDataFormat?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!errors && !data?.updateDataFormat?.errors) {
          form.resetFields();
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
        title="Edit Data Format"
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
          name="updateDataFormat"
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
            label="Extension"
            name="newExtension"
            rules={[
              {
                required: false,
              },
            ]}
            initialValue={extension}
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
            label="Media Type"
            name="newMediaType"
            rules={[
              {
                required: true,
              },
            ]}
            initialValue={mediaType}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Schema Locator"
            name="newSchemaLocator"
            rules={[
              {
                required: false,
              },
              {
                type: "url",
              },
            ]}
            initialValue={schemaLocator}
          >
            <Input />
          </Form.Item>
          <Divider />
          <ReferenceForm form={form} />
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
