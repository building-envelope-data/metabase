import { useMutation } from "@apollo/client/react";
import { Select, Form, Button } from "antd";
import {
  AddInstitutionRepresentativeDocument,
  AddInstitutionRepresentativeMutation,
} from "../../queries/institutionRepresentatives.generated";
import { InstitutionRepresentativeRole } from "../../__generated__/graphql";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { SelectUserId } from "../SelectUserId";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";

type FormValues = {
  userId: Scalars["Uuid"]["input"];
  role: InstitutionRepresentativeRole;
};

interface AddInstitutionRepresentativeProps {
  institutionId: Scalars["Uuid"]["input"];
};

export default function AddInstitutionRepresentative({
  institutionId,
}: AddInstitutionRepresentativeProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [addInstitutionRepresentativeMutation] = useMutation(
    AddInstitutionRepresentativeDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: InstitutionDocument,
          variables: {
            uuid: institutionId,
          },
        },
      ],
    },
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<AddInstitutionRepresentativeMutation>({
      getErrors: (data) => data.addInstitutionRepresentative.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        addInstitutionRepresentativeMutation({
          variables: {
            input: {
              institutionId: institutionId,
              userId: values.userId,
              role: values.role,
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
        name="addInstitutionRepresentative"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="User"
          name="userId"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <SelectUserId />
        </Form.Item>
        <Form.Item
          label="Role"
          name="role"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Select
            placeholder="Please select"
            options={Object.entries(InstitutionRepresentativeRole).map(
              ([_key, value]) => ({ label: value, value: value }),
            )}
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
