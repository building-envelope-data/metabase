import { useMutation } from "@apollo/client/react";
import { useState } from "react";
import {
  ApplicationsDocument,
  CreateApplicationDocument,
  CreateApplicationMutation,
} from "../../../queries/openIdConnect.generated";
import { Button, Form, Input, App, Typography, Modal, Divider } from "antd";
import {
  OpenIdConnectConsentType,
  OpenIdConnectEndpoint,
  OpenIdConnectGrantType,
  OpenIdConnectResponseType,
  OpenIdConnectScope,
  OpenIdConnectRequirement,
  Scalars,
} from "../../../__generated__/graphql";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";
import { layout, tailLayout } from "../../../lib/form";
import ErrorAlert from "../../ErrorAlert";
import NewButton from "../../NewButton";
import OpenIdConnectApplicationSummary from "./OpenIdConnectApplicationSummary";
import EntityLink from "../../entities/EntityLink";
import paths from "../../../paths";
import CodeView from "../../CodeView";
import RepresentedInstitutionIdSelect from "../../institutions/RepresentedInstitutionIdSelect";
import EnumSelect, { allEnumSelectOptions } from "../../EnumSelect";

type FormValues = {
  clientId: string;
  displayName: string;
  consentType: OpenIdConnectConsentType;
  redirectUri: Scalars["Url"]["input"] | null | undefined;
  postLogoutRedirectUri: Scalars["Url"]["input"] | null | undefined;
  endpoints: OpenIdConnectEndpoint[];
  grantTypes: OpenIdConnectGrantType[];
  responseTypes: OpenIdConnectResponseType[];
  scopes: OpenIdConnectScope[];
  ownerId: Scalars["Uuid"]["input"];
};

interface CreateApplicationProps {
  initialOwnerId: Scalars["Uuid"]["input"];
}

export default function CreateOpenIdConnectApplication({
  initialOwnerId,
}: CreateApplicationProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [open, setOpen] = useState(false);
  const [form] = Form.useForm<FormValues>();
  const { notification } = App.useApp();

  const [createApplicationMutation] = useMutation(CreateApplicationDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [ApplicationsDocument],
  });

  const {
    mutating,
    withMutationHandler,
    messageMissingModel,
    augmentFormWithErrors,
  } = useMutationHandler<CreateApplicationMutation>({
    getErrors: (data) => data.createOpenIdConnectApplication.errors,
  });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        createApplicationMutation({
          variables: {
            input: {
              institutionId: values.ownerId,
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
        onSuccess: (data) => {
          const model = data?.createOpenIdConnectApplication?.application;
          if (!model || !data?.createOpenIdConnectApplication.clientSecret) {
            messageMissingModel();
          } else {
            setGlobalErrorMessages([]);
            form.resetFields();
            setOpen(false);
            notification.success({
              title: "Created OpenId-Connect Application",
              placement: "top",
              showProgress: true,
              pauseOnHover: true,
              duration: 0,
              style: {
                width: "max-content",
                minWidth: "384px",
              },
              description: (
                <div>
                  <Typography.Paragraph style={{ maxWidth: "75ch" }}>
                    Please copy and save the following client secret now, you
                    will not be able to access it later
                    <CodeView
                      code={data.createOpenIdConnectApplication.clientSecret}
                    />
                    Should you forget it, you may reset it on{" "}
                    <EntityLink
                      entity={model}
                      route={paths.openIdConnectApplication}
                    />
                    .
                  </Typography.Paragraph>
                  <Divider />
                  <OpenIdConnectApplicationSummary hideExtra entity={model} />
                </div>
              ),
            });
          }
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
      <NewButton onClick={() => setOpen(true)}>
        OpenId-Connect Application
      </NewButton>
      <Modal
        open={open}
        title="New OpenId-Connect Application"
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
            initialValue={OpenIdConnectConsentType.Explicit}
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
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Logout Redirect URL"
            name="postLogoutRedirectUri"
            rules={[{ type: "url" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Endpoints"
            name="endpoints"
            rules={[{ required: true }]}
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
          >
            <EnumSelect
              enumObject={OpenIdConnectResponseType}
              mode="multiple"
              allowClear
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item label="Scopes" name="scopes" rules={[{ required: true }]}>
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
          <Form.Item
            label="Owner"
            name="ownerId"
            rules={[{ required: true }]}
            initialValue={initialOwnerId}
          >
            <RepresentedInstitutionIdSelect />
          </Form.Item>
          <Form.Item {...tailLayout}>
            <Button type="primary" htmlType="submit" loading={mutating}>
              Create
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
