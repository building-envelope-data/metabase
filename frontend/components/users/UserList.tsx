import { UsersPartialFragment } from "../../queries/users.generated";
import EntityList from "../entities/EntityList";
import UserSummary from "./UserSummary";
import EntityItem from "../entities/EntityItem";

export default function UserList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: UsersPartialFragment[] | null;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <UserSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
