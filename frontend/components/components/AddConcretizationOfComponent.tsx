import { useMutation } from "@apollo/client/react";
import { Form, Button } from "antd";
import {
  AddComponentGeneralizationDocument,
  AddComponentGeneralizationMutation,
} from "../../queries/componentGeneralizations.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { ComponentDocument } from "../../queries/components.generated";
import { SelectComponentId } from "../SelectComponentId";
import ErrorAlert from "../ErrorAlert";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

type FormValues = {
  concreteComponentId: Scalars["Uuid"]["input"];
};

interface AddConcretizationOfComponentProps {
  generalComponentId: Scalars["Uuid"]["input"];
}

export default function AddConcretizationOfComponent({
  generalComponentId,
}: AddConcretizationOfComponentProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [addComponentGeneralizationMutation] = useMutation(
    AddComponentGeneralizationDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: ComponentDocument,
          variables: {
            uuid: generalComponentId,
          },
        },
      ],
    },
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
              generalComponentId: generalComponentId,
              concreteComponentId: values.concreteComponentId,
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

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <>
      <ErrorAlert messages={globalErrorMessages} />
      <Form
        {...layout}
        form={form}
        name="addConcreteComponent"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Concretization"
          name="concreteComponentId"
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
