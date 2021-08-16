import Layout from "../../components/Layout";
import { Table, message, Typography } from "antd";
import { useDataFormatsQuery } from "../../queries/dataFormats.graphql";
import { useEffect, useState } from "react";
import paths from "../../paths";
import { setMapValue } from "../../lib/freeTextFilter";
import {
  getExternallyLinkedFilterableLocatorColumnProps,
  getFilterableStringColumnProps,
  getUuidColumnProps,
} from "../../lib/table";
import Link from "next/link";

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
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        <Link href={paths.data}>Data</Link> is shared as resources. Each
        resource has one of the following data formats:
      </Typography.Paragraph>
      <Table
        loading={loading}
        columns={[
          {
            ...getUuidColumnProps<typeof nodes[0]>(
              onFilterTextChange,
              (x) => filterText.get(x),
              paths.dataFormat
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
            ...getExternallyLinkedFilterableLocatorColumnProps<typeof nodes[0]>(
              "Schema",
              "schemaLocator",
              (record) => record.schemaLocator,
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
        ]}
        dataSource={nodes}
      />
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        The{" "}
        <Typography.Link
          href={`${process.env.NEXT_PUBLIC_METABASE_URL}/graphql/`}
        >
          GraphQL endpoint
        </Typography.Link>{" "}
        provides all information about data formats.
      </Typography.Paragraph>
    </Layout>
  );
}

export default Index;
