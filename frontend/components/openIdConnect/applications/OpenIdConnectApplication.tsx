import { Scalars } from "../../../__generated__/graphql";
import { Card, Divider, Result, Skeleton, Typography } from "antd";
import OpenIdConnectAutorizationTable from "../authorizations/OpenIdConnectAuthorizationTable";
import OpenIdConnectTokenTable from "../tokens/OpenIdConnectTokenTable";
import { ApplicationDocument } from "../../../queries/openIdConnect.generated";
import { useQuery } from "@apollo/client/react";
import { useQueryHandler } from "../../../lib/hooks/useQueryHandler";
import OpenIdConnectApplicationSummary from "./OpenIdConnectApplicationSummary";
import QueryToolbar from "../../QueryToolbar";

interface Props {
  applicationId: Scalars["Uuid"]["input"];
}

export default function OpenIdConnectApplication({ applicationId }: Props) {
  const queryVariables = {
    uuid: applicationId,
  };
  const { loading, error, data } = useQuery(ApplicationDocument, {
    variables: queryVariables,
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
    <Card>
      <OpenIdConnectApplicationSummary entity={application} />
      <Divider />
      <Typography.Title level={4}>Authorizations</Typography.Title>
      <OpenIdConnectAutorizationTable
        applicationId={application.uuid}
        authorizations={application.authorizations.edges.map((x) => x.node)}
      />
      <Divider />
      <Typography.Title level={4}>Tokens</Typography.Title>
      <OpenIdConnectTokenTable
        tokens={application.tokens.edges.map((x) => x.node)}
      />
      <Divider />
      <QueryToolbar query={ApplicationDocument} variables={queryVariables} />
    </Card>
  );
}
