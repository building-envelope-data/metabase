import { Table } from "antd";
import {
  getExternallyLinkedFilterableLocatorColumnProps,
  getFilterableStringColumnProps,
  getNameColumnProps,
  getDescriptionColumnProps,
  getUuidColumnProps,
  getReferenceColumnProps,
} from "../../lib/table";
import paths from "../../paths";
import { useState } from "react";
import { setMapValue } from "../../lib/freeTextFilter";
import { DataFormatsPartialFragment } from "../../queries/dataFormats.generated";

interface DataFormatTableProps {
  loading: boolean;
  dataFormats: DataFormatsPartialFragment[];
};

export function DataFormatTable({
  loading,
  dataFormats,
}: DataFormatTableProps) {
  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

  return (
    <Table
      loading={loading}
      columns={[
        {
          ...getUuidColumnProps<(typeof dataFormats)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x),
            paths.dataFormat,
          ),
        },
        {
          ...getNameColumnProps<(typeof dataFormats)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x),
          ),
        },
        {
          ...getDescriptionColumnProps<(typeof dataFormats)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x),
          ),
        },
        {
          ...getFilterableStringColumnProps<(typeof dataFormats)[0]>(
            "Media Type",
            "mediaType",
            (record) => record.mediaType,
            onFilterTextChange,
            (x) => filterText.get(x),
          ),
        },
        {
          ...getFilterableStringColumnProps<(typeof dataFormats)[0]>(
            "Extension",
            "extension",
            (record) => record.extension,
            onFilterTextChange,
            (x) => filterText.get(x),
          ),
        },
        {
          ...getExternallyLinkedFilterableLocatorColumnProps<
            (typeof dataFormats)[0]
          >(
            "Schema",
            "schemaLocator",
            (record) => record.schemaLocator,
            onFilterTextChange,
            (x) => filterText.get(x),
          ),
        },
        {
          ...getReferenceColumnProps<(typeof dataFormats)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x),
          ),
        },
      ]}
      dataSource={dataFormats}
    />
  );
}
