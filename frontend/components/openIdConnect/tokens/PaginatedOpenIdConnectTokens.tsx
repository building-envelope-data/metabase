import {
  TokensDocument,
  TokensQueryVariables,
  OpenIdConnectTokensPartialFragment,
} from "../../../queries/openIdConnect.generated";
import OpenIdConnectTokenList from "./OpenIdConnectTokenList";
import PaginatedEntities from "../../entities/PaginatedEntities";
import {
  OpenIdConnectTokenFilterInput,
  OpenIdConnectTokenSortInput,
} from "../../../__generated__/graphql";

export default function PaginatedOpenIdConnectTokens({
  where,
  order,
  extra,
  loading,
}: {
  where?: TokensQueryVariables["where"];
  order?: TokensQueryVariables["order"];
  extra?: React.ReactNode;
  showJump?: boolean;
  loading?: boolean;
}) {
  return (
    <PaginatedEntities<
      OpenIdConnectTokensPartialFragment,
      OpenIdConnectTokenFilterInput,
      OpenIdConnectTokenSortInput
    >
      loading={loading}
      baseWhere={where}
      defaultOrder={order}
      showJump={false}
      entitiesQuery={TokensDocument}
      extra={extra}
      list={(props) => <OpenIdConnectTokenList {...props} />}
      filterDefinitions={[
        {
          field: "status",
          type: "string",
        },
        {
          field: "subject",
          type: "string",
        },
        {
          field: "type",
          type: "string",
        },
        {
          field: "authorization",
          type: "object",
          items: [
            {
              field: "id",
              type: "uuid",
            },
            {
              field: "status",
              type: "string",
            },
            {
              field: "subject",
              type: "string",
            },
            {
              field: "type",
              type: "string",
            },
          ],
        },
        {
          field: "id",
          type: "uuid",
        },
      ]}
      sortDefinitions={[
        { field: "status" },
        { field: "subject" },
        { field: "type" },
        { field: "createdAt" },
        { field: "expiredAt" },
        { field: "redeemedAt" },
        { field: "updatedAt" },
        { field: "id" },
      ]}
    />
  );
}
