import { useMutation } from "@apollo/client/react";
import { Form, Input, Button, Divider, App, Modal } from "antd";
import {
  CreateDataFormatDocument,
  CreateDataFormatMutation,
  DataFormatsDocument,
} from "../../queries/dataFormats.generated";
import { ReferenceInput, Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { ReferenceSubform } from "../ReferenceSubform";
import ErrorAlert from "../ErrorAlert";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import DataFormatSummary from "./DataFormatSummary";
import NewButton from "../NewButton";

type FormValues = {
  name: string;
  extension: string | null | undefined;
  description: string;
  mediaType: string;
  schemaLocator: Scalars["Url"]["input"] | null | undefined;
  reference: ReferenceInput | null | undefined;
};

interface CreateDataFormatProps {
  managerId: Scalars["Uuid"]["input"];
}

export default function CreateDataFormat({ managerId }: CreateDataFormatProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();
  const { notification } = App.useApp();

  const [createDataFormatMutation] = useMutation(CreateDataFormatDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: InstitutionDocument,
        variables: {
          uuid: managerId,
        },
      },
      DataFormatsDocument,
    ],
  });

  const {
    mutating,
    withMutationHandler,
    messageMissingModel,
    augmentFormWithErrors,
  } = useMutationHandler<CreateDataFormatMutation>({
    getErrors: (data) => data.createDataFormat.errors,
  });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () => {
        // TODO Why does `initialValue` not set standardizers to `[]`?
        if (
          values.reference?.standard != null &&
          values.reference.standard.standardizers == undefined
        ) {
          values.reference.standard.standardizers = [];
        }
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        return createDataFormatMutation({
          variables: {
            input: {
              name: values.name,
              extension: values.extension,
              description: values.description,
              mediaType: values.mediaType,
              schemaLocator: values.schemaLocator,
              reference: values.reference,
              managerId: managerId,
            },
          },
        });
      },
      {
        onSuccess: (data) => {
          const model = data?.createDataFormat?.dataFormat;
          if (!model) {
            messageMissingModel();
          } else {
            setGlobalErrorMessages([]);
            form.resetFields();
            setOpen(false);
            notification.success({
              title: "Created Data Format",
              placement: "top",
              showProgress: true,
              pauseOnHover: true,
              duration: 0,
              style: {
                width: "max-content",
                minWidth: "384px",
              },
              description: <DataFormatSummary hideExtra entity={model} />,
            });
          }
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
      <NewButton onClick={() => setOpen(true)}>Data Format</NewButton>
      <Modal
        open={open}
        title="New Data Format"
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
          name="createDataFormat"
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
          >
            <Input />
          </Form.Item>
          <Divider />
          <ReferenceSubform form={form} namespace={["reference"]} />
          <Form.Item {...tailLayout}>
            <Button type="primary" htmlType="submit" loading={mutating}>
              Create
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
