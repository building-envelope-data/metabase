import Layout from "../../components/Layout";
import { Table, message, List, Typography } from "antd";
import { useMethodsQuery } from "../../queries/methods.graphql";
import { useEffect } from "react";
import Link from "next/link";
import paths from "../../paths";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useMethodsQuery();

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
            render: (_value, record, _index) => <Link href={paths.method(record.uuid)}>{record.uuid}</Link>
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
            title: "Categories",
            dataIndex: "categories",
            key: "categories",
            render: (_value, record, _index) => <List>{record.categories.map(category => <List.Item>{category}</List.Item>)}</List>
          },
          {
            title: "Calculation",
            dataIndex: "calculationLocator",
            key: "calculationLocator",
            sorter: (a, b) => a.calculationLocator.localeCompare(b.calculationLocator, "en"),
            sortDirections: ["ascend", "descend"],
            render: (_value, record, _index) => <Typography.Link href={record.calculationLocator}>{record.calculationLocator}</Typography.Link>
          }
        ]}
        dataSource={data?.methods?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
