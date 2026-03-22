import { ReactNode } from "react";
import { Scalars } from "../../../__generated__/graphql";
import { Descriptions, Divider, Result, Skeleton, Typography } from "antd";
import UpdateApplication from "./UpdateApplication";
import AutorizationTable from "../authorizations/AuthorizationTable";
import TokenTable from "../tokens/TokenTable";
import { PageHeader } from "@ant-design/pro-layout";
import DeleteApplication from "./DeleteApplication";
import { ApplicationDocument } from "../../../queries/openIdConnect.generated";
import ResetApplicationClientSecret from "./ResetApplicationClientSecret";
import { useQuery } from "@apollo/client/react";
import paths from "../../../paths";
import { useQueryHandler } from "../../../lib/hooks/useQueryHandler";

export type ApplicationProps = {
  applicationId: Scalars["Uuid"]["input"];
};

export default function Application({ applicationId }: ApplicationProps) {
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
        title={application.displayName}
        tags={[]}
        extra={([] as ReactNode[])
          .concat(
            application.isAuthorizedToManageNode
              ? [
                  <UpdateApplication
                    key="updateApplication"
                    application={application}
                  />,
                ]
              : [],
          )
          .concat(
            application.isAuthorizedToManageNode
              ? [
                  <ResetApplicationClientSecret
                    key="resetApplicationClientSecret"
                    applicationId={application.uuid}
                  />,
                ]
              : [],
          )
          .concat(
            application.isAuthorizedToManageNode
              ? [
                  <DeleteApplication
                    key="deleteApplication"
                    applicationId={application.uuid}
                    redirectTo={paths.institution(application.owner.node.uuid)}
                  />,
                ]
              : [],
          )}
        backIcon={false}
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
      <AutorizationTable
        applicationId={application.uuid}
        authorizations={application.authorizations.edges.map((x) => x.node)}
      />
      <Divider />
      <Typography.Title level={2}>Tokens</Typography.Title>
      <TokenTable
        applicationId={application.uuid}
        tokens={application.tokens.edges.map((x) => x.node)}
      />
    </>
  );
}
