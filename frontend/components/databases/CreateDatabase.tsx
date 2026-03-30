import { useMutation } from "@apollo/client/react";
import { Form, Input, Button, App, Typography } from "antd";
import {
  CreateDatabaseDocument,
  CreateDatabaseMutation,
  DatabasesDocument,
} from "../../queries/databases.generated";
import {
  DatabaseVerificationState,
  Scalars,
} from "../../__generated__/graphql";
import { useState } from "react";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";
import { ExclamationCircleTwoTone } from "@ant-design/icons";
import Link from "next/link";
import paths from "../../paths";

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
  const { modal } = App.useApp();

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

  const {
    mutating,
    withMutationHandler,
    augmentFormWithErrors,
    messageMissingModel,
  } = useMutationHandler<CreateDatabaseMutation>({
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
        onSuccess: (data) => {
          setGlobalErrorMessages([]);
          form.resetFields();
          const model = data?.createDatabase.database;
          if (!model) {
            messageMissingModel();
          } else {
            if (model.verificationState == DatabaseVerificationState.Pending) {
              modal.info({
                title: "Database Verification Code",
                centered: true,
                width: 500,
                content: (
                  <Typography.Paragraph style={{ maxWidth: "75ch" }}>
                    <span>
                      <ExclamationCircleTwoTone twoToneColor="#f9b02e" />{" "}
                    </span>
                    Have your database&apos;s GraphQL endpoint return the
                    verification code &ldquo;{model.verificationCode}&rdquo;
                    (without the quotation marks), when queried for the GraphQL
                    query &ldquo;verificationCode&rdquo;. Then, press the
                    &ldquo;Verify&rdquo; button on
                    <Link href={paths.database(model.uuid)}>{model.name}</Link>
                    to make the metabase assert that the verification codes
                    match which proves that you control the GraphQL endpoint
                    {model.locator}. Verified databases are publicly listed and
                    included in data searches. When you are logged-in and the
                    database is unverified, the verification code is shown on
                    <Link href={paths.database(model.uuid)}>{model.name}</Link>.
                  </Typography.Paragraph>
                ),
              });
            }
          }
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
