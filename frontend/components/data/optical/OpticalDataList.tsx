import { OpticalDataPartialFragment } from "../../../queries/data.generated";
import EntityList from "../../entities/EntityList";
import OpticalDataSummary from "./OpticalDataSummary";
import EntityItem from "../../entities/EntityItem";
import OpticalDataRibbon from "./OpticalDataRibbon";

export default function OpticalDataList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: OpticalDataPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <OpticalDataRibbon {...node}>
          <EntityItem>
            <OpticalDataSummary entity={node} />
          </EntityItem>
        </OpticalDataRibbon>
      )}
    />
  );
}
