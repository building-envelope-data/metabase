import { Component } from "../../__generated__/__types__";
import { Table } from "antd";
import { useState } from "react";
import { setMapValue } from "../../lib/freeTextFilter";
import { ComponentCategory } from "../../__generated__/__types__";
import paths from "../../paths";
import {
  getAbbreviationColumnProps,
  getDescriptionColumnProps,
  getFilterableEnumListColumnProps,
  getNameColumnProps,
  getUuidColumnProps,
} from "../../lib/table";

export type ComponentTableProps = {
  loading: boolean;
  components: Pick<
    Component,
    "uuid" | "name" | "abbreviation" | "description" | "categories"
  >[];
};

export function ComponentTable({ loading, components }: ComponentTableProps) {
  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

  return (
    <Table
      loading={loading}
      columns={[
        getUuidColumnProps<(typeof components)[0]>(
          onFilterTextChange,
          (x) => filterText.get(x),
          paths.component
        ),
        getNameColumnProps<(typeof components)[0]>(onFilterTextChange, (x) =>
          filterText.get(x)
        ),
        getAbbreviationColumnProps<(typeof components)[0]>(
          onFilterTextChange,
          (x) => filterText.get(x)
        ),
        getDescriptionColumnProps<(typeof components)[0]>(
          onFilterTextChange,
          (x) => filterText.get(x)
        ),
        getFilterableEnumListColumnProps<
          (typeof components)[0],
          ComponentCategory
        >(
          "Categories",
          "categories",
          Object.entries(ComponentCategory),
          (record) => record.categories,
          onFilterTextChange,
          (x) => filterText.get(x)
        ),
      ]}
      dataSource={components}
    />
  );
}
