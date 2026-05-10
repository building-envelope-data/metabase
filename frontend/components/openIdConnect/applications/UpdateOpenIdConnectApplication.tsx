import { useMutation } from "@apollo/client/react";
import { useState } from "react";
import {
  UpdateApplicationDocument,
  UpdateApplicationMutation,
  OpenIdConnectApplicationPartialFragment,
} from "../../../queries/openIdConnect.generated";
import { Button, Form, Input, Modal } from "antd";
import {
  OpenIdConnectConsentType,
  OpenIdConnectEndpoint,
  OpenIdConnectGrantType,
  OpenIdConnectResponseType,
  OpenIdConnectScope,
  OpenIdConnectRequirement,
} from "../../../__generated__/graphql";
import { layout, tailLayout } from "../../../lib/form";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";
import ErrorAlert from "../../ErrorAlert";
import EditButton from "../../EditButton";
import EnumSelect, { allEnumSelectOptions } from "../../EnumSelect";

interface UpdateApplicationProps {
  application: OpenIdConnectApplicationPartialFragment;
}

type FormValues = {
  clientId: string;
  displayName: string;
  consentType: OpenIdConnectConsentType;
  redirectUri: string | null | undefined;
  postLogoutRedirectUri: string | null | undefined;
  endpoints: OpenIdConnectEndpoint[];
  grantTypes: OpenIdConnectGrantType[];
  responseTypes: OpenIdConnectResponseType[];
  scopes: OpenIdConnectScope[];
};

export default function UpdateOpenIdConnectApplication({
  application,
}: UpdateApplicationProps) {
  const [open, setOpen] = useState(false);
  const [form] = Form.useForm<FormValues>();
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );

  const [updateApplicationMutation] = useMutation(UpdateApplicationDocument);

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<UpdateApplicationMutation>({
      getErrors: (data) => data.updateOpenIdConnectApplication.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        updateApplicationMutation({
          variables: {
            input: {
              applicationId: application.uuid,
              clientId: values.clientId,
              displayName: values.displayName,
              consentType: values.consentType,
              redirectUri: values.redirectUri,
              postLogoutRedirectUri: values.postLogoutRedirectUri,
              endpoints: values.endpoints || [],
              grantTypes: values.grantTypes || [],
              responseTypes: values.responseTypes || [],
              scopes: values.scopes || [],
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
        title="Edit Application"
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
          name="updateApplication"
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
        >
          <Form.Item
            label="ClientId"
            name="clientId"
            rules={[{ required: true }]}
            initialValue={application.name}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Display Name"
            name="displayName"
            rules={[{ required: true }]}
            initialValue={application.displayName}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Consent Type"
            name="consentType"
            rules={[{ required: true }]}
            initialValue={application.consentType}
          >
            <EnumSelect
              enumObject={OpenIdConnectConsentType}
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item
            label="Login Redirect URL"
            name="redirectUri"
            rules={[{ type: "url" }]}
            initialValue={application.redirectUri}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Logout Redirect URL"
            name="postLogoutRedirectUri"
            rules={[{ type: "url" }]}
            initialValue={application.postLogoutRedirectUri}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Endpoints"
            name="endpoints"
            rules={[{ required: true }]}
            initialValue={application.endpoints}
          >
            <EnumSelect
              enumObject={OpenIdConnectEndpoint}
              mode="multiple"
              allowClear
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item
            label="GrantTypes"
            name="grantTypes"
            rules={[{ required: true }]}
            initialValue={application.grantTypes}
          >
            <EnumSelect
              enumObject={OpenIdConnectGrantType}
              mode="multiple"
              allowClear
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item
            label="ResponseTypes"
            name="responseTypes"
            rules={[{ required: true }]}
            initialValue={application.responseTypes}
          >
            <EnumSelect
              enumObject={OpenIdConnectResponseType}
              mode="multiple"
              allowClear
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item
            label="Scopes"
            name="scopes"
            rules={[{ required: true }]}
            initialValue={application.scopes}
          >
            <EnumSelect
              enumObject={OpenIdConnectScope}
              mode="multiple"
              allowClear
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item
            label="Requirements"
            name="requirements"
            rules={[{ required: true }]}
            initialValue={allEnumSelectOptions(OpenIdConnectRequirement)}
          >
            <EnumSelect
              enumObject={OpenIdConnectRequirement}
              disabled
              mode="multiple"
              allowClear
              placeholder="Please select"
            />
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
