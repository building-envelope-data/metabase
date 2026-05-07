import {
  UsersDocument,
  UserNamesDocument,
  UsersQueryVariables,
  UsersPartialFragment,
} from "../../queries/users.generated";
import paths from "../../paths";
import UserList from "./UserList";
import PaginatedEntities from "../entities/PaginatedEntities";
import { UserFilterInput, UserSortInput } from "../../__generated__/graphql";

export default function PaginatedUsers({
  where,
  order,
  showJump = false,
  extra,
}: {
  where?: UsersQueryVariables["where"];
  order?: UsersQueryVariables["order"];
  showJump?: boolean;
  extra?: React.ReactNode;
}) {
  return (
    <PaginatedEntities<UsersPartialFragment, UserFilterInput, UserSortInput>
      entitiesQuery={UsersDocument}
      namesQuery={UserNamesDocument}
      where={where}
      order={order}
      showJump={showJump}
      route={paths.user}
      extra={extra}
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
