import paths from "../../paths";
import { Table } from "antd";
import { useState } from "react";
import { setMapValue } from "../../lib/freeTextFilter";
import {
  getExternallyLinkedFilterableLocatorColumnProps,
  getNameColumnProps,
  getDescriptionColumnProps,
  getInternallyLinkedFilterableStringColumnProps,
  getUuidColumnProps,
} from "../../lib/table";
import { Database, Institution } from "../../__generated__/__types__";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/
export type DatabaseTableProps = {
  loading: boolean;
  databases: (Pick<Database, "uuid" | "name" | "description" | "locator"> & {
    operator: { node: Pick<Institution, "uuid" | "name"> };
  })[];
};

export function DatabaseTable({ loading, databases }: DatabaseTableProps) {
  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

  return (
    <Table
      loading={loading}
      columns={[
        {
          ...getUuidColumnProps<(typeof databases)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x),
            paths.database
          ),
        },
        {
          ...getNameColumnProps<(typeof databases)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x)
          ),
        },
        {
          ...getDescriptionColumnProps<(typeof databases)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x)
          ),
        },
        {
          ...getExternallyLinkedFilterableLocatorColumnProps<
            (typeof databases)[0]
          >(
            "Locator",
            "locator",
            (record) => record.locator,
            onFilterTextChange,
            (x) => filterText.get(x)
          ),
        },
        {
          ...getInternallyLinkedFilterableStringColumnProps<
            (typeof databases)[0]
          >(
            "Operator",
            "operator",
            (record) => record.operator.node.name,
            onFilterTextChange,
            (x) => filterText.get(x),
            (x) => paths.institution(x.operator.node.uuid)
          ),
        },
      ]}
      dataSource={databases}
    />
  );
}

export default DatabaseTable;
