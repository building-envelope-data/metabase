import { Scalars } from "../../../__generated__/graphql";
import { GeometricDataDocument } from "../../../queries/data.generated";
import { Skeleton, Result, Card, Divider } from "antd";
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
  const { loading, error, data } = useQuery(GeometricDataDocument, {
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
      />
    );
  }

  return (
    <Card>
      <GeometricDataSummary entity={theData} />
      <Divider />
      <QueryToolbar query={GeometricDataDocument} variables={queryVariables} />
    </Card>
  );
}
