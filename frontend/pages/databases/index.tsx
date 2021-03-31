import Layout from "../../components/Layout";
import { Table, message, Typography } from "antd";
import { useDatabasesQuery } from "../../queries/databases.graphql";
import Link from "next/link";
import paths from "../../paths";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useDatabasesQuery();

  if (error) {
    message.error(error);
  }

  return (
    <Layout>
      <Table
        loading={loading}
        columns={[
          {
            title: "Name",
            dataIndex: "name",
            key: "name",
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
            render: (locator) => (
              <Typography.Link href={locator}>{locator}</Typography.Link>
            ),
          },
          {
            title: "Operator",
            dataIndex: "operator",
            key: "operator",
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
