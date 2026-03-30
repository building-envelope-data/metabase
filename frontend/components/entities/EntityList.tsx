import { Card, Empty, Flex, Skeleton } from "antd";
import { Scalars } from "../../__generated__/graphql";
import { useState, useEffect } from "react";
import { initialPageSize } from "../Pagination";
import { range } from "../../lib/array";

export default function EntityList<
  TEntity extends { uuid: Scalars["Uuid"]["output"] },
>({
  loading,
  dataSource,
  renderItem,
}: {
  loading: boolean;
  dataSource: TEntity[];
  renderItem: (entity: TEntity) => React.ReactNode;
}) {
  const [hasLoadedOnce, setHasLoadedOnce] = useState(false);

  useEffect(() => {
    if (!loading && dataSource?.length > 0) {
      setHasLoadedOnce(true);
    }
  }, [loading, dataSource]);

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

  if (!loading && (!dataSource || dataSource.length === 0)) {
    return <Empty description="No data found" />;
  }

  return (
    <Flex
      vertical
      gap="middle"
      style={{
        opacity: loading ? 0.6 : 1,
        transition: "opacity 0.3s ease-in-out",
        pointerEvents: loading ? "none" : "auto", // Prevent clicks while loading
      }}
    >
      {dataSource.map((entity) => (
        <div key={entity.uuid}>{renderItem(entity)}</div>
      ))}
    </Flex>
  );
}
