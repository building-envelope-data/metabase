import { useMutation } from "@apollo/client/react";
import {
  InstitutionsDocument,
  CreateInstitutionDocument,
  CreateInstitutionMutation,
} from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";
import { Form, Input, Button, Modal, App } from "antd";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";
import { useState } from "react";
import { layout, tailLayout } from "../../lib/form";
import NewButton from "../NewButton";
import InstitutionSummary from "./InstitutionSummary";
import RepresentedInstitutionIdSelect from "./RepresentedInstitutionIdSelect";
import { notEmpty } from "../../lib/array";
import UserIdSelect from "../users/UserIdSelect";
import { createPaginatedIdSelectOption } from "../PaginatedIdSelect";

type ContactFormValues = {
  phoneNumber: string | null | undefined;
  postalAddress: string | null | undefined;
  emailAddress: string | null | undefined;
  websiteLocator: string | null | undefined;
};

type FormValues = {
  name: string;
  abbreviation: string | null | undefined;
  description: string;
  contact: ContactFormValues | null | undefined;
  ownerId:
    | { value: Scalars["Uuid"]["input"]; label: string }
    | null
    | undefined;
  managerId:
    | { value: Scalars["Uuid"]["input"]; label: string }
    | null
    | undefined;
};

type CreateInstitutionProps =
  | { initialOwner: { uuid: Scalars["Uuid"]["input"]; name: string } }
  | { initialManager: { uuid: Scalars["Uuid"]["input"]; name: string } };

export default function CreateInstitution(props: CreateInstitutionProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState<string[]>([]);
  const [form] = Form.useForm<FormValues>();
  const { notification } = App.useApp();

  const [createInstitutionMutation] = useMutation(CreateInstitutionDocument, {
    refetchQueries: [InstitutionsDocument],
  });

  const {
    mutating,
    withMutationHandler,
    messageMissingModel,
    augmentFormWithErrors,
  } = useMutationHandler<CreateInstitutionMutation>({
    getErrors: (data) => data.createInstitution.errors,
  });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        createInstitutionMutation({
          variables: {
            input: {
              name: values.name,
              abbreviation: values.abbreviation,
              description: values.description,
              contact: {
                phoneNumber: values.contact?.phoneNumber,
                postalAddress: values.contact?.postalAddress,
                emailAddress: values.contact?.emailAddress,
                websiteLocator: values.contact?.websiteLocator,
              },
              ownerIds: [values.ownerId?.value].filter(notEmpty),
              managerId: values.managerId?.value,
            },
          },
        }),
      {
        onSuccess: (data) => {
          const model = data?.createInstitution?.institution;
          if (!model) {
            messageMissingModel();
          } else {
            setGlobalErrorMessages([]);
            form.resetFields();
            setOpen(false);
            notification.success({
              title: "Created Institution",
              placement: "top",
              showProgress: true,
              pauseOnHover: true,
              duration: 0,
              style: {
                width: "max-content",
                minWidth: "384px",
              },
              description: (
                <InstitutionSummary hideInputControls entity={model} />
              ),
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
      <NewButton onClick={() => setOpen(true)}>Institution</NewButton>
      <Modal
        open={open}
        title="New Institution"
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
          name="basic"
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
          <Form.Item label="Abbreviation" name="abbreviation">
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
          <Form.Item label="Phone Number" name={["contact", "phoneNumber"]}>
            <Input />
          </Form.Item>
          <Form.Item label="Postal Address" name={["contact", "postalAddress"]}>
            <Input />
          </Form.Item>
          <Form.Item
            label="E-Mail Address"
            name={["contact", "emailAddress"]}
            rules={[
              {
                type: "email",
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Website Locator"
            name={["contact", "websiteLocator"]}
            rules={[
              {
                type: "url",
              },
            ]}
          >
            <Input />
          </Form.Item>
          {"initialManager" in props && (
            <Form.Item
              label="Manager"
              name="managerId"
              rules={[{ required: true }]}
              initialValue={createPaginatedIdSelectOption(props.initialManager)}
            >
              <RepresentedInstitutionIdSelect labelInValue />
            </Form.Item>
          )}
          {"initialOwner" in props && (
            <Form.Item
              label="Owner"
              name="ownerId"
              rules={[{ required: true }]}
              initialValue={createPaginatedIdSelectOption(props.initialOwner)}
            >
              <UserIdSelect labelInValue />
            </Form.Item>
          )}
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
