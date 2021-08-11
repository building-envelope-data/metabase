import Layout from "../../components/Layout";
import { Table, message, List } from "antd";
import { useComponentsQuery } from "../../queries/components.graphql";
import { useEffect, useState } from "react";
import paths from "../../paths";
import Link from "next/link";
import { getFreeTextFilterProps, highlight } from "../../lib/freeTextFilter";
import { ComponentCategory } from "../../__generated__/__types__";

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
            ...getFreeTextFilterProps("uuid", onFreeTextFilterTextChange),
            render: (_text, record, _index) => (
              <Link href={paths.component(record.uuid)}>
                {highlight(record.uuid, freeTextFilterText.get("uuid"))}
              </Link>
            ),
          },
          {
            title: "Name",
            dataIndex: "name",
            key: "name",
            sorter: (a, b) => a.name.localeCompare(b.name, "en"),
            sortDirections: ["ascend", "descend"],
            ...getFreeTextFilterProps("name", onFreeTextFilterTextChange),
            render: (_text, record, _index) =>
              highlight(record.name, freeTextFilterText.get("name")),
          },
          {
            title: "Abbreviation",
            dataIndex: "abbreviation",
            key: "abbreviation",
            sorter: (a, b) => a.name.localeCompare(b.name, "en"),
            sortDirections: ["ascend", "descend"],
            ...getFreeTextFilterProps(
              "abbreviation",
              onFreeTextFilterTextChange
            ),
            render: (_text, record, _index) =>
              highlight(
                record.abbreviation,
                freeTextFilterText.get("abbreviation")
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
              onFreeTextFilterTextChange
            ),
            render: (_text, record, _index) =>
              highlight(
                record.description,
                freeTextFilterText.get("description")
              ),
          },
          {
            title: "Categories",
            dataIndex: "categories",
            key: "categories",
            filters: Object.entries(ComponentCategory).map(([key, value]) => ({
              text: key,
              value: value,
            })),
            onFilter: (value, record) =>
              typeof value === "string" &&
              Object.values(ComponentCategory)
                .map((x) => x.toString())
                .includes(value)
                ? record.categories.includes(value as ComponentCategory)
                : true,
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
