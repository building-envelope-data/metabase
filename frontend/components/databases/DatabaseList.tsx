import { DatabasesPartialFragment } from "../../queries/databases.generated";
import EntityList from "../entities/EntityList";
import DatabaseSummary from "./DatabaseSummary";
import EntityItem from "../entities/EntityItem";

export default function DatabaseList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: DatabasesPartialFragment[] | null;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <DatabaseSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
