import Layout from "../../components/Layout";
import { Table, message, Typography } from "antd";
import { useComponentsQuery } from "../../queries/components.graphql";
import { useEffect, useState } from "react";
import paths from "../../paths";
import { setMapValue } from "../../lib/freeTextFilter";
import { ComponentCategory } from "../../__generated__/__types__";
import {
  getAbbreviationColumnProps,
  getDescriptionColumnProps,
  getFilterableEnumListColumnProps,
  getNameColumnProps,
  getUuidColumnProps,
} from "../../lib/table";
import Link from "next/link";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Page() {
  const { loading, error, data } = useComponentsQuery();
  const nodes = data?.components?.nodes || [];

  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

  useEffect(() => {
    if (error) {
      message.error(error);
    }
  }, [error]);

  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        The building envelope components for which{" "}
        <Link href={paths.data}>data</Link> is available are presented here.
      </Typography.Paragraph>
      <Table
        loading={loading}
        columns={[
          {
            ...getUuidColumnProps<typeof nodes[0]>(
              onFilterTextChange,
              (x) => filterText.get(x),
              paths.component
            ),
          },
          {
            ...getNameColumnProps<typeof nodes[0]>(onFilterTextChange, (x) =>
              filterText.get(x)
            ),
          },
          {
            ...getAbbreviationColumnProps<typeof nodes[0]>(
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getDescriptionColumnProps<typeof nodes[0]>(
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getFilterableEnumListColumnProps<
              typeof nodes[0],
              ComponentCategory
            >(
              "Categories",
              "categories",
              Object.entries(ComponentCategory),
              (record) => record.categories,
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
        ]}
        dataSource={nodes}
      />
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        The{" "}
        <Typography.Link
          href={`${process.env.NEXT_PUBLIC_METABASE_URL}/graphql/`}
        >
          GraphQL endpoint
        </Typography.Link>{" "}
        provides all information about components.
      </Typography.Paragraph>
    </Layout>
  );
}

export default Page;
