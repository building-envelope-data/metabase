import { useQuery } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import { ComponentDocument } from "../../queries/components.generated";
import { Skeleton, Result, Card, Divider } from "antd";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import ComponentSummary from "./ComponentSummary";
import QueryToolbar from "../QueryToolbar";
import PaginatedOpticalData from "../data/optical/PaginatedOpticalData";
import { useMemo } from "react";
import LazyTabs from "../LazyTabs";
import PaginatedCalorimetricData from "../data/calorimetric/PaginatedCalorimetricData";
import PaginatedGeometricData from "../data/geometric/PaginatedGeometricData";
import PaginatedHygrothermalData from "../data/hygrothermal/PaginatedHygrothermalData";
import PaginatedPhotovoltaicData from "../data/photovoltaic/PaginatedPhotovoltaicData";
import PaginatedLifeCycleData from "../data/lifeCycle/PaginatedLifeCycleData";

const getDataTabs = (componentId: Scalars["Uuid"]["output"]) => [
  {
    key: "calorimetric",
    label: "Calorimetric Data",
    children: (
      <PaginatedCalorimetricData
        where={{ componentId: { equalTo: componentId } }}
      />
    ),
  },
  {
    key: "geometric",
    label: "Geometric Data",
    children: (
      <PaginatedGeometricData
        where={{ componentId: { equalTo: componentId } }}
      />
    ),
  },
  {
    key: "hygrothermal",
    label: "Hygrothermal Data",
    children: (
      <PaginatedHygrothermalData
        where={{ componentId: { equalTo: componentId } }}
      />
    ),
  },
  {
    key: "lifeCycle",
    label: "Life-Cycle Data",
    children: (
      <PaginatedLifeCycleData
        where={{ componentId: { equalTo: componentId } }}
      />
    ),
  },
  {
    key: "optical",
    label: "Optical Data",
    children: (
      <PaginatedOpticalData where={{ componentId: { equalTo: componentId } }} />
    ),
  },
  {
    key: "photovoltaic",
    label: "Photovoltaic Data",
    children: (
      <PaginatedPhotovoltaicData
        where={{ componentId: { equalTo: componentId } }}
      />
    ),
  },
];

interface ComponentProps {
  componentId: Scalars["Uuid"]["input"];
}

export default function Component({ componentId }: ComponentProps) {
  const queryVariables = {
    id: componentId,
  };
  const { loading, error, data } = useQuery(ComponentDocument, {
    variables: queryVariables,
  });
  useQueryHandler({ error });
  const component = data?.component;

  const dataTabs = useMemo(() => getDataTabs(componentId), [componentId]);

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
      <Divider />
      <LazyTabs items={dataTabs} />
    </div>
  );
}
