import { useMutation } from "@apollo/client/react";
import { Form, Button, InputNumber, Select, Space } from "antd";
import {
  AddComponentAssemblyDocument,
  AddComponentAssemblyMutation,
} from "../../queries/componentAssemblies.generated";
import { PrimeSurface, Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import { ComponentDocument } from "../../queries/components.generated";
import { ComponentIdSelect } from "./ComponentIdSelect";
import ErrorAlert from "../ErrorAlert";

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
    {
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
    },
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
            <Select
              allowClear={true}
              placeholder="Prime Surface"
              options={Object.entries(PrimeSurface).map(([_key, value]) => ({
                label: value,
                value: value,
              }))}
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
