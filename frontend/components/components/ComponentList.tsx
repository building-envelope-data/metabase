import { ComponentsPartialFragment } from "../../queries/components.generated";
import EntityItem from "../entities/EntityItem";
import EntityList from "../entities/EntityList";
import ComponentSummary from "./ComponentSummary";

export default function ComponentList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: ComponentsPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <ComponentSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
