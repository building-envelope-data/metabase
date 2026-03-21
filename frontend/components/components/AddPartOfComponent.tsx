import { useMutation } from "@apollo/client/react";
import { Form, Button, InputNumber, Select } from "antd";
import {
  AddComponentAssemblyDocument,
  AddComponentAssemblyMutation,
} from "../../queries/componentAssemblies.generated";
import { PrimeSurface, Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { ComponentDocument } from "../../queries/components.generated";
import { SelectComponentId } from "../SelectComponentId";
import ErrorAlert from "../ErrorAlert";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

type FormValues = {
  partComponentId: Scalars["Uuid"]["input"];
  index: Scalars["Byte"]["input"] | null | undefined;
  primeSurface: PrimeSurface | null | undefined;
};

export type AddPartOfComponentProps = {
  assembledComponentId: Scalars["Uuid"]["input"];
};

export default function AddPartOfComponent({
  assembledComponentId,
}: AddPartOfComponentProps) {
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
            uuid: assembledComponentId,
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
              assembledComponentId: assembledComponentId,
              partComponentId: values.partComponentId,
              index: values.index,
              primeSurface: values.primeSurface,
            },
          },
        }),
      {
        onSuccess: () => {
          form.resetFields();
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
      <ErrorAlert messages={globalErrorMessages} />
      <Form
        {...layout}
        form={form}
        name="addPartComponent"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Part"
          name="partComponentId"
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
          <Button type="primary" htmlType="submit" loading={mutating}>
            Add
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
