import {
  Descriptions,
  Divider,
  List,
  Typography,
  message,
  Skeleton,
  Table,
  Button,
} from "antd";
import {
  InstitutionDocument,
  useInstitutionQuery,
} from "../../queries/institutions.graphql";
import {
  Scalars,
  Maybe,
  Numeration,
  Publication,
  Standard,
} from "../../__generated__/__types__";
import CreateComponent from "../components/CreateComponent";
import CreateMethod from "../methods/CreateMethod";
import CreateDataFormat from "../dataFormats/CreateDataFormat";
import CreateInstitution from "../institutions/CreateInstitution";
import CreateDatabase from "../databases/CreateDatabase";
import AddInstitutionRepresentative from "./AddInstitutionRepresentative";
import Link from "next/link";
import paths from "../../paths";
import { useEffect, useState } from "react";
import { useConfirmInstitutionMethodDeveloperMutation } from "../../queries/institutionMethodDevelopers.graphql";
import { MethodDocument } from "../../queries/methods.graphql";
import { useConfirmComponentManufacturerMutation } from "../../queries/componentManufacturers.graphql";
import { ComponentDocument } from "../../queries/components.graphql";

const renderReference = (
  reference:
    | Maybe<
        | ({ __typename?: "Publication" | undefined } & Pick<
            Publication,
            | "title"
            | "arXiv"
            | "authors"
            | "doi"
            | "urn"
            | "webAddress"
            | "abstract"
            | "section"
          >)
        | ({ __typename?: "Standard" | undefined } & Pick<
            Standard,
            | "title"
            | "abstract"
            | "section"
            | "locator"
            | "standardizers"
            | "year"
          > & {
              numeration: { __typename?: "Numeration" | undefined } & Pick<
                Numeration,
                "mainNumber" | "prefix" | "suffix"
              >;
            })
      >
    | undefined
) => (
  <Descriptions column={1}>
    {reference && (
      <>
        <Descriptions.Item label="Abstract">
          {reference?.abstract}
        </Descriptions.Item>
        <Descriptions.Item label="Section">
          {reference?.section}
        </Descriptions.Item>
        <Descriptions.Item label="Title">{reference?.title}</Descriptions.Item>
      </>
    )}
    {reference?.__typename === "Standard" && (
      <>
        <Descriptions.Item label="Locator">
          <Typography.Link href={reference.locator}>
            {reference.locator}
          </Typography.Link>
        </Descriptions.Item>
        <Descriptions.Item label="Numeration">{`${reference.numeration.prefix} ${reference.numeration.mainNumber} ${reference.numeration.suffix}`}</Descriptions.Item>
        <Descriptions.Item label="Standardizers">
          {reference.standardizers.join(", ")}
        </Descriptions.Item>
        <Descriptions.Item label="Year">{reference.year}</Descriptions.Item>
      </>
    )}
    {reference?.__typename === "Publication" && (
      <>
        <Descriptions.Item label="arXiv">{reference.arXiv}</Descriptions.Item>
        <Descriptions.Item label="Authors">
          {reference.authors?.join(", ")}
        </Descriptions.Item>
        <Descriptions.Item label="DOI">{reference.doi}</Descriptions.Item>
        <Descriptions.Item label="URN">{reference.urn}</Descriptions.Item>
        <Descriptions.Item label="Web Address">
          {reference.webAddress}
        </Descriptions.Item>
      </>
    )}
  </Descriptions>
);

export type InstitutionProps = {
  institutionId: Scalars["Uuid"];
};

export default function Institution({ institutionId }: InstitutionProps) {
  const { loading, error, data } = useInstitutionQuery({
    variables: {
      uuid: institutionId,
    },
  });
  const institution = data?.institution;

  const [confirmInstitutionMethodDeveloperMutation] =
    useConfirmInstitutionMethodDeveloperMutation();
  const [
    confirmingInstitutionMethodDeveloper,
    setConfirmingInstitutionMethodDeveloper,
  ] = useState(false);

  const confirmInstitutionMethodDeveloper = async (
    methodId: Scalars["Uuid"]
  ) => {
    try {
      setConfirmingInstitutionMethodDeveloper(true);
      const { errors, data } = await confirmInstitutionMethodDeveloperMutation({
        variables: {
          methodId: methodId,
          institutionId: institutionId,
        },
        refetchQueries: [
          {
            query: MethodDocument,
            variables: {
              uuid: methodId,
            },
          },
          {
            query: InstitutionDocument,
            variables: {
              uuid: institutionId,
            },
          },
        ],
      });
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.confirmInstitutionMethodDeveloper?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.confirmInstitutionMethodDeveloper?.errors
            .map((error) => error.message)
            .join(" ")
        );
      }
    } finally {
      setConfirmingInstitutionMethodDeveloper(false);
    }
  };

  const [confirmComponentManufacturerMutation] =
    useConfirmComponentManufacturerMutation();
  const [confirmingComponentManufacturer, setConfirmingComponentManufacturer] =
    useState(false);

  const confirmComponentManufacturer = async (componentId: Scalars["Uuid"]) => {
    try {
      setConfirmingComponentManufacturer(true);
      const { errors, data } = await confirmComponentManufacturerMutation({
        variables: {
          componentId: componentId,
          institutionId: institutionId,
        },
        refetchQueries: [
          {
            query: ComponentDocument,
            variables: {
              uuid: componentId,
            },
          },
          {
            query: InstitutionDocument,
            variables: {
              uuid: institutionId,
            },
          },
        ],
      });
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.confirmComponentManufacturer?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.confirmComponentManufacturer?.errors
            .map((error) => error.message)
            .join(" ")
        );
      }
    } finally {
      setConfirmingComponentManufacturer(false);
    }
  };

  useEffect(() => {
    if (error) {
      message.error(error);
    }
  }, [error]);

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
            title: "UUID",
            dataIndex: "uuid",
            key: "uuid",
            render: (_value, record, _index) => (
              <Link href={paths.component(record.uuid)}>{record.uuid}</Link>
            ),
          },
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
            render: (_value, record, _index) => record.categories.join(", "),
          },
          {
            title: "Availability",
            dataIndex: "availability",
            key: "availability",
            render: (_value, record, _index) =>
              `${record.availability?.from} to ${record.availability?.to}`,
          },
        ]}
        dataSource={institution.manufacturedComponents.edges.map((x) => x.node)}
      />
      {institution.pendingManufacturedComponents.canCurrentUserConfirmEdge &&
        institution.pendingManufacturedComponents.edges.length >= 1 && (
          <List
            size="small"
            header="Pending"
            dataSource={institution.pendingManufacturedComponents.edges}
            renderItem={(item) => (
              <List.Item>
                <Link href={paths.component(item.node.uuid)}>
                  {item.node.name}
                </Link>
                <Button
                  onClick={() => confirmComponentManufacturer(item.node.uuid)}
                  loading={confirmingComponentManufacturer}
                >
                  Confirm
                </Button>
              </List.Item>
            )}
          />
        )}
      {institution.manufacturedComponents.canCurrentUserAddEdge && (
        <CreateComponent manufacturerId={institution.uuid} />
      )}
      <Divider />
      <Typography.Title level={2}>Operated Databases</Typography.Title>
      <Table
        columns={[
          {
            title: "UUID",
            dataIndex: "uuid",
            key: "uuid",
            render: (_value, record, _index) => (
              <Link href={paths.database(record.uuid)}>{record.uuid}</Link>
            ),
          },
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
            title: "UUID",
            dataIndex: "uuid",
            key: "uuid",
            render: (_value, record, _index) => (
              <Link href={paths.dataFormat(record.uuid)}>{record.uuid}</Link>
            ),
          },
          {
            title: "Name",
            dataIndex: "name",
            key: "name",
          },
          {
            title: "Extension",
            dataIndex: "extension",
            key: "extension",
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
          {
            title: "Schema",
            dataIndex: "schemaLocator",
            key: "schemaLocator",
            render: (_text, record, _index) => (
              <Typography.Link href={record.schemaLocator}>
                {record.schemaLocator}
              </Typography.Link>
            ),
          },
          {
            title: "Reference",
            dataIndex: "reference",
            key: "reference",
            render: (_text, record, _index) =>
              renderReference(record.reference),
          },
        ]}
        dataSource={institution.managedDataFormats.edges.map((x) => x.node)}
      />
      {institution.managedDataFormats.canCurrentUserAddEdge ? (
        <CreateDataFormat managerId={institution.uuid} />
      ) : (
        <></>
      )}
      <Divider />
      <Typography.Title level={2}>Managed Methods</Typography.Title>
      <Table
        columns={[
          {
            title: "UUID",
            dataIndex: "uuid",
            key: "uuid",
            render: (_value, record, _index) => (
              <Link href={paths.method(record.uuid)}>{record.uuid}</Link>
            ),
          },
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
            title: "Validity",
            dataIndex: "validity",
            key: "validity",
            render: (_text, record, _index) =>
              `from ${record.validity?.from} to ${record.validity?.to}`,
          },
          {
            title: "Availability",
            dataIndex: "availability",
            key: "availability",
            render: (_text, record, _index) =>
              `from ${record.availability?.from} to ${record.availability?.to}`,
          },
          {
            title: "Reference",
            dataIndex: "reference",
            key: "reference",
            render: (_text, record, _index) =>
              renderReference(record.reference),
          },
          {
            title: "Calculation Locator",
            dataIndex: "calculationLocator",
            key: "calculationLocator",
            render: (_text, record, _index) => (
              <Typography.Link href={record.calculationLocator}>
                {record.calculationLocator}
              </Typography.Link>
            ),
          },
          {
            title: "Categories",
            dataIndex: "categories",
            key: "categories",
            render: (_text, record, _index) => record.categories.join(", "),
          },
        ]}
        dataSource={institution.managedMethods.edges.map((x) => x.node)}
      />
      {institution.managedMethods.canCurrentUserAddEdge && (
        <CreateMethod managerId={institution.uuid} />
      )}
      <Divider />
      <Typography.Title level={2}>Developed Methods</Typography.Title>
      <List
        size="small"
        dataSource={institution.developedMethods.edges}
        renderItem={(item) => (
          <List.Item>
            <Link href={paths.method(item.node.uuid)}>{item.node.name}</Link>
          </List.Item>
        )}
      />
      {institution.pendingDevelopedMethods.canCurrentUserConfirmEdge &&
        institution.pendingDevelopedMethods.edges.length >= 1 && (
          <List
            size="small"
            header="Pending"
            dataSource={institution.pendingDevelopedMethods.edges}
            renderItem={(item) => (
              <List.Item>
                <Link href={paths.method(item.node.uuid)}>
                  {item.node.name}
                </Link>
                <Button
                  onClick={() =>
                    confirmInstitutionMethodDeveloper(item.node.uuid)
                  }
                  loading={confirmingInstitutionMethodDeveloper}
                >
                  Confirm
                </Button>
              </List.Item>
            )}
          />
        )}
      <Divider />
      <Typography.Title level={2}>Managed Institutions</Typography.Title>
      <List
        size="small"
        dataSource={institution.managedInstitutions.edges.map((x) => x.node)}
        renderItem={(item) => (
          <List.Item>
            <Link href={paths.institution(item?.uuid)}>{item?.name}</Link>
          </List.Item>
        )}
      />
      {institution.managedInstitutions.canCurrentUserAddEdge ? (
        <CreateInstitution managerId={institution.uuid} />
      ) : (
        <></>
      )}
      <Divider />
      <Typography.Title level={2}>Representatives</Typography.Title>
      <List
        size="small"
        dataSource={institution.representatives.edges}
        renderItem={(item) => (
          <List.Item>
            <Link href={paths.user(item.node.uuid)}>{item.node.name}</Link>
            <Typography.Text>{item.role}</Typography.Text>
          </List.Item>
        )}
      />
      {institution.representatives.canCurrentUserAddEdge ? (
        <AddInstitutionRepresentative institutionId={institution.uuid} />
      ) : (
        <></>
      )}
      {institution.manager?.node && (
        <>
          <Divider />
          <Typography.Title level={2}>Managing Institution</Typography.Title>
          <Link href={paths.institution(institution.manager?.node?.uuid)}>
            {institution.manager?.node?.name}
          </Link>
        </>
      )}
    </>
  );
}
