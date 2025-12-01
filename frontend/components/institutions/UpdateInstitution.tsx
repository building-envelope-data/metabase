import { useMutation } from "@apollo/client/react";
import {
  InstitutionsDocument,
  UpdateInstitutionDocument,
} from "../../queries/institutions.generated";
import { Alert, Form, Input, Button, Modal } from "antd";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { ContactInformation, Scalars } from "../../__generated__/graphql";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

type FormValues = {
  newName: string;
  newAbbreviation: string | null | undefined;
  newDescription: string;
  newPhoneNumber: string | null | undefined;
  newPostalAddress: string | null | undefined;
  newEmailAddress: string | null | undefined;
  newWebsiteLocator: string | null | undefined;
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
  const [updateInstitutionMutation] = useMutation(UpdateInstitutionDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: InstitutionsDocument,
      },
    ],
  });
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();
  const [updating, setUpdating] = useState(false);

  const onFinish = ({
    newName,
    newAbbreviation,
    newDescription,
    newPhoneNumber,
    newPostalAddress,
    newEmailAddress,
    newWebsiteLocator,
  }: FormValues) => {
    const update = async () => {
      try {
        setUpdating(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { error, data } = await updateInstitutionMutation({
          variables: {
            input: {
              institutionId: institutionId,
              name: newName,
              abbreviation: newAbbreviation,
              description: newDescription,
              contact: {
                phoneNumber: newPhoneNumber,
                postalAddress: newPostalAddress,
                emailAddress: newEmailAddress,
                websiteLocator: newWebsiteLocator,
              },
            },
          },
        });
        handleFormErrors(
          error,
          data?.updateInstitution?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form,
        );
        if (
          !error &&
          !data?.updateInstitution?.errors &&
          data?.updateInstitution?.institution
        ) {
          setOpen(false);
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setUpdating(false);
      }
    };
    update();
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
        {/* TODO Display error messages in a list? */}
        {globalErrorMessages.length > 0 ? (
          <Alert type="error" message={globalErrorMessages.join(" ")} />
        ) : (
          <></>
        )}
        <Form
          {...layout}
          form={form}
          name="basic"
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
        >
          <Form.Item
            label="Name"
            name="newName"
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
            name="newAbbreviation"
            initialValue={abbreviation}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Description"
            name="newDescription"
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
            name="newWebsiteLocator"
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
            <Button type="primary" htmlType="submit" loading={updating}>
              Update
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
