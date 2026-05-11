import { Card, Empty, Flex, Skeleton } from "antd";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { initialPageSize } from "../Pagination";
import { range } from "../../lib/array";
import { usePreviousNonNull } from "../../lib/hooks/usePreviousNonNull";

export default function EntityList<
  TEntity extends { uuid: Scalars["Uuid"]["output"] },
>({
  loading,
  dataSource,
  renderItem,
}: {
  loading: boolean;
  dataSource: TEntity[] | null;
  renderItem: (entity: TEntity) => React.ReactNode;
}) {
  const [hasLoadedOnce, setHasLoadedOnce] = useState(false);
  const previousDataSource = usePreviousNonNull(dataSource);
  const currentOrPreviousDataSource = dataSource ?? previousDataSource ?? [];

  if (!hasLoadedOnce && !loading) {
    setHasLoadedOnce(true);
  }

  if (loading && !hasLoadedOnce) {
    return (
      <Flex vertical gap="middle">
        {range(1, initialPageSize).map((key) => (
          <Card key={key}>
            <Skeleton active avatar paragraph={{ rows: 2 }} />
          </Card>
        ))}
      </Flex>
    );
  }

  if (!loading && currentOrPreviousDataSource.length === 0) {
    return <Empty description="No data found" />;
  }

  return (
    <Flex
      vertical
      gap="middle"
      style={{
        opacity: loading ? 0.4 : 1,
        transition: "opacity 0.3s ease-in-out",
        pointerEvents: loading ? "none" : "auto", // prevent clicks while loading
      }}
    >
      {currentOrPreviousDataSource.map((entity) => (
        <div key={entity.uuid}>{renderItem(entity)}</div>
      ))}
    </Flex>
  );
}
