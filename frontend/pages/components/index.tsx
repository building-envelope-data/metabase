import Layout from "../../components/Layout";
import { Table, message, List } from "antd";
import { useComponentsQuery } from "../../queries/components.graphql";
import { useEffect } from "react";
import paths from "../../paths";
import Link from "next/link";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useComponentsQuery();

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
            render: (_value, record, _index) => <Link href={paths.component(record.uuid)}>{record.uuid}</Link>
          },
          {
            title: "Name",
            dataIndex: "name",
            key: "name",
            sorter: (a, b) => a.name.localeCompare(b.name, "en"),
            sortDirections: ["ascend", "descend"],
          },
          {
            title: "Abbreviation",
            dataIndex: "abbreviation",
            key: "abbreviation",
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
        ]}
        dataSource={data?.components?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
