import {
  ApplicationsDocument,
  ApplicationsQueryVariables,
  OpenIdConnectApplicationsPartialFragment,
} from "../../../queries/openIdConnect.generated";
import paths from "../../../paths";
import OpenIdConnectApplicationList from "./OpenIdConnectApplicationList";
import PaginatedEntities from "../../entities/PaginatedEntities";
import {
  OpenIdConnectApplicationFilterInput,
  OpenIdConnectApplicationSortInput,
} from "../../../__generated__/graphql";

export default function PaginatedOpenIdConnectApplications({
  where,
  order,
  showJump = false,
  extra,
  loading,
}: {
  where?: ApplicationsQueryVariables["where"];
  order?: ApplicationsQueryVariables["order"];
  extra?: React.ReactNode;
  showJump?: boolean;
  loading?: boolean;
}) {
  return (
    <PaginatedEntities<
      OpenIdConnectApplicationsPartialFragment,
      OpenIdConnectApplicationFilterInput,
      OpenIdConnectApplicationSortInput
    >
      loading={loading}
      baseWhere={where}
      defaultOrder={order}
      showJump={showJump}
      entitiesQuery={ApplicationsDocument}
      // namesQuery={ApplicationNamesDocument}
      route={paths.openIdConnectApplication}
      extra={extra}
      list={(props) => <OpenIdConnectApplicationList {...props} />}
      filterDefinitions={[
        {
          field: "displayName",
          type: "string",
        },
        {
          field: "clientId",
          type: "string",
        },
        {
          field: "owner",
          type: "object",
          items: [
            {
              field: "name",
              type: "string",
            },
            { field: "id", type: "uuid" },
          ],
        },
        {
          field: "id",

          type: "uuid",
        },
      ]}
      sortDefinitions={[
        { field: "displayName" },
        { field: "clientId" },
        { field: "createdAt" },
        { field: "updatedAt" },
        { field: "id" },
      ]}
    />
  );
}
