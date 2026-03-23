import { useMutation } from "@apollo/client/react";
import { Form, Input, Button, Divider, Modal } from "antd";
import {
  UpdateDataFormatDocument,
  UpdateDataFormatMutation,
  DataFormatPartialFragment,
} from "../../queries/dataFormats.generated";
import { ReferenceInput, Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { ReferenceForm } from "../ReferenceForm";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";
import { layout, tailLayout } from "../../lib/form";

type FormValues = {
  name: string;
  extension: string | null | undefined;
  description: string;
  mediaType: string;
  schemaLocator: Scalars["Url"]["input"] | null | undefined;
  reference: ReferenceInput | null | undefined;
};

export type UpdateDataFormatProps = {
  dataFormat: Pick<
    DataFormatPartialFragment,
    | "uuid"
    | "name"
    | "extension"
    | "description"
    | "mediaType"
    | "schemaLocator"
    | "reference"
    | "manager"
  >;
};

export default function UpdateDataFormat({
  dataFormat,
}: UpdateDataFormatProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();
  const [open, setOpen] = useState(false);

  const [updateDataFormatMutation] = useMutation(UpdateDataFormatDocument);

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<UpdateDataFormatMutation>({
      getErrors: (data) => data.updateDataFormat.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () => {
        // TODO Why does `initialValue` not set standardizers to `[]`?
        if (
          values.reference?.standard != null &&
          values.reference?.standard.standardizers == undefined
        ) {
          values.reference.standard.standardizers = [];
        }
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        return updateDataFormatMutation({
          variables: {
            input: {
              dataFormatId: dataFormat.uuid,
              name: values.name,
              extension: values.extension,
              description: values.description,
              mediaType: values.mediaType,
              schemaLocator: values.schemaLocator,
              reference: values.reference,
            },
          },
        });
      },
      {
        onSuccess: async () => {
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
        title="Edit Data Format"
        // onOk={handleOk}
        onCancel={() => setOpen(false)}
        footer={false}
      >
        <ErrorAlert messages={globalErrorMessages} />
        <Form
          {...layout}
          form={form}
          name="updateDataFormat"
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
            initialValue={dataFormat.name}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Extension"
            name="extension"
            rules={[
              {
                required: false,
              },
            ]}
            initialValue={dataFormat.extension}
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
            initialValue={dataFormat.description}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Media Type"
            name="mediaType"
            rules={[
              {
                required: true,
              },
            ]}
            initialValue={dataFormat.mediaType}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Schema Locator"
            name="schemaLocator"
            rules={[
              {
                required: false,
              },
              {
                type: "url",
              },
            ]}
            initialValue={dataFormat.schemaLocator}
          >
            <Input />
          </Form.Item>
          <Divider />
          <ReferenceForm
            form={form}
            namespace={["reference"]}
            initialValue={dataFormat.reference}
          />
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
