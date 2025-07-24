import { useEffect, useState } from "react";
import { useUpdateApplicationMutation, useApplicationQuery, ApplicationPartialFragment, ApplicationDocument } from "../../../queries/openIdConnectApplications.graphql";
import { Alert, Button, Form, Input, message, Result, Select, Skeleton } from "antd";
import { messageApolloError } from "../../../lib/apollo";
import { handleFormErrors } from "../../../lib/form";
import { OpenIdConnectConsentType, OpenIdConnectScope, Scalars } from "../../../__generated__/__types__";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export type UpdateApplicationProps = {
    applicationId: Scalars["Uuid"];
};

type FormValues = {
  newClientId: string;
  newDisplayName: string;
  newConsentType: OpenIdConnectConsentType
  newRedirectUri: string | null | undefined;
  newPostLogoutRedirectUri: string | null | undefined;
  newScopes: OpenIdConnectScope[];
};

export default function UpdateApplication({ applicationId }: UpdateApplicationProps) {
  const { loading, error, data } = useApplicationQuery({
    variables: {
      uuid: applicationId,
    },
  });
  const application = data?.openIdConnectApplication as ApplicationPartialFragment;
  const [form] = Form.useForm<FormValues>();
  const [updating, setUpdating] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(new Array<string>());

  const [updateApplicationMutation] = useUpdateApplicationMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: ApplicationDocument,
        variables: {
          uuid: applicationId,
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
            applicationId: applicationId,
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
          message.success('Successfully updated application ' + data.updateOpenIdConnectApplication.application?.clientId)
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

  useEffect(() => {
    if (error) {
      messageApolloError(error);
    }
  }, [error]);

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!application) {
    return (
      <Result
        status="500"
        title="500"
        subTitle="Sorry, something went wrong."
      />
    );
  }

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
    </>
  );
}