import Layout from "../../components/Layout";
import paths from "../../paths";
import { Table, message } from "antd";
import { useDatabasesQuery } from "../../queries/databases.graphql";
import { useEffect, useState } from "react";
import { setMapValue } from "../../lib/freeTextFilter";
import {
  getExternallyLinkedFilterableLocatorColumnProps,
  getFilterableStringColumnProps,
  getInternallyLinkedFilterableStringColumnProps,
  getUuidColumnProps,
} from "../../lib/table";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useDatabasesQuery();
  const nodes = data?.databases?.nodes || [];

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
              paths.database
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
              "Description",
              "description",
              (record) => record.description,
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getExternallyLinkedFilterableLocatorColumnProps<typeof nodes[0]>(
              "Locator",
              "locator",
              (record) => record.locator,
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getInternallyLinkedFilterableStringColumnProps<typeof nodes[0]>(
              "Operator",
              "operator",
              (record) => record.operator.node.name,
              onFilterTextChange,
              (x) => filterText.get(x),
              (x) => paths.institution(x.operator.node.uuid)
            ),
          },
        ]}
        dataSource={data?.databases?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
