import { GeometricDataPartialFragment } from "../../../queries/data.generated";
import EntityList from "../../entities/EntityList";
import GeometricDataSummary from "./GeometricDataSummary";
import EntityItem from "../../entities/EntityItem";

export default function GeometricDataList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: GeometricDataPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <GeometricDataSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
