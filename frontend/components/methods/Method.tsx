import { useQuery } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import { MethodDocument } from "../../queries/methods.generated";
import { Skeleton, Result, Card } from "antd";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import MethodSummary from "./MethodSummary";
import QueryToolbar from "../QueryToolbar";

interface MethodProps {
  methodId: Scalars["Uuid"]["input"];
}

export default function Method({ methodId }: MethodProps) {
  const queryVariables = {
    uuid: methodId,
  };
  const { loading, error, data } = useQuery(MethodDocument, {
    variables: queryVariables,
  });
  useQueryHandler({ error });
  const method = data?.method;

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!method) {
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
        <MethodSummary entity={method} />
      </Card>
      <QueryToolbar query={MethodDocument} variables={queryVariables} />
    </div>
  );
}
