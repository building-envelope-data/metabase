import { useQuery } from "@apollo/client/react";
import { PendingDatabasesDocument } from "../../queries/databases.generated";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import EntityList from "../entities/EntityList";
import EntityItem from "../entities/EntityItem";
import DatabaseSummary from "./DatabaseSummary";

export default function PendingDatabaseList() {
  const { data, loading, error } = useQuery(PendingDatabasesDocument);
  useQueryHandler({ error });
  const nodes = data?.pendingDatabases?.edges?.map((e) => e.node) || [];
  console.error(loading);
  console.error(data);
  console.error(error);

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
