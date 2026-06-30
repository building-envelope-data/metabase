import { DataFormatsPartialFragment } from "../../queries/dataFormats.generated";
import EntityList from "../entities/EntityList";
import DataFormatSummary from "./DataFormatSummary";
import EntityItem from "../entities/EntityItem";

export default function DataFormatList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: DataFormatsPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <DataFormatSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
