import { useMutation } from "@apollo/client/react";
import { Form, Button } from "antd";
import {
  AddComponentVariantDocument,
  AddComponentVariantMutation,
} from "../../queries/componentVariants.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { ComponentDocument } from "../../queries/components.generated";
import { SelectComponentId } from "../SelectComponentId";
import ErrorAlert from "../ErrorAlert";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

type FormValues = {
  variantComponentId: Scalars["Uuid"]["input"];
};

interface AddVariantOfComponentProps {
  componentId: Scalars["Uuid"]["input"];
};

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
        name="addComponentVariant"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Variant"
          name="variantComponentId"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <SelectComponentId />
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
