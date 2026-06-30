import { CSSProperties, useState } from "react";
import { Button, Input, Space } from "antd";
import { useRouter } from "next/router";
import { Scalars } from "../__generated__/graphql";
import { Route } from "next";
import PaginatedIdSelect, { PaginatedSelectProps } from "./PaginatedIdSelect";
import { isUuid } from "../lib/string";

export type JumpToIdProps = {
  query?: PaginatedSelectProps["query"];
  route: (id: Scalars["Uuid"]["output"]) => Route;
  style?: CSSProperties;
};

export default function JumpToId({ query, route, style }: JumpToIdProps) {
  const router = useRouter();
  const [id, setId] = useState<string | undefined>(undefined);

  const jump = (id: string | undefined) => {
    if (id && isUuid(id)) {
      router.push(route(id));
    }
  };

  return (
    <Space.Compact>
      {/* 36 characters is what a UUID of the form "ffffffff-ffff-ffff-ffff-ffffffffffff" has */}
      {query ? (
        <PaginatedIdSelect
          query={query}
          style={{ minWidth: "66ch", ...style }}
          onChange={jump}
        />
      ) : (
        <Input
          placeholder="Paste ID..."
          style={{ fontFamily: "monospace", minWidth: "66ch", ...style }}
          maxLength={36}
          onChange={(e) => setId(e.target.value)}
        />
      )}
      <Button type="primary" onClick={() => jump(id)}>
        Jump to Page
      </Button>
    </Space.Compact>
  );
}
