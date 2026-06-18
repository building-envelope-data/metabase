import { useMutation, useQuery } from "@apollo/client/react";
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
import EnumSelect, {
  allEnumSelectOptions,
  allEnumValues,
} from "../../EnumSelect";
import { createPaginatedIdSelectOption } from "../../PaginatedIdSelect";
import { CurrentUserDocument } from "../../../queries/currentUser.generated";
import { humanize } from "../../../lib/string";

export const scopesFormItemExtra = `The scope '${humanize(String(OpenIdConnectScope.OpenId), "all-upper")}' is added on submission if it is missing here and the scope '${humanize(String(OpenIdConnectScope.OfflineAccess), "all-upper")}' is added on submission if the grant type '${humanize(String(OpenIdConnectGrantType.RefreshToken))}' is included above.`;

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
  ownerId: { value: Scalars["Uuid"]["input"]; label: string };
};

interface CreateApplicationProps {
  initialOwner: { uuid: Scalars["Uuid"]["input"]; name: string };
}

export default function CreateOpenIdConnectApplication({
  initialOwner,
}: CreateApplicationProps) {
  const currentUser = useQuery(CurrentUserDocument)?.data?.currentUser;

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
              institutionId: values.ownerId.value,
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
              title: "Created OpenID-Connect Application",
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
                  <OpenIdConnectApplicationSummary
                    hideInputControls
                    entity={model}
                  />
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
        OpenID-Connect Application
      </NewButton>
      <Modal
        open={open}
        title="New OpenID-Connect Application"
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
            label="Client ID"
            name="clientId"
            rules={[
              { required: true },
              {
                whitespace: true,
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Display Name"
            name="displayName"
            rules={[
              { required: true },
              {
                whitespace: true,
              },
            ]}
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
              values={currentUser?.authorizedOpenIdConnectConsentTypes ?? []}
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
              values={currentUser?.authorizedOpenIdConnectEndpoints ?? []}
              mode="multiple"
              allowClear
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item
            label="Grant Types"
            name="grantTypes"
            rules={[{ required: true }]}
          >
            <EnumSelect
              values={currentUser?.authorizedOpenIdConnectGrantTypes ?? []}
              mode="multiple"
              allowClear
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item
            label="Response Types"
            name="responseTypes"
            rules={[{ required: true }]}
          >
            <EnumSelect
              values={currentUser?.authorizedOpenIdConnectResponseTypes ?? []}
              mode="multiple"
              allowClear
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item
            label="Scopes"
            name="scopes"
            rules={[{ required: true }]}
            extra={scopesFormItemExtra}
          >
            <EnumSelect
              values={currentUser?.authorizedOpenIdConnectScopes ?? []}
              mode="multiple"
              allowClear
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item
            label="Requirements"
            name="requirements"
            rules={[{ required: true }]}
            initialValue={allEnumSelectOptions(
              allEnumValues(OpenIdConnectRequirement),
            )}
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
            initialValue={createPaginatedIdSelectOption(initialOwner)}
          >
            <RepresentedInstitutionIdSelect labelInValue />
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
