import Layout from "../../components/Layout";
import { Table, message } from "antd";
import { useUsersQuery } from "../../queries/users.graphql";
import paths from "../../paths";
import { useEffect, useState } from "react";
import { setMapValue } from "../../lib/freeTextFilter";
import {
  getFilterableStringColumnProps,
  getUuidColumnProps,
} from "../../lib/table";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useUsersQuery();
  const nodes = data?.users?.nodes || [];

  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

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
            ...getUuidColumnProps<typeof nodes[0]>(
              onFilterTextChange,
              (x) => filterText.get(x),
              paths.method
            ),
          },
          {
            ...getFilterableStringColumnProps<typeof nodes[0]>(
              "Name",
              "name",
              (record) => record.name,
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getFilterableStringColumnProps<typeof nodes[0]>(
              "Email",
              "email",
              (record) => record.email,
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
        ]}
        dataSource={nodes}
      />
    </Layout>
  );
}

export default Index;
