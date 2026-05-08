import { Scalars } from "../../__generated__/graphql";
import { DataFormatDocument } from "../../queries/dataFormats.generated";
import { Skeleton, Result, Card } from "antd";
import { useQuery } from "@apollo/client/react";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import DataFormatSummary from "./DataFormatSummary";
import QueryToolbar from "../QueryToolbar";

interface DataFormatProps {
  dataFormatId: Scalars["Uuid"]["input"];
}

export default function DataFormat({ dataFormatId }: DataFormatProps) {
  const queryVariables = {
    uuid: dataFormatId,
  };
  const { loading, error, data } = useQuery(DataFormatDocument, {
    variables: queryVariables,
  });
  useQueryHandler({ error });
  const dataFormat = data?.dataFormat;

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!dataFormat) {
    return (
      <Result
        status="500"
        title="500"
        subTitle="Sorry, something went wrong."
      />
    );
  }

  return (
    <div>
      <Card style={{ marginBottom: "1em" }}>
        <DataFormatSummary entity={dataFormat} />
      </Card>
      <QueryToolbar query={DataFormatDocument} variables={queryVariables} />
    </div>
  );
}
