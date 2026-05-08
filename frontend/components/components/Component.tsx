import { useQuery } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import { ComponentDocument } from "../../queries/components.generated";
import { Skeleton, Result, Card } from "antd";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import ComponentSummary from "./ComponentSummary";
import QueryToolbar from "../QueryToolbar";

interface ComponentProps {
  componentId: Scalars["Uuid"]["input"];
}

export default function Component({ componentId }: ComponentProps) {
  const queryVariables = {
    uuid: componentId,
  };
  const { loading, error, data } = useQuery(ComponentDocument, {
    variables: queryVariables,
  });
  useQueryHandler({ error });
  const component = data?.component;

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!component) {
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
        <ComponentSummary entity={component} />
      </Card>
      <QueryToolbar query={ComponentDocument} variables={queryVariables} />
    </div>
  );
}
