import Layout from "../../components/Layout";
import { Table, message, Typography } from "antd";
import { useDatabasesQuery } from "../../queries/databases.graphql";
import Link from "next/link";
import paths from "../../paths";
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
            title: "Name",
            dataIndex: "name",
            key: "name",
            sorter: (a, b) => a.name.length - b.name.length,
            sortDirections: ["ascend", "descend", "ascend"],
            defaultSortOrder: "ascend",
          },
          {
            title: "Description",
            dataIndex: "description",
            key: "description",
          },
          {
            title: "Locator",
            dataIndex: "locator",
            key: "locator",
            sorter: (a, b) => a.locator.length - b.locator.length,
            sortDirections: ["ascend", "descend", "ascend"],
            defaultSortOrder: "ascend",
            render: (locator) => (
              <Typography.Link href={locator}>{locator}</Typography.Link>
            ),
          },
          {
            title: "Operator",
            dataIndex: "operator",
            key: "operator",
            sorter: (a, b) =>
              a.operator.node.name.length - b.operator.node.name.length,
            sortDirections: ["ascend", "descend", "ascend"],
            defaultSortOrder: "ascend",
            render: (operator) => (
              <Link href={paths.institution(operator.node.uuid)}>
                {operator.node.name}
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
