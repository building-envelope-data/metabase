import Layout from "../../components/Layout";
import { Table, message } from "antd";
import { useDataFormatsQuery } from "../../queries/dataFormats.graphql";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useDataFormatsQuery();

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
            title: "Media Type",
            dataIndex: "mediaType",
            key: "mediaType",
          },
        ]}
        dataSource={data?.dataFormats?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
