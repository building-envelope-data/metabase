import { useMutation } from "@apollo/client/react";
import { Form, Button, InputNumber, Input, Select, Modal, Space } from "antd";
import {
  UpdateComponentAssemblyDocument,
  UpdateComponentAssemblyMutation,
} from "../../queries/componentAssemblies.generated";
import { PrimeSurface, Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { ComponentDocument } from "../../queries/components.generated";
import ErrorAlert from "../ErrorAlert";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

type FormValues = {
  index: Scalars["Byte"]["input"] | null | undefined;
  primeSurface: PrimeSurface | null | undefined;
};

export type UpdateComponentAssemblyProps = {
  assembledComponent: { uuid: Scalars["Uuid"]["input"]; name: string };
  partComponent: { uuid: Scalars["Uuid"]["input"]; name: string };
  index: Scalars["Byte"]["input"] | null | undefined;
  primeSurface: PrimeSurface | null | undefined;
};

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
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: ComponentDocument,
          variables: {
            uuid: componentAssembly.assembledComponent.uuid,
          },
        },
        {
          query: ComponentDocument,
          variables: {
            uuid: componentAssembly.partComponent.uuid,
          },
        },
      ],
    },
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
        onSuccess: () => setOpen(false),
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
        title="Edit Assembly"
        // onOk={handleOk}
        onCancel={() => setOpen(false)}
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
