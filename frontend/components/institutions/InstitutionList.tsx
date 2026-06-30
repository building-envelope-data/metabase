import { InstitutionsPartialFragment } from "../../queries/institutions.generated";
import EntityList from "../entities/EntityList";
import InstitutionSummary from "./InstitutionSummary";
import EntityItem from "../entities/EntityItem";

export default function InstitutionList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: InstitutionsPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <InstitutionSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
