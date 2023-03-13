import {
  InstitutionsDocument,
  useUpdateInstitutionMutation,
} from "../../queries/institutions.graphql";
import { Alert, Form, Input, Button, Modal } from "antd";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { Scalars } from "../../__generated__/__types__";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export type UpdateInstitutionProps = {
  institutionId: Scalars["Uuid"];
  name: string;
  abbreviation: string | null | undefined;
  description: string;
  websiteLocator: string;
};

export default function UpdateInstitution({
  institutionId,
  name,
  abbreviation,
  description,
  websiteLocator,
}: UpdateInstitutionProps) {
  const [open, setOpen] = useState(false);
  const [updateInstitutionMutation] = useUpdateInstitutionMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: InstitutionsDocument,
      },
    ],
  });
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [updating, setUpdating] = useState(false);

  const onFinish = ({
    newName,
    newAbbreviation,
    newDescription,
    newWebsiteLocator,
  }: {
    newName: string;
    newAbbreviation: string | null | undefined;
    newDescription: string;
    newWebsiteLocator: string;
  }) => {
    const update = async () => {
      try {
        setUpdating(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await updateInstitutionMutation({
          variables: {
            institutionId: institutionId,
            name: newName,
            abbreviation: newAbbreviation,
            description: newDescription,
            websiteLocator: newWebsiteLocator,
          },
        });
        handleFormErrors(
          errors,
          data?.updateInstitution?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (
          !errors &&
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
        title="Edit Assembly"
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
            label="Website"
            name="newWebsiteLocator"
            rules={[
              {
                type: "url",
              },
            ]}
            initialValue={websiteLocator}
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
