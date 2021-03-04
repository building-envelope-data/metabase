import Layout from "../../components/Layout";
import { Divider, List, Typography, message, Skeleton, Table } from "antd";
import { useInstitutionQuery } from "../../queries/institutions.graphql";
import { useRouter } from "next/router";
import { useCurrentUserQuery } from "../../queries/currentUser.graphql";
import CreateComponent from "../../components/components/CreateComponent";
import CreateDatabase from "../../components/databases/CreateDatabase";
import AddInstitutionRepresentative from "../../components/institutions/AddInstitutionRepresentative";
import Link from "next/link";
import paths from "../../paths";

function Show() {
  const router = useRouter();

  const currentUserQuery = useCurrentUserQuery();
  const currentUser = currentUserQuery.data?.currentUser;

  const { uuid } = router.query;
  const { loading, error, data } = useInstitutionQuery({
    variables: {
      uuid: uuid,
    },
  });
  const institution = data?.institution;

  if (error) {
    message.error(error);
  }

  if (loading || !institution) {
    return <Layout></Layout>;
  }

  return (
    <Layout>
      {loading || !institution ? (
        <Skeleton active avatar title />
      ) : (
        <>
          <Typography.Title>
            {institution.websiteLocator ? (
              <Typography.Link href={institution.websiteLocator}>
                {`${institution.name} (${institution.abbreviation})`}
              </Typography.Link>
            ) : (
              `${institution.name} (${institution.abbreviation})`
            )}
          </Typography.Title>
          <Typography.Text>{institution.description}</Typography.Text>
        </>
      )}
      <Divider />
      <Typography.Title level={2}>Manufactured Components</Typography.Title>
      <Table
        columns={[
          {
            title: "Name",
            dataIndex: "name",
            key: "name",
          },
          {
            title: "Abbreviation",
            dataIndex: "abbreviation",
            key: "abbreviation",
          },
          {
            title: "Description",
            dataIndex: "description",
            key: "description",
          },
          {
            title: "Categories",
            dataIndex: "categories",
            key: "categories",
            render: (categories) => categories.join(", "),
          },
          {
            title: "Availability",
            dataIndex: "availability",
            key: "availability",
            render: (availability) =>
              `${availability.from} to ${availability.to}`,
          },
        ]}
        dataSource={
          institution?.manufacturedComponents?.edges?.map((x) => x.node) || []
        }
      />
      {currentUser ? (
        <CreateComponent manufacturerId={institution.uuid} />
      ) : (
        <></>
      )}
      <Divider />
      <Typography.Title level={2}>Operated Databases</Typography.Title>
      <Table
        columns={[
          {
            title: "Name",
            dataIndex: "name",
            key: "name",
          },
          {
            title: "Description",
            dataIndex: "description",
            key: "description",
          },
          {
            title: "Locator",
            dataIndex: "locator",
            key: "locator",
          },
        ]}
        dataSource={
          institution.operatedDatabases?.edges?.map((x) => x.node) || []
        }
      />
      {currentUser ? <CreateDatabase operatorId={institution.uuid} /> : <></>}
      <Divider />
      <Typography.Title level={2}>Representatives</Typography.Title>
      <List
        dataSource={
          institution.representatives?.edges?.map((x) => x.node) || []
        }
        renderItem={(item) => (
          <Link href={paths.user(item?.uuid)}>{item?.email}</Link>
        )}
      />
      {currentUser ? (
        <AddInstitutionRepresentative institutionId={institution.uuid} />
      ) : (
        <></>
      )}
    </Layout>
  );
}

export default Show;
