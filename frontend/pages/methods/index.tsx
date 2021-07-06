import Layout from "../../components/Layout";
import { Table, message } from "antd";
import { useMethodsQuery } from "../../queries/methods.graphql";
import { useEffect } from "react";

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
          },
        ]}
        dataSource={data?.methods?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
