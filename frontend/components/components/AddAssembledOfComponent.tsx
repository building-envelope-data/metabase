import * as React from "react";
import { Alert, Form, Button, InputNumber, Select } from "antd";
import { useAddComponentAssemblyMutation } from "../../queries/componentAssemblies.graphql";
import { PrimeSurface, Scalars } from "../../__generated__/__types__";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { ComponentDocument } from "../../queries/components.graphql";
import { SelectComponentId } from "../SelectComponentId";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export type AddAssembledOfComponentProps = {
  partComponentId: Scalars["Uuid"];
};

export default function AddAssembledOfComponent({
  partComponentId,
}: AddAssembledOfComponentProps) {
  const [addComponentAssemblyMutation] = useAddComponentAssemblyMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: ComponentDocument,
        variables: {
          uuid: partComponentId,
        },
      },
    ],
  });
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [adding, setAdding] = useState(false);

  const onFinish = ({
    assembledComponentId,
    index,
    primeSurface,
  }: {
    assembledComponentId: Scalars["Uuid"];
    index: Scalars["Byte"] | null | undefined;
    primeSurface: PrimeSurface | null | undefined;
  }) => {
    const add = async () => {
      try {
        setAdding(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await addComponentAssemblyMutation({
          variables: {
            partComponentId: partComponentId,
            assembledComponentId: assembledComponentId,
            index: index,
            primeSurface: primeSurface,
          },
        });
        handleFormErrors(
          errors,
          data?.addComponentAssembly?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!errors && !data?.addComponentAssembly?.errors) {
          form.resetFields();
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setAdding(false);
      }
    };
    add();
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
        name="addComponentAssembly"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Assembly"
          name="assembledComponentId"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <SelectComponentId />
        </Form.Item>
        <Form.Item label="Index" name="index">
          <InputNumber min={1} max={255} />
        </Form.Item>
        <Form.Item label="Prime Surface" name="primeSurface">
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
          <Button type="primary" htmlType="submit" loading={adding}>
            Add
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
