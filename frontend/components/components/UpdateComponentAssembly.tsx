import { useMutation } from "@apollo/client/react";
import { Form, Button, InputNumber, Input, Modal, Space } from "antd";
import {
  UpdateComponentAssemblyDocument,
  UpdateComponentAssemblyMutation,
} from "../../queries/componentAssemblies.generated";
import { PrimeSurface, Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import ErrorAlert from "../ErrorAlert";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import EditButton from "../EditButton";
import EnumSelect from "../EnumSelect";

type FormValues = {
  index: Scalars["UnsignedByte"]["input"] | null | undefined;
  primeSurface: PrimeSurface | null | undefined;
};

interface UpdateComponentAssemblyProps {
  assembledComponent: { uuid: Scalars["Uuid"]["input"]; name: string };
  partComponent: { uuid: Scalars["Uuid"]["input"]; name: string };
  index: Scalars["UnsignedByte"]["input"] | null | undefined;
  primeSurface: PrimeSurface | null | undefined;
}

export default function UpdateComponentAssembly(
  componentAssembly: UpdateComponentAssemblyProps,
) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [updateComponentAssemblyMutation] = useMutation(
    UpdateComponentAssemblyDocument,
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<UpdateComponentAssemblyMutation>({
      getErrors: (data) => data.updateComponentAssembly.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        updateComponentAssemblyMutation({
          variables: {
            input: {
              assembledComponentId: componentAssembly.assembledComponent.uuid,
              partComponentId: componentAssembly.partComponent.uuid,
              index: values.index,
              primeSurface: values.primeSurface,
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
      <EditButton type="icon" onClick={() => setOpen(true)} />
      <Modal
        open={open}
        title="Edit Assembly"
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
          name="updateComponentAssembly"
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
        >
          <Form.Item label="Assembled Component">
            <Input
              disabled={true}
              value={componentAssembly.assembledComponent.name}
            />
          </Form.Item>
          <Form.Item label="Part Component">
            <Input
              disabled={true}
              value={componentAssembly.partComponent.name}
            />
          </Form.Item>
          <Form.Item
            initialValue={componentAssembly.index}
            label="Index"
            name="index"
          >
            <InputNumber min={1} max={255} />
          </Form.Item>
          <Form.Item
            initialValue={componentAssembly.primeSurface}
            label="Prime Surface"
            name="primeSurface"
          >
            <EnumSelect
              enumObject={PrimeSurface}
              allowClear={true}
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item {...tailLayout}>
            <Space>
              <Button type="primary" htmlType="submit" loading={mutating}>
                Update
              </Button>
              <Button onClick={() => setOpen(false)} disabled={mutating}>
                Cancel
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
