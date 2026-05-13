import { MethodsPartialFragment } from "../../queries/methods.generated";
import EntityItem from "../entities/EntityItem";
import EntityList from "../entities/EntityList";
import MethodSummary from "./MethodSummary";

export default function MethodList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: MethodsPartialFragment[] | null;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <MethodSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
