import { Scalars } from "../../../__generated__/graphql";
import { LifeCycleDataDocument } from "../../../queries/data.generated";
import { Skeleton, Result, Card, Divider } from "antd";
import { useQuery } from "@apollo/client/react";
import { useQueryHandler } from "../../../lib/hooks/useQueryHandler";
import LifeCycleDataSummary from "./LifeCycleDataSummary";
import QueryToolbar from "../../QueryToolbar";

interface LifeCycleDataProps {
  databaseId: Scalars["Uuid"]["input"];
  id: Scalars["Uuid"]["input"];
}

export default function LifeCycleData({ databaseId, id }: LifeCycleDataProps) {
  const queryVariables = {
    databaseId: databaseId,
    id: id,
  };
  const { loading, error, data } = useQuery(LifeCycleDataDocument, {
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
      <LifeCycleDataSummary entity={theData} />
      <Divider />
      <QueryToolbar query={LifeCycleDataDocument} variables={queryVariables} />
    </Card>
  );
}
