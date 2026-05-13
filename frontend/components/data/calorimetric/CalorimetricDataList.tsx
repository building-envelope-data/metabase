import { CalorimetricDataPartialFragment } from "../../../queries/data.generated";
import EntityList from "../../entities/EntityList";
import CalorimetricDataSummary from "./CalorimetricDataSummary";
import EntityItem from "../../entities/EntityItem";

export default function CalorimetricDataList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: CalorimetricDataPartialFragment[] | null;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <CalorimetricDataSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
