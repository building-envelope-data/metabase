import { Scalars } from "../../../__generated__/graphql";
import { HygrothermalDataDocument } from "../../../queries/data.generated";
import { Skeleton, Result, Card, Button } from "antd";
import { useQuery } from "@apollo/client/react";
import { useQueryHandler } from "../../../lib/hooks/useQueryHandler";
import HygrothermalDataSummary from "./HygrothermalDataSummary";
import QueryToolbar from "../../QueryToolbar";

interface HygrothermalDataProps {
  databaseId: Scalars["Uuid"]["input"];
  id: Scalars["Uuid"]["input"];
}

export default function HygrothermalData({
  databaseId,
  id,
}: HygrothermalDataProps) {
  const queryVariables = {
    databaseId: databaseId,
    id: id,
  };
  const { loading, error, data, refetch } = useQuery(HygrothermalDataDocument, {
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
        <HygrothermalDataSummary entity={theData} />
      </Card>
      <QueryToolbar
        query={HygrothermalDataDocument}
        variables={queryVariables}
      />
    </div>
  );
}
