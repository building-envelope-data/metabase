import {
  AuthorizationsDocument,
  AuthorizationsQueryVariables,
  OpenIdConnectAuthorizationsPartialFragment,
} from "../../../queries/openIdConnect.generated";
import OpenIdConnectAuthorizationList from "./OpenIdConnectAuthorizationList";
import PaginatedEntities from "../../entities/PaginatedEntities";
import {
  OpenIdConnectAuthorizationFilterInput,
  OpenIdConnectAuthorizationSortInput,
} from "../../../__generated__/graphql";

export default function PaginatedOpenIdConnectAuthorizations({
  where,
  order,
  extra,
  loading,
}: {
  where?: AuthorizationsQueryVariables["where"];
  order?: AuthorizationsQueryVariables["order"];
  extra?: React.ReactNode;
  showJump?: boolean;
  loading?: boolean;
}) {
  return (
    <PaginatedEntities<
      OpenIdConnectAuthorizationsPartialFragment,
      OpenIdConnectAuthorizationFilterInput,
      OpenIdConnectAuthorizationSortInput
    >
      loading={loading}
      where={where}
      order={order}
      showJump={false}
      entitiesQuery={AuthorizationsDocument}
      extra={extra}
      list={(props) => <OpenIdConnectAuthorizationList {...props} />}
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
          field: "id",

          type: "uuid",
        },
      ]}
      sortDefinitions={[
        { field: "status" },
        { field: "subject" },
        { field: "type" },
        { field: "createdAt" },
        { field: "updatedAt" },
        { field: "id" },
      ]}
    />
  );
}
