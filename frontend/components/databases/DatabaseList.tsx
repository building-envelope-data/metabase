import { DatabasesPartialFragment } from "../../queries/databases.generated";
import EntityList from "../entities/EntityList";
import DatabaseSummary from "./DatabaseSummary";
import EntityItem from "../entities/EntityItem";

export default function DatabaseList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: DatabasesPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <DatabaseSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
