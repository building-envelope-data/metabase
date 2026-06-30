import { useMutation } from "@apollo/client/react";
import { Form, Button, Space } from "antd";
import {
  AddComponentGeneralizationDocument,
  AddComponentGeneralizationMutation,
} from "../../queries/componentGeneralizations.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import ComponentIdSelect from "./ComponentIdSelect";
import ErrorAlert from "../ErrorAlert";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

type FormValues = {
  generalComponentId: Scalars["Uuid"]["input"];
};

interface AddGeneralizationOfComponentProps {
  concreteComponentId: Scalars["Uuid"]["input"];
}

export default function AddGeneralizationOfComponent({
  concreteComponentId,
}: AddGeneralizationOfComponentProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [addComponentGeneralizationMutation] = useMutation(
    AddComponentGeneralizationDocument,
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<AddComponentGeneralizationMutation>({
      getErrors: (data) => data.addComponentGeneralization.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        addComponentGeneralizationMutation({
          variables: {
            input: {
              generalComponentId: values.generalComponentId,
              concreteComponentId: concreteComponentId,
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
        name="addGeneralComponent"
        onFinish={onFinish}
        style={{ display: "flex" }}
      >
        <Space.Compact style={{ flex: 1 }}>
          <Form.Item
            noStyle
            label="Generalization"
            name="generalComponentId"
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
