import Layout from "../../components/Layout";
import { Table, message } from "antd";
import { useComponentsQuery } from "../../queries/components.graphql";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useComponentsQuery();

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
            sorter: (a, b) => a.name.length - b.name.length,
            sortDirections: ["ascend", "descend", "ascend"],
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
