import Layout from "../../components/Layout";
import { Table, message } from "antd";
import { useUsersQuery } from "../../queries/users.graphql";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useUsersQuery();

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
            title: "Email",
            dataIndex: "email",
            key: "email",
          },
        ]}
        dataSource={data?.users?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
