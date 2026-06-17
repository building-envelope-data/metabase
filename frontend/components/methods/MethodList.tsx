import { MethodsPartialFragment } from "../../queries/methods.generated";
import EntityItem from "../entities/EntityItem";
import EntityList from "../entities/EntityList";
import MethodSummary from "./MethodSummary";

export default function MethodList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: MethodsPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <MethodSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
