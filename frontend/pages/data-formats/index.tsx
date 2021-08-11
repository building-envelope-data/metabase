import Layout from "../../components/Layout";
import { Typography, Table, message } from "antd";
import { useDataFormatsQuery } from "../../queries/dataFormats.graphql";
import { useEffect, useState } from "react";
import paths from "../../paths";
import Link from "next/link";
import { setMapValue } from "../../lib/freeTextFilter";
import { getFilterableStringColumnProps } from "../../lib/table";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useDataFormatsQuery();
  const nodes = data?.dataFormats?.nodes || [];

  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

  useEffect(() => {
    if (error) {
      message.error(error);
    }
  }, [error]);

  return (
    <Layout>
      <Table
        loading={loading}
        columns={[
          {
            ...getFilterableStringColumnProps<typeof nodes[0]>(
              "UUID",
              "uuid",
              (record) => record.uuid,
              onFilterTextChange,
              (x) => filterText.get(x),
              (record, _highlightedValue, value) => (
                <Link href={paths.dataFormat(record.uuid)}>{value}</Link>
              )
            ),
          },
          {
            ...getFilterableStringColumnProps<typeof nodes[0]>(
              "Name",
              "name",
              (record) => record.name,
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getFilterableStringColumnProps<typeof nodes[0]>(
              "Description",
              "description",
              (record) => record.description,
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getFilterableStringColumnProps<typeof nodes[0]>(
              "Media Type",
              "mediaType",
              (record) => record.mediaType,
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getFilterableStringColumnProps<typeof nodes[0]>(
              "Extension",
              "extension",
              (record) => record.extension,
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getFilterableStringColumnProps<typeof nodes[0]>(
              "Schema",
              "schemaLocator",
              (record) => record.schemaLocator,
              onFilterTextChange,
              (x) => filterText.get(x),
              (record, highlightedValue) => (
                <Typography.Link href={record.schemaLocator}>
                  {highlightedValue}
                </Typography.Link>
              )
            ),
          },
        ]}
        dataSource={nodes}
      />
    </Layout>
  );
}

export default Index;
