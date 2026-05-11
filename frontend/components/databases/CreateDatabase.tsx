import { useMutation } from "@apollo/client/react";
import { Form, Input, Button, App, Modal } from "antd";
import {
  AnyDatabasesDocument,
  CreateDatabaseDocument,
  CreateDatabaseMutation,
  DatabasesDocument,
} from "../../queries/databases.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";
import NewButton from "../NewButton";
import DatabaseSummary from "./DatabaseSummary";
import RepresentedInstitutionIdSelect from "../institutions/RepresentedInstitutionIdSelect";

type FormValues = {
  name: string;
  description: string;
  locator: Scalars["Url"]["input"];
  operatorId: Scalars["Uuid"]["input"];
};

interface CreateDatabaseProps {
  initialOperatorId: Scalars["Uuid"]["input"];
}

export default function CreateDatabase({
  initialOperatorId,
}: CreateDatabaseProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();
  const { notification } = App.useApp();

  const [createDatabaseMutation] = useMutation(CreateDatabaseDocument, {
    refetchQueries: [DatabasesDocument, AnyDatabasesDocument],
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
              operatorId: values.operatorId,
            },
          },
        }),
      {
        onSuccess: (data) => {
          const model = data?.createDatabase.database;
          if (!model) {
            messageMissingModel();
          } else {
            setGlobalErrorMessages([]);
            form.resetFields();
            setOpen(false);
            notification.success({
              title: "Created Database",
              placement: "top",
              showProgress: true,
              pauseOnHover: true,
              duration: 0,
              style: {
                width: "max-content",
                minWidth: "384px",
              },
              description: <DatabaseSummary hideInputControls entity={model} />,
            });
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
      <NewButton onClick={() => setOpen(true)}>Database</NewButton>
      <Modal
        open={open}
        title="New Database"
        // onOk={handleOk}
        onCancel={() => {
          setGlobalErrorMessages([]);
          form.resetFields();
          setOpen(false);
        }}
        footer={false}
      >
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
          <Form.Item
            label="Operator"
            name="operatorId"
            rules={[{ required: true }]}
            initialValue={initialOperatorId}
          >
            <RepresentedInstitutionIdSelect />
          </Form.Item>
          <Form.Item {...tailLayout}>
            <Button type="primary" htmlType="submit" loading={mutating}>
              Create
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
