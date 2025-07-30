import { useState } from "react";
import { useUpdateApplicationMutation, ApplicationPartialFragment, ApplicationDocument } from "../../../queries/openIdConnect.graphql";
import { Alert, Button, Form, Input, message, Modal, Select } from "antd";
import { handleFormErrors } from "../../../lib/form";
import { OpenIdConnectConsentType, OpenIdConnectScope } from "../../../__generated__/__types__";
import { ApplicationsDocument } from "../../../__generated__/queries/openIdConnectApplications.graphql";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export type UpdateApplicationProps = {
  application: ApplicationPartialFragment;
};

type FormValues = {
  newClientId: string;
  newDisplayName: string;
  newConsentType: OpenIdConnectConsentType
  newRedirectUri: string | null | undefined;
  newPostLogoutRedirectUri: string | null | undefined;
  newScopes: OpenIdConnectScope[];
};

export default function UpdateApplication({ application }: UpdateApplicationProps) {
  const [open, setOpen] = useState(false);
  const [form] = Form.useForm<FormValues>();
  const [updating, setUpdating] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(new Array<string>());

  const [updateApplicationMutation] = useUpdateApplicationMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: ApplicationsDocument,
      },
      {
        query: ApplicationDocument,
        variables: {
          uuid: application.uuid,
        },
      },
    ],
  });

  const onFinish = ({
    newClientId,
    newDisplayName,
    newConsentType,
    newRedirectUri,
    newPostLogoutRedirectUri,
    newScopes,
  }: FormValues) => {
    const update = async () => {
      try {
        setUpdating(true);
        const { errors, data } = await updateApplicationMutation({
          variables: {
            applicationId: application.uuid,
            clientId: newClientId,
            displayName: newDisplayName,
            consentType: newConsentType,
            redirectUri: newRedirectUri,
            postLogoutRedirectUri: newPostLogoutRedirectUri,
            scopes: newScopes || [],
          },
        });
        handleFormErrors(
          errors,
          data?.updateOpenIdConnectApplication?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (
          !errors &&
          !data?.updateOpenIdConnectApplication?.errors &&
          data?.updateOpenIdConnectApplication?.application
        ) {
          setOpen(false);
        }
      } catch (error) {
        // TODO Handle properly.
        message.error("Failed:" + error);
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
        title="Edit Application"
        // onOk={handleOk}
        onCancel={() => setOpen(false)}
        footer={false}
      >
        {globalErrorMessages.length > 0 ? (
          <Alert type="error" message={globalErrorMessages.join(" ")} />
        ) : (
          <></>
        )}
        <Form
          {...layout}
          form={form}
          name="updateApplication"
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
        >
          <Form.Item
            label="ClientId"
            name="newClientId"
            rules={[{ required: true }]}
            initialValue={application.clientId}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Display Name"
            name="newDisplayName"
            rules={[{ required: true }]}
            initialValue={application.displayName}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Consent Type"
            name="newConsentType"
            rules={[{ required: true }]}
            initialValue={application.consentType}
          >
            <Select
              placeholder="Please select"
              options={Object.entries(OpenIdConnectConsentType).map(
                ([_key, value]) => ({ label: value, value: value })
              )}
            />
          </Form.Item>
          <Form.Item
            label="Login Redirect URL"
            name="newRedirectUri"
            rules={[{ type: 'url' }]}
            initialValue={application.redirectUri}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Logout Redirect URL"
            name="newPostLogoutRedirectUri"
            rules={[{ type: 'url' }]}
            initialValue={application.postLogoutRedirectUri}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Scopes"
            name="newScopes"
            rules={[{ required: true }]}
            initialValue={application.scopes}
          >
            <Select
              mode="multiple"
              allowClear
              placeholder="Please select"
              options={Object.entries(OpenIdConnectScope).map(
                ([_key, value]) => ({ label: value, value: value })
              )}
            />
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