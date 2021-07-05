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
            title: "Media Type",
            dataIndex: "mediaType",
            key: "mediaType",
            sorter: (a, b) => a.mediaType.length - b.mediaType.length,
            sortDirections: ["ascend", "descend", "ascend"],
            defaultSortOrder: "ascend",
          },
        ]}
        dataSource={data?.dataFormats?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
