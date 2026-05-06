import { useQuery } from "@apollo/client/react";
import { PendingInstitutionsDocument } from "../../queries/institutions.generated";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import EntityList from "../entities/EntityList";
import EntityItem from "../entities/EntityItem";
import InstitutionSummary from "./InstitutionSummary";

export default function PendingInstitutionList() {
  const { data, loading, error } = useQuery(PendingInstitutionsDocument);
  useQueryHandler({ error });
  const nodes = data?.pendingInstitutions?.edges?.map((e) => e.node) || [];

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
