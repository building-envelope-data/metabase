import Layout from "../../components/Layout";
import { Table, message } from "antd";
import { useComponentsQuery } from "../../queries/components.graphql";
import { useEffect } from "react";

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
            title: "Name",
            dataIndex: "name",
            key: "name",
            sorter: (a, b) => a.name.localeCompare(b.name, "en"),
            sortDirections: ["ascend", "descend"],
            defaultSortOrder: "ascend",
          },
          {
            title: "Description",
            dataIndex: "description",
            key: "description",
          },
        ]}
        dataSource={data?.components?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
