import Layout from "../../components/Layout";
import { Table, message } from "antd";
import { useMethodsQuery } from "../../queries/methods.graphql";
import { useEffect, useState } from "react";
import paths from "../../paths";
import { setMapValue } from "../../lib/freeTextFilter";
import {
  getExternallyLinkedFilterableLocatorColumnProps,
  getFilterableEnumListColumnProps,
  getFilterableStringColumnProps,
  getUuidColumnProps,
} from "../../lib/table";
import { MethodCategory } from "../../__generated__/__types__";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useMethodsQuery();
  const nodes = data?.methods?.nodes || [];

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
              "Description",
              "description",
              (record) => record.description,
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getFilterableEnumListColumnProps<
              typeof nodes[0],
              MethodCategory
            >(
              "Categories",
              "categories",
              Object.entries(MethodCategory),
              (record) => record.categories,
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getExternallyLinkedFilterableLocatorColumnProps<typeof nodes[0]>(
              "Calculation",
              "calculationLocator",
              (record) => record.calculationLocator,
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
