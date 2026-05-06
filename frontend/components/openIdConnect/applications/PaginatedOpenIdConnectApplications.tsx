import {
  ApplicationsDocument,
  ApplicationsQueryVariables,
} from "../../../queries/openIdConnect.generated";
import paths from "../../../paths";
import OpenIdConnectApplicationList from "./OpenIdConnectApplicationList";
import PaginatedEntities from "../../entities/PaginatedEntities";

export default function PaginatedOpenIdConnectApplications({
  where,
  showJump = false,
  loading,
}: {
  where?: ApplicationsQueryVariables["where"];
  showJump?: boolean;
  loading?: boolean;
}) {
  return (
    <PaginatedEntities
      loading={loading}
      where={where}
      showJump={showJump}
      entitiesQuery={ApplicationsDocument}
      // namesQuery={ApplicationNamesDocument}
      route={paths.openIdConnectApplication}
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
