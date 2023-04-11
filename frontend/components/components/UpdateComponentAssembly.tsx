import * as React from "react";
import {
  Alert,
  Form,
  Button,
  InputNumber,
  Input,
  Select,
  Modal,
  Space,
} from "antd";
import { useUpdateComponentAssemblyMutation } from "../../queries/componentAssemblies.graphql";
import { PrimeSurface, Scalars } from "../../__generated__/__types__";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { ComponentDocument } from "../../queries/components.graphql";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

type FormValues = {
  newIndex: Scalars["Byte"] | null | undefined;
  newPrimeSurface: PrimeSurface | null | undefined;
};

export type UpdateComponentAssemblyProps = {
  assembledComponent: { uuid: Scalars["Uuid"]; name: string };
  partComponent: { uuid: Scalars["Uuid"]; name: string };
  index: Scalars["Byte"] | null | undefined;
  primeSurface: PrimeSurface | null | undefined;
};

export default function UpdateComponentAssembly({
  assembledComponent,
  partComponent,
  index,
  primeSurface,
}: UpdateComponentAssemblyProps) {
  const [open, setOpen] = useState(false);
  const [updateComponentAssemblyMutation] = useUpdateComponentAssemblyMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: ComponentDocument,
        variables: {
          uuid: assembledComponent.uuid,
        },
      },
      {
        query: ComponentDocument,
        variables: {
          uuid: partComponent.uuid,
        },
      },
    ],
  });
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm<FormValues>();
  const [updating, setUpdating] = useState(false);

  const onFinish = ({ newIndex, newPrimeSurface }: FormValues) => {
    const update = async () => {
      try {
        setUpdating(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await updateComponentAssemblyMutation({
          variables: {
            assembledComponentId: assembledComponent.uuid,
            partComponentId: partComponent.uuid,
            index: newIndex,
            primeSurface: newPrimeSurface,
          },
        });
        handleFormErrors(
          errors,
          data?.updateComponentAssembly?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!errors && !data?.updateComponentAssembly?.errors) {
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
        title="Edit Assembly"
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
          name="updateComponentAssembly"
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
        >
          <Form.Item label="Assembled Component">
            <Input disabled={true} value={assembledComponent.name} />
          </Form.Item>
          <Form.Item label="Part Component">
            <Input disabled={true} value={partComponent.name} />
          </Form.Item>
          <Form.Item initialValue={index} label="Index" name="newIndex">
            <InputNumber min={1} max={255} />
          </Form.Item>
          <Form.Item
            initialValue={primeSurface}
            label="Prime Surface"
            name="newPrimeSurface"
          >
            <Select
              allowClear={true}
              placeholder="Please select"
              options={Object.entries(PrimeSurface).map(([_key, value]) => ({
                label: value,
                value: value,
              }))}
            />
          </Form.Item>
          <Form.Item {...tailLayout}>
            <Space>
              <Button type="primary" htmlType="submit" loading={updating}>
                Update
              </Button>
              <Button onClick={() => setOpen(false)} disabled={updating}>
                Cancel
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
