import { HygrothermalDataPartialFragment } from "../../../queries/data.generated";
import EntityList from "../../entities/EntityList";
import HygrothermalDataSummary from "./HygrothermalDataSummary";
import EntityItem from "../../entities/EntityItem";

export default function HygrothermalDataList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: HygrothermalDataPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <HygrothermalDataSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
