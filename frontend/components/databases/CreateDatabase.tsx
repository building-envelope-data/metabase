import { useMutation } from "@apollo/client/react";
import { Form, Input, Button } from "antd";
import {
  CreateDatabaseDocument,
  CreateDatabaseMutation,
  DatabasesDocument,
} from "../../queries/databases.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";

type FormValues = {
  name: string;
  description: string;
  locator: Scalars["Url"]["input"];
};

interface CreateDatabaseProps {
  operatorId: Scalars["Uuid"]["input"];
}

export default function CreateDatabase({ operatorId }: CreateDatabaseProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [createDatabaseMutation] = useMutation(CreateDatabaseDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: InstitutionDocument,
        variables: {
          uuid: operatorId,
        },
      },
      {
        query: DatabasesDocument,
      },
    ],
  });

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<CreateDatabaseMutation>({
      getErrors: (data) => data.createDatabase.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        createDatabaseMutation({
          variables: {
            input: {
              name: values.name,
              description: values.description,
              locator: values.locator,
              operatorId: operatorId,
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
        name="createDatabase"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Name"
          name="name"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Description"
          name="description"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Locator"
          name="locator"
          rules={[
            {
              required: true,
            },
            {
              type: "url",
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Create
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
