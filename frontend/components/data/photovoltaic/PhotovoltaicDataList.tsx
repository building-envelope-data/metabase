import { PhotovoltaicDataPartialFragment } from "../../../queries/data.generated";
import EntityList from "../../entities/EntityList";
import PhotovoltaicDataSummary from "./PhotovoltaicDataSummary";
import EntityItem from "../../entities/EntityItem";

export default function PhotovoltaicDataList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: PhotovoltaicDataPartialFragment[] | null;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <PhotovoltaicDataSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
