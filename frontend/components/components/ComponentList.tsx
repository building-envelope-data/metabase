import { ComponentsPartialFragment } from "../../queries/components.generated";
import EntityItem from "../entities/EntityItem";
import EntityList from "../entities/EntityList";
import ComponentSummary from "./ComponentSummary";

export default function ComponentList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: ComponentsPartialFragment[];
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <ComponentSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
