import { useState } from "react";
import { Button, Input, Space } from "antd";
import { useRouter } from "next/router";
import { Scalars } from "../__generated__/graphql";
import { Route } from "next";
import PaginatedIdSelect, { PaginatedSelectProps } from "./PaginatedIdSelect";

export type JumpToIdProps = {
  query?: PaginatedSelectProps["query"];
  route: (id: Scalars["Uuid"]["output"]) => Route;
};

export default function JumpToId({ query, route }: JumpToIdProps) {
  const router = useRouter();
  const [id, setId] = useState("");

  const handleJump = () => {
    if (id) {
      router.push(route(id));
    }
  };

  return (
    <Space.Compact>
      {/* 36 characters is what a UUID of the form "ffffffff-ffff-ffff-ffff-ffffffffffff" has */}
      {query ? (
        <PaginatedIdSelect
          value={id}
          query={query}
          style={{ minWidth: "66ch" }}
          onChange={setId}
        />
      ) : (
        <Input
          placeholder="ID"
          style={{ fontFamily: "monospace", width: "66ch" }}
          maxLength={36}
          value={id}
          onChange={(e) => setId(e.target.value)}
        />
      )}
      <Button type="primary" onClick={handleJump}>
        Jump to Page
      </Button>
    </Space.Compact>
  );
}
