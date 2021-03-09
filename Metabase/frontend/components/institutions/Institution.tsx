import { Divider, List, Typography, message, Skeleton, Table } from "antd";
import {
  useInstitutionQuery,
  Scalars,
} from "../../queries/institutions.graphql";
import CreateComponent from "../components/CreateComponent";
import CreateDatabase from "../databases/CreateDatabase";
import AddInstitutionRepresentative from "./AddInstitutionRepresentative";
import Link from "next/link";
import paths from "../../paths";

export type InstitutionProps = {
  institutionId: Scalars["Uuid"];
};

export const Institution: React.FunctionComponent<InstitutionProps> = ({
  institutionId,
}) => {
  const { loading, error, data } = useInstitutionQuery({
    variables: {
      uuid: institutionId,
    },
  });
  const institution = data?.institution;

  if (error) {
    // TODO Why can't we display error messages like this? It results in a `Objects are not valid as a React` child error.
    // message.error(error);
  }

  if (loading || !institution) {
    return <Skeleton active avatar title />;
  }

  return (
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
        dataSource={institution.manufacturedComponents.edges.map((x) => x.node)}
      />
      {institution.manufacturedComponents.canCurrentUserAddEdge ? (
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
        dataSource={institution.operatedDatabases.edges.map((x) => x.node)}
      />
      {institution.operatedDatabases.canCurrentUserAddEdge ? (
        <CreateDatabase operatorId={institution.uuid} />
      ) : (
        <></>
      )}
      <Divider />
      <Typography.Title level={2}>Managed Data Formats</Typography.Title>
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
            title: "Media Type",
            dataIndex: "mediaType",
            key: "mediaType",
          },
        ]}
        dataSource={institution.managedDataFormats.edges.map((x) => x.node)}
      />
      <Divider />
      <Typography.Title level={2}>Developed Methods</Typography.Title>
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
        ]}
        dataSource={institution.developedMethods.edges.map((x) => x.node)}
      />
      <Divider />
      <Typography.Title level={2}>Representatives</Typography.Title>
      <List
        dataSource={institution.representatives.edges.map((x) => x.node)}
        renderItem={(item) => (
          <Link href={paths.user(item?.uuid)}>{item?.email}</Link>
        )}
      />
      {institution.representatives.canCurrentUserAddEdge ? (
        <AddInstitutionRepresentative institutionId={institution.uuid} />
      ) : (
        <></>
      )}
    </>
  );
};

export default Institution;
