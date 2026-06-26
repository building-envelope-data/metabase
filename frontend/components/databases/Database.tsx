import { Scalars } from "../../__generated__/graphql";
import { DatabaseDocument } from "../../queries/databases.generated";
import { Skeleton, Result, Card, Button } from "antd";
import { useQuery } from "@apollo/client/react";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import DatabaseSummary from "./DatabaseSummary";
import QueryToolbar from "../QueryToolbar";

interface DatabaseProps {
  databaseId: Scalars["Uuid"]["input"];
}

export default function Database({ databaseId }: DatabaseProps) {
  const queryVariables = {
    id: databaseId,
  };
  const { loading, error, data, refetch } = useQuery(DatabaseDocument, {
    variables: queryVariables,
  });
  useQueryHandler({ error });
  const database = data?.database;

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!database) {
    return (
      <Result
        status="500"
        title="500"
        subTitle="Sorry, something went wrong."
        extra={
          <Button loading={loading} onClick={() => refetch()}>
            Reload
          </Button>
        }
      />
    );
  }

  return (
    <div>
      <Card style={{ marginBottom: "1em" }}>
        <DatabaseSummary entity={database} />
      </Card>
      <QueryToolbar query={DatabaseDocument} variables={queryVariables} />
    </div>
  );
}
