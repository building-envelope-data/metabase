import { Table } from "antd";
import { useState } from "react";
import paths from "../../paths";
import { setMapValue } from "../../lib/freeTextFilter";
import {
  getReferenceColumnProps,
  getExternallyLinkedFilterableLocatorColumnProps,
  getFilterableEnumListColumnProps,
  getNameColumnProps,
  getDescriptionColumnProps,
  getUuidColumnProps,
} from "../../lib/table";
import { Method, MethodCategory } from "../../__generated__/__types__";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

export type MethodTableProps = {
  loading: boolean;
  methods: Pick<
    Method,
    | "uuid"
    | "name"
    | "description"
    | "categories"
    | "calculationLocator"
    | "reference"
  >[];
};

export function MethodTable({ loading, methods }: MethodTableProps) {
  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

  return (
    <Table
      loading={loading}
      columns={[
        {
          ...getUuidColumnProps<(typeof methods)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x),
            paths.method
          ),
        },
        {
          ...getNameColumnProps<(typeof methods)[0]>(onFilterTextChange, (x) =>
            filterText.get(x)
          ),
        },
        {
          ...getDescriptionColumnProps<(typeof methods)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x)
          ),
        },
        {
          ...getFilterableEnumListColumnProps<
            (typeof methods)[0],
            MethodCategory
          >(
            "Categories",
            "categories",
            Object.entries(MethodCategory),
            (record) => record.categories,
            onFilterTextChange,
            (x) => filterText.get(x)
          ),
        },
        {
          ...getExternallyLinkedFilterableLocatorColumnProps<
            (typeof methods)[0]
          >(
            "Calculation",
            "calculationLocator",
            (record) => record.calculationLocator,
            onFilterTextChange,
            (x) => filterText.get(x)
          ),
        },
        {
          ...getReferenceColumnProps<(typeof methods)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x)
          ),
        },
      ]}
      dataSource={methods}
    />
  );
}

export default MethodTable;
