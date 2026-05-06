import { useMutation } from "@apollo/client/react";
import { Form, Button, Space } from "antd";
import {
  AddComponentVariantDocument,
  AddComponentVariantMutation,
} from "../../queries/componentVariants.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { ComponentDocument } from "../../queries/components.generated";
import { ComponentIdSelect } from "./ComponentIdSelect";
import ErrorAlert from "../ErrorAlert";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

type FormValues = {
  variantComponentId: Scalars["Uuid"]["input"];
};

interface AddVariantOfComponentProps {
  componentId: Scalars["Uuid"]["input"];
}

export default function AddVariantOfComponent({
  componentId,
}: AddVariantOfComponentProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [addComponentVariantMutation] = useMutation(
    AddComponentVariantDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: ComponentDocument,
          variables: {
            uuid: componentId,
          },
        },
      ],
    },
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<AddComponentVariantMutation>({
      getErrors: (data) => data.addComponentVariant.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        addComponentVariantMutation({
          variables: {
            input: {
              oneComponentId: componentId,
              otherComponentId: values.variantComponentId,
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
        name="addComponentVariant"
        onFinish={onFinish}
        style={{ display: "flex" }}
      >
        <Space.Compact style={{ flex: 1 }}>
          <Form.Item
            noStyle
            label="Variant"
            name="variantComponentId"
            rules={[
              {
                required: true,
              },
            ]}
            style={{ width: "100%" }}
          >
            <ComponentIdSelect />
          </Form.Item>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Add
          </Button>
        </Space.Compact>
      </Form>
    </>
  );
}
