import { InstitutionsPartialFragment } from "../../queries/institutions.generated";
import EntityList from "../entities/EntityList";
import InstitutionSummary from "./InstitutionSummary";
import EntityItem from "../entities/EntityItem";

export default function InstitutionList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: InstitutionsPartialFragment[] | null;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <InstitutionSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
