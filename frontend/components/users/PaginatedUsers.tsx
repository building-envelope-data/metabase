import {
  UsersDocument,
  UserNamesDocument,
  UsersQueryVariables,
} from "../../queries/users.generated";
import paths from "../../paths";
import UserList from "./UserList";
import PaginatedEntities from "../entities/PaginatedEntities";

export default function PaginatedUsers({
  where,
  showJump = false,
}: {
  where?: UsersQueryVariables["where"];
  showJump?: boolean;
}) {
  return (
    <PaginatedEntities
      entitiesQuery={UsersDocument}
      namesQuery={UserNamesDocument}
      where={where}
      showJump={showJump}
      route={paths.user}
      list={(props) => <UserList {...props} />}
      filterDefinitions={[
        {
          field: "name",
          type: "string",
        },
        {
          field: "representedInstitutions",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "name",
                type: "string",
              },
              {
                field: "id",
                type: "uuid",
              },
            ],
          },
        },
        {
          field: "developedMethods",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "name",
                type: "string",
              },
              {
                field: "id",
                type: "uuid",
              },
            ],
          },
        },
        {
          field: "id",
          type: "uuid",
        },
      ]}
      sortDefinitions={[
        { field: "name" },
        { field: "createdAt" },
        { field: "updatedAt" },
        { field: "id" },
      ]}
    />
  );
}
