import React, { useState } from "react";
import { Select, Space, Spin } from "antd";
import { useQuery } from "@apollo/client/react";
import { useDebounce } from "../lib/hooks/useDebounce";
import { Scalars, SortEnumType } from "../__generated__/graphql";
import { TypedDocumentNode } from "@apollo/client";
import { isUuid, notEmpty } from "../lib/string";
import { isTruthy } from "../lib/array";
import Id from "./Id";

// Inspired by https://ant.design/components/select/#components-select-demo-select-users

interface PageInfo {
  endCursor: string | null;
  hasNextPage: boolean;
}

interface Item {
  uuid: Scalars["Uuid"]["output"];
  name: string;
}

interface ItemsData {
  connection: {
    edges: { node: Item }[];
    pageInfo: PageInfo;
  };
}

interface ItemsVariables {
  first: number;
  after?: string | null;
  where?: { or?: { id?: { equalTo: any }; name?: { contains: any } }[] };
  order?: { name?: SortEnumType };
}

interface BaseProps {
  query: TypedDocumentNode<any, ItemsVariables>;
  value?: string;
  style?: React.CSSProperties;
  onChange?: (value: string) => void;
}

interface SingleProps extends BaseProps {
  mode?: undefined;
}

interface MultiProps extends BaseProps {
  mode: "multiple" | "tags";
}

export type PaginatedSelectProps = SingleProps | MultiProps;

const order = {
  name: SortEnumType.Asc,
};

export default function PaginatedIdSelect({
  query,
  mode,
  value,
  style = { width: "100%" },
  onChange,
}: PaginatedSelectProps) {
  const [search, setSearch] = useState<string>("");
  const pageSize = 10;

  const filters = [
    isUuid(search) && { id: { equalTo: search } },
    notEmpty(search) && { name: { contains: search } },
  ].filter(isTruthy);
  const where = filters.length > 0 ? { or: filters } : undefined;

  const { data, loading, fetchMore, refetch } = useQuery<
    ItemsData,
    ItemsVariables
  >(query, {
    variables: {
      first: pageSize,
      after: null,
      where: where,
      order: order,
    },
  });

  const { edges, pageInfo } = data?.connection || {
    edges: [],
    pageInfo: { hasNextPage: false, endCursor: null },
  };

  const handleSearch = useDebounce((value: string) => {
    setSearch(value);
    refetch({
      first: pageSize,
      after: null,
      where: where,
      order: order,
    });
  }, 500);

  const handleScroll = (e: React.UIEvent<HTMLDivElement>) => {
    const { scrollTop, scrollHeight, clientHeight } = e.currentTarget;
    const isBottom = scrollHeight - scrollTop <= clientHeight + 10;

    if (isBottom && !loading && pageInfo?.hasNextPage) {
      fetchMore({
        variables: {
          first: pageSize,
          after: pageInfo.endCursor,
          where: where,
          order: order,
        },
      });
    }
  };

  return (
    <Select
      popupMatchSelectWidth={false}
      mode={mode}
      value={value}
      onChange={onChange}
      showSearch={{
        filterOption: false,
        onSearch: handleSearch,
      }}
      allowClear
      placeholder="Search by name or paste ID..."
      onPopupScroll={handleScroll}
      onClear={() => {
        setSearch("");
        refetch({
          after: null,
          first: pageSize,
        });
      }}
      loading={loading}
      options={edges?.map(({ node }) => ({
        label: (
          <Space>
            {node.name} (<Id value={node.uuid} />)
          </Space>
        ),
        value: node.uuid,
      }))}
      maxTagCount="responsive"
      // 36 characters is what a UUID of the form "ffffffff-ffff-ffff-ffff-ffffffffffff" has
      style={style}
      notFoundContent={loading ? <Spin size="small" /> : "No data found"}
      popupRender={(menu) => (
        <>
          {menu}
          {loading && pageInfo?.hasNextPage && (
            <div style={{ padding: "8px", textAlign: "center" }}>
              <Spin size="small" description="Loading more..." />
            </div>
          )}
        </>
      )}
    />
  );
}
