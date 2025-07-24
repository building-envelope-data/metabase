import { useState } from "react";
import { ApplicationsDocument, useCreateApplicationMutation } from "../../../queries/openIdConnectApplications.graphql";
import { Alert, Button, Form, Input, message, Modal, Select, Typography } from "antd";
import { handleFormErrors } from "../../../lib/form";
import { ExclamationCircleTwoTone } from '@ant-design/icons';
import { OpenIdConnectConsentType, OpenIdConnectScope, Scalars } from "../../../__generated__/__types__";
import { InstitutionDocument } from "../../../queries/institutions.graphql";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

type FormValues = {
    clientId: string;
    displayName: string;
    consentType: OpenIdConnectConsentType;
    redirectUri: Scalars["Url"] | null | undefined;
    postLogoutRedirectUri: Scalars["Url"] | null | undefined;
    scopes: OpenIdConnectScope[];
};

export type CreateApplicationProps = {
  institutionId: Scalars["Uuid"];
};

export default function CreateApplication({ institutionId }: CreateApplicationProps) {
    const [createApplicationMutation] = useCreateApplicationMutation({
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: InstitutionDocument,
          variables: {
            uuid: institutionId,
          },
        },
        {
          query: ApplicationsDocument,
        },
      ],
    });
    const [globalErrorMessages, setGlobalErrorMessages] = useState(
      new Array<string>()
    );
    const [form] = Form.useForm<FormValues>();
    const [creating, setCreating] = useState(false);

    const onFinish = ({
        clientId,
        displayName,
        consentType,
        redirectUri,
        postLogoutRedirectUri,
        scopes,
    }: FormValues) => {
        const update = async () => {
            try {
                setCreating(true);
                // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout

                const { errors, data } = await createApplicationMutation({
                    variables: {
                        institutionId: institutionId,
                        clientId: clientId,
                        displayName: displayName,
                        consentType: consentType,
                        redirectUri: redirectUri,
                        postLogoutRedirectUri: postLogoutRedirectUri,
                        scopes: scopes || [],
                    },
                });
                handleFormErrors(
                    errors,
                    data?.createOpenIdConnectApplication?.errors?.map((x) => {
                        return { code: x.code, message: x.message, path: x.path };
                    }),
                    setGlobalErrorMessages,
                    form
                );
                if (
                  !errors &&
                  !data?.createOpenIdConnectApplication?.errors &&
                  data?.createOpenIdConnectApplication?.application
                ) {
                    Modal.info({
                        title: "Application Client Secret",
                        centered: true,
                        width: 500,
                        content: (
                          <Typography.Paragraph>
                            <span><ExclamationCircleTwoTone twoToneColor="#f9b02e" /> </span>
                            Please copy an save the client secret now, you will not be able to access it later.
                            <p/>
                          <Typography.Paragraph copyable>{data.createOpenIdConnectApplication.clientSecret}</Typography.Paragraph>
                          </Typography.Paragraph>
                        ),
                      });
                }
            } catch (error) {
                // TODO Handle properly.
                message.error("Failed:" + error);
            } finally {
                setCreating(false);
            }
        };
        update();
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
                name="createApplication"
                onFinish={onFinish}
                onFinishFailed={onFinishFailed}
            >
                <Form.Item
                    label="Client Id"
                    name="clientId"
                    rules={[{ required: true }]}
                >
                    <Input />
                </Form.Item>
                <Form.Item
                    label="Display Name"
                    name="displayName"
                    rules={[{ required: true }]}
                >
                    <Input />
                </Form.Item>
                <Form.Item
                    label="Consent Type"
                    name="consentType"
                    rules={[{ required: true }]}
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
                    name="redirectUri"
                    rules={[{ type: 'url' }]}
                >
                    <Input />
                </Form.Item>
                <Form.Item
                    label="Logout Redirect URL"
                    name="postLogoutRedirectUri"
                    rules={[{ type: 'url' }]}
                >
                    <Input />
                </Form.Item>
                <Form.Item
                    label="Scopes"
                    name="scopes"
                    rules={[{ required: true }]}
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
                    <Button type="primary" htmlType="submit" loading={creating}>
                        Create
                    </Button>
                </Form.Item>
            </Form>
        </>
    );
}