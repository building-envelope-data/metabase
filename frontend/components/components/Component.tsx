import { useQuery } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  ComponentDataTotalCountsDocument,
  ComponentDataTotalCountsPartialFragment,
  ComponentDocument,
} from "../../queries/components.generated";
import { Skeleton, Result, Card, Divider, Button } from "antd";
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

const getDataTabs = (
  componentId: Scalars["Uuid"]["output"],
  dataTotalCounts: ComponentDataTotalCountsPartialFragment,
) => [
  {
    key: "calorimetric",
    count: dataTotalCounts?.allCalorimetricData.totalCount,
    label: "Calorimetric Data",
    children: (
      <PaginatedCalorimetricData
        where={{ componentId: { equalTo: componentId } }}
      />
    ),
  },
  {
    key: "geometric",
    count: dataTotalCounts?.allGeometricData.totalCount,
    label: "Geometric Data",
    children: (
      <PaginatedGeometricData
        where={{ componentId: { equalTo: componentId } }}
      />
    ),
  },
  {
    key: "hygrothermal",
    count: dataTotalCounts?.allHygrothermalData.totalCount,
    label: "Hygrothermal Data",
    children: (
      <PaginatedHygrothermalData
        where={{ componentId: { equalTo: componentId } }}
      />
    ),
  },
  {
    key: "lifeCycle",
    count: dataTotalCounts?.allLifeCycleData.totalCount,
    label: "Life-Cycle Data",
    children: (
      <PaginatedLifeCycleData
        where={{ componentId: { equalTo: componentId } }}
      />
    ),
  },
  {
    key: "optical",
    count: dataTotalCounts?.allOpticalData.totalCount,
    label: "Optical Data",
    children: (
      <PaginatedOpticalData where={{ componentId: { equalTo: componentId } }} />
    ),
  },
  {
    key: "photovoltaic",
    count: dataTotalCounts?.allPhotovoltaicData.totalCount,
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
  const { loading, error, data, refetch } = useQuery(ComponentDocument, {
    variables: queryVariables,
  });
  useQueryHandler({ error });

  const {
    loading: totalCountsLoading,
    data: dataTotalCountsData,
    refetch: totalCountsRefetch,
  } = useQuery(ComponentDataTotalCountsDocument, {
    variables: queryVariables,
  });

  const component = data?.component;
  const dataTotalCounts = dataTotalCountsData?.component;

  const dataTabs = useMemo(
    () =>
      dataTotalCounts == null
        ? null
        : getDataTabs(componentId, dataTotalCounts),
    [componentId, dataTotalCounts],
  );

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!component) {
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
        <ComponentSummary entity={component} />
      </Card>
      <QueryToolbar query={ComponentDocument} variables={queryVariables} />
      <Divider />
      {totalCountsLoading ? (
        <Skeleton active avatar title />
      ) : dataTabs == null ? (
        <Result
          status="error"
          title="Data Loading Failed"
          subTitle="Could not load associated data from databases."
          extra={
            <Button
              loading={totalCountsLoading}
              onClick={() => totalCountsRefetch()}
            >
              Reload
            </Button>
          }
        />
      ) : (
        <LazyTabs items={dataTabs} />
      )}
    </div>
  );
}
