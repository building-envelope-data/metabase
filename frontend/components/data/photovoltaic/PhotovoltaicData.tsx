import { Scalars } from "../../../__generated__/graphql";
import { PhotovoltaicDataDocument } from "../../../queries/data.generated";
import { Skeleton, Result, Card, Button } from "antd";
import { useQuery } from "@apollo/client/react";
import { useQueryHandler } from "../../../lib/hooks/useQueryHandler";
import PhotovoltaicDataSummary from "./PhotovoltaicDataSummary";
import QueryToolbar from "../../QueryToolbar";

interface PhotovoltaicDataProps {
  databaseId: Scalars["Uuid"]["input"];
  id: Scalars["Uuid"]["input"];
}

export default function PhotovoltaicData({
  databaseId,
  id,
}: PhotovoltaicDataProps) {
  const queryVariables = {
    databaseId: databaseId,
    id: id,
  };
  const { loading, error, data, refetch } = useQuery(PhotovoltaicDataDocument, {
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
        <PhotovoltaicDataSummary entity={theData} />
      </Card>
      <QueryToolbar
        query={PhotovoltaicDataDocument}
        variables={queryVariables}
      />
    </div>
  );
}
