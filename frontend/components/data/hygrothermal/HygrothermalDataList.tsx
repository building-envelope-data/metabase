import { HygrothermalDataPartialFragment } from "../../../queries/data.generated";
import EntityList from "../../entities/EntityList";
import HygrothermalDataSummary from "./HygrothermalDataSummary";
import EntityItem from "../../entities/EntityItem";

export default function HygrothermalDataList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: HygrothermalDataPartialFragment[] | null;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <HygrothermalDataSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
