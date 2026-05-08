import { Scalars, SortEnumType } from "../../../__generated__/graphql";
import { Card, Divider, Result, Skeleton } from "antd";
import { ApplicationDocument } from "../../../queries/openIdConnect.generated";
import { useQuery } from "@apollo/client/react";
import { useQueryHandler } from "../../../lib/hooks/useQueryHandler";
import OpenIdConnectApplicationSummary from "./OpenIdConnectApplicationSummary";
import QueryToolbar from "../../QueryToolbar";
import LazyTabs from "../../LazyTabs";
import PaginatedOpenIdConnectAuthorizations from "../authorizations/PaginatedOpenIdConnectAuthorizations";
import PaginatedOpenIdConnectTokens from "../tokens/PaginatedOpenIdConnectTokens";

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
    <div>
      <Card style={{ marginBottom: "1em" }}>
        <OpenIdConnectApplicationSummary entity={application} />
      </Card>
      <QueryToolbar query={ApplicationDocument} variables={queryVariables} />
      <Divider />
      <LazyTabs
        items={[
          {
            key: "authorizations",
            label: "Authorizations",
            count: application.authorizations.totalCount,
            children: (
              <PaginatedOpenIdConnectAuthorizations
                where={{
                  application: {
                    id: { equalTo: application.uuid },
                  },
                }}
                order={{ createdAt: SortEnumType.Desc }}
              />
            ),
          },
          {
            key: "tokens",
            label: "Tokens",
            count: application.tokens.totalCount,
            children: (
              <PaginatedOpenIdConnectTokens
                where={{
                  application: {
                    id: { equalTo: application.uuid },
                  },
                }}
                order={{ createdAt: SortEnumType.Desc }}
              />
            ),
          },
        ]}
      />
    </div>
  );
}
