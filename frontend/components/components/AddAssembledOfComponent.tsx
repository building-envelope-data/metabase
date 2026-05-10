import { useMutation } from "@apollo/client/react";
import { Form, Button, InputNumber, Space } from "antd";
import {
  AddComponentAssemblyDocument,
  AddComponentAssemblyMutation,
} from "../../queries/componentAssemblies.generated";
import { PrimeSurface, Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ComponentIdSelect from "./ComponentIdSelect";
import ErrorAlert from "../ErrorAlert";
import EnumSelect from "../EnumSelect";

type FormValues = {
  assembledComponentId: Scalars["Uuid"]["input"];
  index: Scalars["Byte"]["input"] | null | undefined;
  primeSurface: PrimeSurface | null | undefined;
};

interface AddAssembledOfComponentProps {
  partComponentId: Scalars["Uuid"]["input"];
}

export default function AddAssembledOfComponent({
  partComponentId,
}: AddAssembledOfComponentProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [addComponentAssemblyMutation] = useMutation(
    AddComponentAssemblyDocument,
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<AddComponentAssemblyMutation>({
      getErrors: (data) => data.addComponentAssembly.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        addComponentAssemblyMutation({
          variables: {
            input: {
              partComponentId: partComponentId,
              assembledComponentId: values.assembledComponentId,
              index: values.index,
              primeSurface: values.primeSurface,
            },
          },
        }),
      {
        onSuccess: () => {
          setGlobalErrorMessages([]);
          form.resetFields();
        },
        onError: (graphQlErrors, userErrors) =>
          setGlobalErrorMessages(
            augmentFormWithErrors(graphQlErrors, userErrors, form),
          ),
      },
    );
  };

  return (
    <>
      <ErrorAlert messages={globalErrorMessages} />
      <Form
        form={form}
        name="addAssembledComponent"
        onFinish={onFinish}
        style={{ display: "flex" }}
      >
        <Space.Compact style={{ flex: 1 }}>
          <Form.Item
            noStyle
            label="Assembly"
            name="assembledComponentId"
            rules={[
              {
                required: true,
              },
            ]}
            style={{ width: "100%" }}
          >
            <ComponentIdSelect />
          </Form.Item>
          <Form.Item noStyle label="Index" name="index">
            <InputNumber placeholder="Index" min={1} max={255} />
          </Form.Item>
          <Form.Item noStyle label="Prime Surface" name="primeSurface">
            <EnumSelect
              enumObject={PrimeSurface}
              allowClear={true}
              placeholder="Prime Surface"
            />
          </Form.Item>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Add
          </Button>
        </Space.Compact>
      </Form>
    </>
  );
}
