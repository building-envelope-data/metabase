import { Scalars } from "../../../__generated__/graphql";
import { OpticalDataDocument } from "../../../queries/data.generated";
import { Skeleton, Result, Card, Divider } from "antd";
import { useQuery } from "@apollo/client/react";
import { useQueryHandler } from "../../../lib/hooks/useQueryHandler";
import OpticalDataSummary from "./OpticalDataSummary";
import OpticalDataRibbon from "./OpticalDataRibbon";
import QueryToolbar from "../../QueryToolbar";

interface OpticalDataProps {
  databaseId: Scalars["Uuid"]["input"];
  id: Scalars["Uuid"]["input"];
}

export default function OpticalData({ databaseId, id }: OpticalDataProps) {
  const queryVariables = {
    databaseId: databaseId,
    id: id,
  };
  const { loading, error, data } = useQuery(OpticalDataDocument, {
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
    <OpticalDataRibbon {...theData}>
      <Card>
        <OpticalDataSummary entity={theData} />
        <Divider />
        <QueryToolbar query={OpticalDataDocument} variables={queryVariables} />
      </Card>
    </OpticalDataRibbon>
  );
}
