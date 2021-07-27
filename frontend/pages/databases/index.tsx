import Layout from "../../components/Layout";
import Link from "next/link";
import paths from "../../paths";
import { Table, message, Typography } from "antd";
import { useDatabasesQuery } from "../../queries/databases.graphql";
import { useEffect } from "react";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useDatabasesQuery();

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
            render: (_value, record, _index) => <Link href={paths.database(record.uuid)}>{record.uuid}</Link>
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
            title: "Locator",
            dataIndex: "locator",
            key: "locator",
            sorter: (a, b) => a.locator.localeCompare(b.locator, "en"),
            sortDirections: ["ascend", "descend"],
            render: (_value, record, _index) => (
              <Typography.Link href={record.locator}>{record.locator}</Typography.Link>
            ),
          },
          {
            title: "Operator",
            dataIndex: "operator",
            key: "operator",
            sorter: (a, b) =>
              a.operator.node.name.localeCompare(b.operator.node.name, "en"),
            sortDirections: ["ascend", "descend"],
            render: (_value, record, _index) => (
              <Link href={paths.institution(record.operator.node.uuid)}>
                {record.operator.node.name}
              </Link>
            ),
          },
        ]}
        dataSource={data?.databases?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
