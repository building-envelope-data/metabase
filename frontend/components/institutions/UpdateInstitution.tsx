import { useMutation } from "@apollo/client/react";
import {
  InstitutionPartialFragment,
  UpdateInstitutionDocument,
  UpdateInstitutionMutation,
} from "../../queries/institutions.generated";
import { Form, Input, Button, Modal } from "antd";
import { useState } from "react";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";
import EditButton from "../EditButton";
import { Scalars } from "../../__generated__/graphql";
import { phoneNumberFormInput } from "../ContactInformation";

type ContactFormValues = {
  phoneNumber: Scalars["PhoneNumber"]["input"] | null | undefined;
  postalAddress: string | null | undefined;
  emailAddress: Scalars["EmailAddress"]["input"] | null | undefined;
  websiteLocator: string | null | undefined;
};

type FormValues = {
  name: string;
  abbreviation: string | null | undefined;
  description: string;
  contact: ContactFormValues | null | undefined;
};

interface UpdateInstitutionProps {
  institution: InstitutionPartialFragment;
}

export default function UpdateInstitution({
  institution,
}: UpdateInstitutionProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [updateInstitutionMutation] = useMutation(UpdateInstitutionDocument);

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<UpdateInstitutionMutation>({
      getErrors: (data) => data.updateInstitution.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        updateInstitutionMutation({
          variables: {
            input: {
              institutionId: institution.uuid,
              name: values.name,
              abbreviation: values.abbreviation,
              description: values.description,
              contact: {
                phoneNumber: values.contact?.phoneNumber,
                postalAddress: values.contact?.postalAddress,
                emailAddress: values.contact?.emailAddress,
                websiteLocator: values.contact?.websiteLocator,
              },
            },
          },
        }),
      {
        onSuccess: () => {
          setGlobalErrorMessages([]);
          setOpen(false);
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
      <EditButton onClick={() => setOpen(true)} />
      <Modal
        open={open}
        title="Edit Institution"
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
              {
                whitespace: true,
              },
            ]}
            initialValue={institution.name}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Abbreviation"
            name="abbreviation"
            initialValue={institution.abbreviation}
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
              {
                whitespace: true,
              },
            ]}
            initialValue={institution.description}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Phone Number"
            name={["contact", "phoneNumber"]}
            extra={phoneNumberFormInput.extra}
            initialValue={institution.contact?.phoneNumber}
          >
            <Input placeholder={phoneNumberFormInput.placeholder} />
          </Form.Item>
          <Form.Item
            label="Postal Address"
            name={["contact", "postalAddress"]}
            initialValue={institution.contact?.postalAddress}
          >
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
            initialValue={institution.contact?.emailAddress}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Website"
            name={["contact", "websiteLocator"]}
            rules={[
              {
                type: "url",
              },
            ]}
            initialValue={institution.contact?.websiteLocator}
          >
            <Input />
          </Form.Item>
          <Form.Item {...tailLayout}>
            <Button type="primary" htmlType="submit" loading={mutating}>
              Update
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
