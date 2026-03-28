import { Scalars } from "../../../__generated__/graphql";
import { Descriptions, Divider, Result, Skeleton, Typography } from "antd";
import UpdateOpenIdConnectApplication from "./UpdateOpenIdConnectApplication";
import OpenIdConnectAutorizationTable from "../authorizations/OpenIdConnectAuthorizationTable";
import OpenIdConnectTokenTable from "../tokens/OpenIdConnectTokenTable";
import DeleteOpenIdConnectApplication from "./DeleteOpenIdConnectApplication";
import PageHeader from "../../PageHeader";
import { ApplicationDocument } from "../../../queries/openIdConnect.generated";
import ResetOpenIdConnectApplicationClientSecret from "./ResetOpenIdConnectApplicationClientSecret";
import { useQuery } from "@apollo/client/react";
import paths from "../../../paths";
import { useQueryHandler } from "../../../lib/hooks/useQueryHandler";
import { isTruthy } from "../../../lib/array";

interface Props {
  applicationId: Scalars["Uuid"]["input"];
}

export default function OpenIdConnectApplication({ applicationId }: Props) {
  const { loading, error, data } = useQuery(ApplicationDocument, {
    variables: {
      uuid: applicationId,
    },
  });
  useQueryHandler({ error });
  const application = data?.openIdConnectApplication;

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
      <PageHeader
        id={application.uuid}
        title={application.displayName ?? application.clientId}
        tags={[]}
        extra={[
          application.isAuthorizedToManageNode && (
            <UpdateOpenIdConnectApplication
              key="updateApplication"
              application={application}
            />
          ),
          application.isAuthorizedToManageNode && (
            <ResetOpenIdConnectApplicationClientSecret
              key="resetApplicationClientSecret"
              applicationId={application.uuid}
            />
          ),
          application.isAuthorizedToManageNode && (
            <DeleteOpenIdConnectApplication
              key="deleteApplication"
              applicationId={application.uuid}
              redirectTo={paths.institution(application.owner.node.uuid)}
            />
          ),
        ].filter(isTruthy)}
      >
        <Descriptions size="small" column={1}>
          <Descriptions.Item label="UUID">{application.uuid}</Descriptions.Item>
          <Descriptions.Item label="Client ID">
            {application.clientId}
          </Descriptions.Item>
          <Descriptions.Item label="Consent Type">
            {application.consentType}
          </Descriptions.Item>
          <Descriptions.Item label="Endpoints">
            {application.endpoints.join(", ")}
          </Descriptions.Item>
          <Descriptions.Item label="Grant Types">
            {application.grantTypes.join(", ")}
          </Descriptions.Item>
          <Descriptions.Item label="Response Types">
            {application.responseTypes.join(", ")}
          </Descriptions.Item>
          <Descriptions.Item label="Scopes">
            {application.scopes.join(", ")}
          </Descriptions.Item>
          <Descriptions.Item label="Requirements">
            {application.requirements.join(", ")}
          </Descriptions.Item>
          {application.postLogoutRedirectUri && (
            <Descriptions.Item label="Post Logout Redirect URI">
              <Typography.Link href={application.postLogoutRedirectUri}>
                {application.postLogoutRedirectUri}
              </Typography.Link>
            </Descriptions.Item>
          )}
          {application.redirectUri && (
            <Descriptions.Item label="Redirect URI">
              <Typography.Link href={application.redirectUri}>
                {application.redirectUri}
              </Typography.Link>
            </Descriptions.Item>
          )}
        </Descriptions>
      </PageHeader>
      <Divider />
      <Typography.Title level={2}>Authorizations</Typography.Title>
      <OpenIdConnectAutorizationTable
        applicationId={application.uuid}
        authorizations={application.authorizations.edges.map((x) => x.node)}
      />
      <Divider />
      <Typography.Title level={2}>Tokens</Typography.Title>
      <OpenIdConnectTokenTable
        tokens={application.tokens.edges.map((x) => x.node)}
      />
    </>
  );
}
