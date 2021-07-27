import Layout from "../../components/Layout";
import { Typography, Table, message } from "antd";
import { useDataFormatsQuery } from "../../queries/dataFormats.graphql";
import { useEffect } from "react";
import paths from "../../paths";
import Link from "next/link";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useDataFormatsQuery();

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
            title: "UUID",
            dataIndex: "uuid",
            key: "uuid",
            sorter: (a, b) => a.uuid.localeCompare(b.uuid, "en"),
            sortDirections: ["ascend", "descend"],
            render: (_value, record, _index) => <Link href={paths.dataFormat(record.uuid)}>{record.uuid}</Link>
          },
          {
            title: "Name",
            dataIndex: "name",
            key: "name",
            sorter: (a, b) => a.name.localeCompare(b.name, "en"),
            sortDirections: ["ascend", "descend"],
          },
          {
            title: "Description",
            dataIndex: "description",
            key: "description",
            sorter: (a, b) => a.description.localeCompare(b.description, "en"),
            sortDirections: ["ascend", "descend"],
          },
          {
            title: "Media Type",
            dataIndex: "mediaType",
            key: "mediaType",
            sorter: (a, b) => a.mediaType.localeCompare(b.mediaType, "en"),
            sortDirections: ["ascend", "descend"],
          },
          {
            title: "Extension",
            dataIndex: "extension",
            key: "extension",
            sorter: (a, b) => a.extension && b.extension ? a.extension.localeCompare(b.extension, "en") : 0,
            sortDirections: ["ascend", "descend"],
          },
          {
            title: "Schema",
            dataIndex: "schemaLocator",
            key: "schemaLocator",
            sorter: (a, b) => a.schemaLocator && b.schemaLocator ? a.schemaLocator.localeCompare(b.schemaLocator, "en") : 0,
            sortDirections: ["ascend", "descend"],
            render: (_value, record, _index) => <Typography.Link href={record.schemaLocator}>{record.schemaLocator}</Typography.Link>
          },
        ]}
        dataSource={data?.dataFormats?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
