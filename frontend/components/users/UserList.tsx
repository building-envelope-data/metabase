import { UsersPartialFragment } from "../../queries/users.generated";
import EntityList from "../entities/EntityList";
import UserSummary from "./UserSummary";
import EntityItem from "../entities/EntityItem";

export default function UserList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: UsersPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <UserSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
