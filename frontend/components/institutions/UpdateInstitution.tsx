import { useMutation } from "@apollo/client/react";
import {
  InstitutionsDocument,
  UpdateInstitutionDocument,
  UpdateInstitutionMutation,
} from "../../queries/institutions.generated";
import { Form, Input, Button, Modal } from "antd";
import { useState } from "react";
import { ContactInformation, Scalars } from "../../__generated__/graphql";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";

type FormValues = {
  name: string;
  abbreviation: string | null | undefined;
  description: string;
  phoneNumber: string | null | undefined;
  postalAddress: string | null | undefined;
  emailAddress: string | null | undefined;
  websiteLocator: string | null | undefined;
};

export type UpdateInstitutionProps = {
  institutionId: Scalars["Uuid"]["input"];
  name: string;
  abbreviation: string | null | undefined;
  description: string;
  contact: ContactInformation | null | undefined;
};

export default function UpdateInstitution({
  institutionId,
  name,
  abbreviation,
  description,
  contact,
}: UpdateInstitutionProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [updateInstitutionMutation] = useMutation(UpdateInstitutionDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: InstitutionsDocument,
      },
    ],
  });

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
              institutionId: institutionId,
              name: values.name,
              abbreviation: values.abbreviation,
              description: values.description,
              contact: {
                phoneNumber: values.phoneNumber,
                postalAddress: values.postalAddress,
                emailAddress: values.emailAddress,
                websiteLocator: values.websiteLocator,
              },
            },
          },
        }),
      {
        onSuccess: () => {
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
      <Button onClick={() => setOpen(true)}>Edit</Button>
      <Modal
        open={open}
        title="Edit Institution"
        // onOk={handleOk}
        onCancel={() => setOpen(false)}
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
            initialValue={name}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Abbreviation"
            name="abbreviation"
            initialValue={abbreviation}
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
            initialValue={description}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Phone Number"
            name="phoneNumber"
            initialValue={contact?.phoneNumber}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Postal Address"
            name="postalAddress"
            initialValue={contact?.postalAddress}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="E-Mail Address"
            name="emailAddress"
            rules={[
              {
                type: "email",
              },
            ]}
            initialValue={contact?.emailAddress}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Website"
            name="websiteLocator"
            rules={[
              {
                type: "url",
              },
            ]}
            initialValue={contact?.websiteLocator}
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
