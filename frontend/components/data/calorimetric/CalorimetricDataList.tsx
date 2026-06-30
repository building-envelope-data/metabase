import { CalorimetricDataPartialFragment } from "../../../queries/data.generated";
import EntityList from "../../entities/EntityList";
import CalorimetricDataSummary from "./CalorimetricDataSummary";
import EntityItem from "../../entities/EntityItem";

export default function CalorimetricDataList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: CalorimetricDataPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <CalorimetricDataSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
