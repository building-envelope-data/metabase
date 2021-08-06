import Layout from "../../components/Layout";
import Link from "next/link";
import paths from "../../paths";
import { Table, message, Typography, Divider } from "antd";
import { useInstitutionsQuery } from "../../queries/institutions.graphql";
import { useEffect } from "react";
import { useCurrentUserQuery } from "../../queries/currentUser.graphql";
import PendingInstitutions from "../../components/institutions/PendingInstitutions";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

// Taken from https://stackoverflow.com/questions/43118692/typescript-filter-out-nulls-from-an-array/46700791#46700791
function notEmpty<TValue>(value: TValue | null | undefined): value is TValue {
  return value !== null && value !== undefined;
}

function Index() {
  const { loading, error, data } = useInstitutionsQuery();
  const currentUser = useCurrentUserQuery()?.data?.currentUser;

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
            title: "UUID",
            dataIndex: "uuid",
            key: "uuid",
            sorter: (a, b) => a.uuid.localeCompare(b.uuid, "en"),
            sortDirections: ["ascend", "descend"],
            render: (_value, record, _index) => (
              <Link href={paths.institution(record.uuid)}>{record.uuid}</Link>
            ),
          },
          {
            title: "Name",
            dataIndex: "name",
            key: "name",
            sorter: (a, b) => a.name.localeCompare(b.name, "en"),
            sortDirections: ["ascend", "descend"],
          },
          {
            title: "Abbreviation",
            dataIndex: "abbreviation",
            key: "abbreviation",
            sorter: (a, b) =>
              a.abbreviation && b.abbreviation
                ? a.abbreviation.localeCompare(b.abbreviation, "en")
                : 0,
            sortDirections: ["ascend", "descend"],
          },
          {
            title: "Description",
            dataIndex: "description",
            key: "description",
            sorter: (a, b) => a.description.localeCompare(b.description, "en"),
            sortDirections: ["ascend", "descend"],
          },
          {
            title: "Website",
            dataIndex: "websiteLocator",
            key: "websiteLocator",
            sorter: (a, b) =>
              a.websiteLocator.localeCompare(b.websiteLocator, "en"),
            sortDirections: ["ascend", "descend"],
            render: (_value, record, _index) => (
              <Typography.Link href={record.websiteLocator}>
                {record.websiteLocator}
              </Typography.Link>
            ),
          },
        ]}
        dataSource={data?.institutions?.nodes?.filter(notEmpty) || []}
      />
      {/* TODO Make role name `Verifier` a constant. */}
      {currentUser && currentUser?.roles?.includes("Verifier") && (
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
