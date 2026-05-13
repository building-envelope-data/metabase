import { GeometricDataPartialFragment } from "../../../queries/data.generated";
import EntityList from "../../entities/EntityList";
import GeometricDataSummary from "./GeometricDataSummary";
import EntityItem from "../../entities/EntityItem";

export default function GeometricDataList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: GeometricDataPartialFragment[] | null;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <GeometricDataSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
