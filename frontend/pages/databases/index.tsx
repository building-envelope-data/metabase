import { useQuery } from "@apollo/client/react";
import { stringifyApolloError } from "../../lib/apollo";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { Divider, Table, Typography, message } from "antd";
import { DatabasesDocument } from "../../queries/databases.generated";
import { useEffect, useState } from "react";
import { CurrentUserDocument } from "../../queries/currentUser.generated";
import { setMapValue } from "../../lib/freeTextFilter";
import PendingDatabases from "../../components/databases/PendingDatabases";
import {
  getExternallyLinkedFilterableLocatorColumnProps,
  getNameColumnProps,
  getDescriptionColumnProps,
  getInternallyLinkedFilterableStringColumnProps,
  getUuidColumnProps,
} from "../../lib/table";
import Link from "next/link";
import { UserRole } from "../../__generated__/graphql";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Page() {
  const { loading, error, data } = useQuery(DatabasesDocument);
  const nodes = data?.databases?.edges?.map((e) => e.node) || [];

  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

  const currentUser = useQuery(CurrentUserDocument)?.data?.currentUser;

  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    if (error) {
      messageApi.error(stringifyApolloError(error));
    }
  }, [error]);

  return (
    <Layout>
      {contextHolder}
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        The following databases are connected to{" "}
        <Link href={paths.home}>buildingenvelopedata.org</Link> and contain{" "}
        <Link href={paths.data}>data</Link> on{" "}
        <Link href={paths.components}>components</Link>.
      </Typography.Paragraph>
      <Table
        loading={loading}
        columns={[
          {
            ...getUuidColumnProps<(typeof nodes)[0]>(
              onFilterTextChange,
              (x) => filterText.get(x),
              paths.database,
            ),
          },
          {
            ...getNameColumnProps<(typeof nodes)[0]>(onFilterTextChange, (x) =>
              filterText.get(x),
            ),
          },
          {
            ...getDescriptionColumnProps<(typeof nodes)[0]>(
              onFilterTextChange,
              (x) => filterText.get(x),
            ),
          },
          {
            ...getExternallyLinkedFilterableLocatorColumnProps<
              (typeof nodes)[0]
            >(
              "Locator",
              "locator",
              (record) => record.locator,
              onFilterTextChange,
              (x) => filterText.get(x),
            ),
          },
          {
            ...getInternallyLinkedFilterableStringColumnProps<
              (typeof nodes)[0]
            >(
              "Operator",
              "operator",
              (record) => record.operator.node.name,
              onFilterTextChange,
              (x) => filterText.get(x),
              (x) => paths.institution(x.operator.node.uuid),
            ),
          },
        ]}
        dataSource={nodes}
      />
      {currentUser && currentUser?.roles?.includes(UserRole.Administrator) && (
        <>
          <Divider />
          <Typography.Title level={2}>Pending Databases</Typography.Title>
          <PendingDatabases />
          <Divider />
        </>
      )}
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        The{" "}
        <Typography.Link
          href={`${process.env.NEXT_PUBLIC_METABASE_URL}/graphql/`}
        >
          GraphQL endpoint
        </Typography.Link>{" "}
        provides all information about databases.
      </Typography.Paragraph>
    </Layout>
  );
}

export default Page;
