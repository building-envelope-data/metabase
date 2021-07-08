import * as React from "react";
import { Alert, Form, Input, Button } from "antd";
import {
  useCreateDataFormatMutation,
  Scalars,
  DataFormatsDocument,
} from "../../queries/dataFormats.graphql";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { InstitutionDocument } from "../../queries/institutions.graphql";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export type CreateDataFormatProps = {
  managerId: Scalars["Uuid"];
};

export const CreateDataFormat: React.FunctionComponent<CreateDataFormatProps> =
  ({ managerId }) => {
    const [createDataFormatMutation] = useCreateDataFormatMutation({
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
    const [form] = Form.useForm();
    const [creating, setCreating] = useState(false);

    const onFinish = ({
      name,
      extension,
      description,
      mediaType,
      schemaLocator,
    }: {
      name: string;
      extension: string | null;
      description: string;
      mediaType: string;
      schemaLocator: Scalars["Url"] | null;
      // standard: CreateStandardInput;
      // publication: CreatePublicationInput;
    }) => {
      const create = async () => {
        try {
          setCreating(true);
          // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
          const { errors, data } = await createDataFormatMutation({
            variables: {
              name: name,
              extension: extension,
              description: description,
              mediaType: mediaType,
              schemaLocator: schemaLocator,
              // standard: standard,
              // publication: publication,
              managerId: managerId,
            },
          });
          handleFormErrors(
            errors,
            data?.createDataFormat?.errors?.map((x) => {
              return { code: x.code, message: x.message, path: x.path };
            }),
            setGlobalErrorMessages,
            form
          );
        } catch (error) {
          // TODO Handle properly.
          console.log("Failed:", error);
        } finally {
          setCreating(false);
        }
      };
      create();
    };

    const onFinishFailed = () => {
      setGlobalErrorMessages(["Fix the errors below."]);
    };

    return (
      <>
        {globalErrorMessages.length > 0 ? (
          <Alert type="error" message={globalErrorMessages.join(" ")} />
        ) : (
          <></>
        )}
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
          {/* TODO $standard: CreateStandardInput
            $publication: CreatePublicationInput */}
          <Form.Item {...tailLayout}>
            <Button type="primary" htmlType="submit" loading={creating}>
              Create
            </Button>
          </Form.Item>
        </Form>
      </>
    );
  };

export default CreateDataFormat;
