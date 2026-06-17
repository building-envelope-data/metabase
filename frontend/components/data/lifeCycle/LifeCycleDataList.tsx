import { LifeCycleDataPartialFragment } from "../../../queries/data.generated";
import EntityList from "../../entities/EntityList";
import LifeCycleDataSummary from "./LifeCycleDataSummary";
import EntityItem from "../../entities/EntityItem";

export default function LifeCycleDataList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: LifeCycleDataPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <LifeCycleDataSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
