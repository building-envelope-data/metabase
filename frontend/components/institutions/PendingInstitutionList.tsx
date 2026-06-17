import { useQuery } from "@apollo/client/react";
import { PendingInstitutionsDocument } from "../../queries/institutions.generated";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import EntityList from "../entities/EntityList";
import EntityItem from "../entities/EntityItem";
import InstitutionSummary from "./InstitutionSummary";

export default function PendingInstitutionList() {
  const { data, loading, error, refetch } = useQuery(
    PendingInstitutionsDocument,
    {
      // always fetch instead of erroneously using the cached pending institutions that contain only the total count coming from `query CurrentUser`
      fetchPolicy: "network-only",
    },
  );
  useQueryHandler({ error });
  const nodes = data?.pendingInstitutions?.edges?.map((e) => e.node) || [];

  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={refetch}
      renderItem={(node) => (
        <EntityItem>
          <InstitutionSummary
            hideInputControls
            showVerifyAnyway
            entity={node}
          />
        </EntityItem>
      )}
    />
  );
}
