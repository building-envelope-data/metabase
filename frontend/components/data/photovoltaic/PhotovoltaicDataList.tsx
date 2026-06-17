import { PhotovoltaicDataPartialFragment } from "../../../queries/data.generated";
import EntityList from "../../entities/EntityList";
import PhotovoltaicDataSummary from "./PhotovoltaicDataSummary";
import EntityItem from "../../entities/EntityItem";

export default function PhotovoltaicDataList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: PhotovoltaicDataPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <PhotovoltaicDataSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
