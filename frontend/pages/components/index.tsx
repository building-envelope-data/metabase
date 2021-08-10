import Layout from "../../components/Layout";
import { Table, message, List } from "antd";
import { useComponentsQuery } from "../../queries/components.graphql";
import { useEffect, useState } from "react";
import paths from "../../paths";
import Link from "next/link";
import { getFreeTextFilterProps } from "../../lib/freeTextFilter";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useComponentsQuery();
  const [freeTextFilterText, setFreeTextFilterText] = useState(
    () => new Map<string | string[], string>()
  );

  useEffect(() => {
    if (error) {
      message.error(error);
    }
  }, [error]);

  const onFreeTextFilterTextChange = (
    dataIndex: string | string[],
    newFilterText: string
  ) => {
    if (freeTextFilterText.get(dataIndex) !== newFilterText) {
      const copy = new Map(freeTextFilterText);
      copy.set(dataIndex, newFilterText);
      setFreeTextFilterText(copy);
    }
  };

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
            // TODO Pass optional render method to free text filter props that takes function that outputs highlighted text
            render: (_value, record, _index) => (
              <Link href={paths.component(record.uuid)}>{record.uuid}</Link>
            ),
          },
          {
            title: "Name",
            dataIndex: "name",
            key: "name",
            sorter: (a, b) => a.name.localeCompare(b.name, "en"),
            sortDirections: ["ascend", "descend"],
            ...getFreeTextFilterProps(
              "name",
              freeTextFilterText.get("name") || "",
              onFreeTextFilterTextChange
            ),
          },
          {
            title: "Abbreviation",
            dataIndex: "abbreviation",
            key: "abbreviation",
            sorter: (a, b) => a.name.localeCompare(b.name, "en"),
            sortDirections: ["ascend", "descend"],
            ...getFreeTextFilterProps(
              "abbreviation",
              freeTextFilterText.get("abbreviation") || "",
              onFreeTextFilterTextChange
            ),
          },
          {
            title: "Description",
            dataIndex: "description",
            key: "description",
            sorter: (a, b) => a.description.localeCompare(b.description, "en"),
            sortDirections: ["ascend", "descend"],
            ...getFreeTextFilterProps(
              "description",
              freeTextFilterText.get("description") || "",
              onFreeTextFilterTextChange
            ),
          },
          {
            title: "Categories",
            dataIndex: "categories",
            key: "categories",
            render: (_value, record, _index) => (
              <List>
                {record.categories.map((category) => (
                  <List.Item key={category}>{category}</List.Item>
                ))}
              </List>
            ),
          },
        ]}
        dataSource={data?.components?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
