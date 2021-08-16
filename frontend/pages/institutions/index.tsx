import Layout from "../../components/Layout";
import Link from "next/link";
import paths from "../../paths";
import { Table, message, Typography, Divider } from "antd";
import { useInstitutionsQuery } from "../../queries/institutions.graphql";
import { useEffect, useState } from "react";
import { useCurrentUserQuery } from "../../queries/currentUser.graphql";
import PendingInstitutions from "../../components/institutions/PendingInstitutions";
import { UserRole } from "../../__generated__/__types__";
import { setMapValue } from "../../lib/freeTextFilter";
import {
  getExternallyLinkedFilterableLocatorColumnProps,
  getFilterableStringColumnProps,
  getUuidColumnProps,
} from "../../lib/table";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

// Taken from https://stackoverflow.com/questions/43118692/typescript-filter-out-nulls-from-an-array/46700791#46700791
function notEmpty<TValue>(value: TValue | null | undefined): value is TValue {
  return value !== null && value !== undefined;
}

function Index() {
  const { loading, error, data } = useInstitutionsQuery();
  const nodes = data?.institutions?.nodes?.filter(notEmpty) || [];

  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

  const currentUser = useCurrentUserQuery()?.data?.currentUser;

  useEffect(() => {
    if (error) {
      message.error(error);
    }
  }, [error]);

  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        Institutions can manufacture{" "}
        <Link href={paths.components}>components</Link>, operate{" "}
        <Link href={paths.databases}>databases</Link> and create{" "}
        <Link href={paths.dataFormats}>data formats</Link> and{" "}
        <Link href={paths.methods}>methods</Link>.
      </Typography.Paragraph>
      <Table
        loading={loading}
        columns={[
          {
            ...getUuidColumnProps<typeof nodes[0]>(
              onFilterTextChange,
              (x) => filterText.get(x),
              paths.institution
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
              "Abbreviation",
              "abbreviation",
              (record) => record.description,
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
              "Website",
              "websiteLocator",
              (record) => record.websiteLocator,
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
        provides all information about institutions.
      </Typography.Paragraph>
      {currentUser && currentUser?.roles?.includes(UserRole.Verifier) && (
        <>
          <Divider />
          <Typography.Title level={2}>Pending Institutions</Typography.Title>
          <PendingInstitutions />
        </>
      )}
      {currentUser && (
        <Link href={paths.institutionCreate}>Create Institution</Link>
      )}
    </Layout>
  );
}

export default Index;
