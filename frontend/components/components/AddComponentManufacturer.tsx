import { useMutation } from "@apollo/client/react";
import { Form, Button, Space } from "antd";
import {
  AddComponentManufacturerDocument,
  AddComponentManufacturerMutation,
} from "../../queries/componentManufacturers.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import InstitutionIdSelect from "../institutions/InstitutionIdSelect";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";

type FormValues = { institutionId: Scalars["Uuid"]["input"] };

interface AddComponentManufacturerProps {
  componentId: Scalars["Uuid"]["input"];
}

export default function AddComponentManufacturer({
  componentId,
}: AddComponentManufacturerProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [addComponentManufacturerMutation] = useMutation(
    AddComponentManufacturerDocument,
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<AddComponentManufacturerMutation>({
      getErrors: (data) => data.addComponentManufacturer.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        addComponentManufacturerMutation({
          variables: {
            input: {
              componentId: componentId,
              institutionId: values.institutionId,
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
        name="addComponentManufacturer"
        onFinish={onFinish}
        style={{ display: "flex" }}
      >
        <Space.Compact style={{ flex: 1 }}>
          <Form.Item
            noStyle
            label="Institution"
            name="institutionId"
            rules={[
              {
                required: true,
              },
            ]}
            style={{ width: "100%" }}
          >
            <InstitutionIdSelect />
          </Form.Item>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Add
          </Button>
        </Space.Compact>
      </Form>
    </>
  );
}
