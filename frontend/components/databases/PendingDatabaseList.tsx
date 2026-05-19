import { useQuery } from "@apollo/client/react";
import { PendingDatabasesDocument } from "../../queries/databases.generated";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import EntityList from "../entities/EntityList";
import EntityItem from "../entities/EntityItem";
import DatabaseSummary from "./DatabaseSummary";

export default function PendingDatabaseList() {
  const { data, loading, error } = useQuery(PendingDatabasesDocument, {
    // always fetch instead of erroneously using the cached pending databases that contain only the total count coming from `query CurrentUser`
    fetchPolicy: "network-only",
  });
  useQueryHandler({ error });
  const nodes = data?.pendingDatabases?.edges?.map((e) => e.node) || [];

  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <DatabaseSummary hideInputControls showVerifyAnyway entity={node} />
        </EntityItem>
      )}
    />
  );
}
