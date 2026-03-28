import { useMutation } from "@apollo/client/react";
import { Form, Button } from "antd";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { MethodDocument } from "../../queries/methods.generated";
import { SelectInstitutionId } from "../SelectInstitutionId";
import {
  AddInstitutionMethodDeveloperDocument,
  AddInstitutionMethodDeveloperMutation,
} from "../../queries/institutionMethodDevelopers.generated";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";

type FormValues = { institutionId: Scalars["Uuid"]["input"] };

interface AddInstitutionMethodDeveloperProps {
  methodId: Scalars["Uuid"]["input"];
}

export default function AddInstitutionMethodDeveloper({
  methodId,
}: AddInstitutionMethodDeveloperProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [addInstitutionMethodDeveloperMutation] = useMutation(
    AddInstitutionMethodDeveloperDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: MethodDocument,
          variables: {
            uuid: methodId,
          },
        },
      ],
    },
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<AddInstitutionMethodDeveloperMutation>({
      getErrors: (data) => data.addInstitutionMethodDeveloper.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        addInstitutionMethodDeveloperMutation({
          variables: {
            input: {
              methodId: methodId,
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

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <>
      <ErrorAlert messages={globalErrorMessages} />
      <Form
        {...layout}
        form={form}
        name="addInstitutionMethodDeveloper"
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
