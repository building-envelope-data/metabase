import { useMutation } from "@apollo/client/react";
import { Form, Button } from "antd";
import {
  AddComponentManufacturerDocument,
  AddComponentManufacturerMutation,
} from "../../queries/componentManufacturers.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { ComponentDocument } from "../../queries/components.generated";
import { SelectInstitutionId } from "../SelectInstitutionId";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";
import { layout, tailLayout } from "../../lib/form";

type FormValues = { institutionId: Scalars["Uuid"]["input"] };

interface AddComponentManufacturerProps {
  componentId: Scalars["Uuid"]["input"];
};

export default function AddComponentManufacturer({
  componentId,
}: AddComponentManufacturerProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [addComponentManufacturerMutation] = useMutation(
    AddComponentManufacturerDocument,
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
        name="addComponentManufacturer"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Institution"
          name="institutionId"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <SelectInstitutionId />
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
