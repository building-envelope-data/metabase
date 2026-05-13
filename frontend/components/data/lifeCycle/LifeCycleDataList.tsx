import { LifeCycleDataPartialFragment } from "../../../queries/data.generated";
import EntityList from "../../entities/EntityList";
import LifeCycleDataSummary from "./LifeCycleDataSummary";
import EntityItem from "../../entities/EntityItem";

export default function LifeCycleDataList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: LifeCycleDataPartialFragment[] | null;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <LifeCycleDataSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
