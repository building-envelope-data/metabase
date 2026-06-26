import { Scalars } from "../../../__generated__/graphql";
import { GeometricDataDocument } from "../../../queries/data.generated";
import { Skeleton, Result, Card, Button } from "antd";
import { useQuery } from "@apollo/client/react";
import { useQueryHandler } from "../../../lib/hooks/useQueryHandler";
import GeometricDataSummary from "./GeometricDataSummary";
import QueryToolbar from "../../QueryToolbar";

interface GeometricDataProps {
  databaseId: Scalars["Uuid"]["input"];
  id: Scalars["Uuid"]["input"];
}

export default function GeometricData({ databaseId, id }: GeometricDataProps) {
  const queryVariables = {
    databaseId: databaseId,
    id: id,
  };
  const { loading, error, data, refetch } = useQuery(GeometricDataDocument, {
    variables: queryVariables,
  });
  useQueryHandler({ error });
  const theData = data?.data;

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!theData) {
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
        <GeometricDataSummary entity={theData} />
      </Card>
      <QueryToolbar query={GeometricDataDocument} variables={queryVariables} />
    </div>
  );
}
