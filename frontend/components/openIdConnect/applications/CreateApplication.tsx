import { useEffect, useState } from "react";
import { ApplicationsDocument, useCreateApplicationMutation } from "../../../queries/openIdConnectApplications.graphql";
import { Alert, Button, Flex, Form, Input, message, Modal, Select, Typography } from "antd";
import { useCurrentUserQuery } from "../../../queries/currentUser.graphql";
import { useRouter } from "next/router";
import { handleFormErrors } from "../../../lib/form";
import paths, { redirectToLoginPage } from "../../../paths";
import { ExclamationCircleTwoTone } from '@ant-design/icons';
import { OpenIdConnectConsentType, OpenIdConnectScope, Scalars } from "../../../__generated__/__types__";

type FormValues = {
    associatedInstitutionId: string;
    clientId: string;
    displayName: string;
    consentType: OpenIdConnectConsentType;
    redirectUri: Scalars["Url"] | null | undefined;
    postLogoutRedirectUri: Scalars["Url"] | null | undefined;
    scopes: OpenIdConnectScope[];
};

export default function CreateApplication() {
    const currentUser = useCurrentUserQuery()?.data?.currentUser;
    const router = useRouter();
    const shouldRedirect = !(currentUser);
    const [form] = Form.useForm<FormValues>();
    const [updating, setUpdating] = useState(false);
    const [globalErrorMessages, setGlobalErrorMessages] = useState(new Array<string>());

    const [createApplicationMutation] = useCreateApplicationMutation({
        // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
        // See https://www.apollographql.com/docs/react/data/mutations/#options
        refetchQueries: [
            {
                query: ApplicationsDocument,
            },
        ],
    });

    const onFinish = ({
        associatedInstitutionId,
        clientId,
        displayName,
        consentType,
        redirectUri,
        postLogoutRedirectUri,
        scopes,
    }: FormValues) => {
        const update = async () => {
            try {
                setUpdating(true);
                // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout

                const { errors, data } = await createApplicationMutation({
                    variables: {
                        associatedInstitutionId: associatedInstitutionId,
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
                if (data) {
                    message.success('Successfully created application ' + data.createOpenIdConnectApplication.application?.displayName);
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
                        onOk: () => {
                            router.push(paths.openIdConnect)
                        },
                      });
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
        if (router.isReady && shouldRedirect) {
            redirectToLoginPage(router, paths.openIdConnectApplicationCreate);
        }
    }, [shouldRedirect, router]);

    return (
        <>
            {globalErrorMessages.length > 0 ? (
                <Alert className="error-message" type="error" message={globalErrorMessages.join(" ")} />
            ) : (
                <></>
            )}
            <Form
                labelAlign="left"
                labelCol={{ flex: "150px" }}
                wrapperCol={{ flex: "auto" }}
                form={form}
                name="basic"
                onFinish={onFinish}
                onFinishFailed={onFinishFailed}
            >
                <Form.Item
                    label="Associated Institution"
                    name="associatedInstitutionId"
                    rules={[{ required: true }]}
                >
                    <Select
                        allowClear
                        style={{ width: '100%' }}
                        placeholder="Please select"
                    >
                        {currentUser?.representedInstitutions?.edges.map((node => {
                            return <Select.Option key={node.node.id!} value={node.node.uuid}>
                                {node.node.name}
                            </Select.Option>
                        }))
                        }
                    </Select>
                </Form.Item>
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
                <Form.Item>
                    <Flex gap="small" justify="right">
                        <Button type="primary"
                            htmlType="button"
                            loading={updating}
                            href={paths.openIdConnect}>
                            Cancel
                        </Button>
                        <Button type="primary"
                            htmlType="submit"
                            loading={updating}
                            style={{ marginLeft: "5px" }}>
                            Create
                        </Button>
                    </Flex>
                </Form.Item>
            </Form>
        </>
    );
}