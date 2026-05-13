import { DataFormatsPartialFragment } from "../../queries/dataFormats.generated";
import EntityList from "../entities/EntityList";
import DataFormatSummary from "./DataFormatSummary";
import EntityItem from "../entities/EntityItem";

export default function DataFormatList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: DataFormatsPartialFragment[] | null;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <DataFormatSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
