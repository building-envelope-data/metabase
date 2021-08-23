import * as React from "react";
import { Select, Alert, Form, Button } from "antd";
import { useAddInstitutionRepresentativeMutation } from "../../queries/institutions.graphql";
import { InstitutionRepresentativeRole } from "../../__generated__/__types__";
import { Scalars } from "../../__generated__/__types__";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { InstitutionDocument } from "../../queries/institutions.graphql";
import { SelectUserId } from "../SelectUserId";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export type AddInstitutionRepresentativeProps = {
  institutionId: Scalars["Uuid"];
};

export default function AddInstitutionRepresentative({
  institutionId,
}: AddInstitutionRepresentativeProps) {
  const [addInstitutionRepresentativeMutation] =
    useAddInstitutionRepresentativeMutation({
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
    });
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [adding, setAdding] = useState(false);

  const onFinish = ({
    userId,
    role,
  }: {
    userId: Scalars["Uuid"];
    role: InstitutionRepresentativeRole;
  }) => {
    const add = async () => {
      try {
        setAdding(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await addInstitutionRepresentativeMutation({
          variables: {
            institutionId: institutionId,
            userId: userId,
            role: role,
          },
        });
        handleFormErrors(
          errors,
          data?.addInstitutionRepresentative?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!errors && !data?.addInstitutionRepresentative?.errors) {
          form.resetFields();
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setAdding(false);
      }
    };
    add();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <>
      {globalErrorMessages.length > 0 ? (
        <Alert type="error" message={globalErrorMessages.join(" ")} />
      ) : (
        <></>
      )}
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
              ([_key, value]) => ({ label: value, value: value })
            )}
          />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={adding}>
            Add
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
