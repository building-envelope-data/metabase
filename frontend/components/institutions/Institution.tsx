import {
  Divider,
  List,
  Typography,
  message,
  Skeleton,
  Button,
  Result,
  Descriptions,
  Tag,
} from "antd";
import { PageHeader } from "@ant-design/pro-layout"
import {
  InstitutionDocument,
  useInstitutionQuery,
} from "../../queries/institutions.graphql";
import { Scalars } from "../../__generated__/__types__";
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
import { DataFormatTable } from "../dataFormats/DataFormatTable";
import { ComponentTable } from "../components/ComponentTable";
import DatabaseTable from "../databases/DatabaseTable";
import MethodTable from "../methods/MethodTable";
import { messageApolloError } from "../../lib/apollo";

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
      messageApolloError(error);
    }
  }, [error]);

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!institution) {
    return (
      <Result
        status="500"
        title="500"
        subTitle="Sorry, something went wrong."
      />
    );
  }

  return (
    <PageHeader
      title={[
        institution.name,
        institution.abbreviation == null
          ? null
          : `(${institution.abbreviation})`,
      ]
        .filter((x) => x != null)
        .join(" ")}
      subTitle={institution.description}
      tags={[
        <Tag key={institution.state} color="magenta">
          {institution.state}
        </Tag>,
      ]}
      backIcon={false}
    >
      <Descriptions size="small" column={1}>
        <Descriptions.Item label="UUID">{institution.uuid}</Descriptions.Item>
        {institution.websiteLocator && (
          <Descriptions.Item label="Website">
            <Typography.Link href={institution.websiteLocator}>
              {institution.websiteLocator}
            </Typography.Link>
          </Descriptions.Item>
        )}
      </Descriptions>
      <Divider />
      <Typography.Title level={2}>Manufactured Components</Typography.Title>
      <ComponentTable
        loading={loading}
        components={institution.manufacturedComponents.edges.map((x) => x.node)}
      />
      {institution.pendingManufacturedComponents.canCurrentUserConfirmEdge &&
        institution.pendingManufacturedComponents.edges.length >= 1 && (
          <List
            size="small"
            header="Pending"
            dataSource={institution.pendingManufacturedComponents.edges}
            renderItem={(item) => (
              <List.Item key={item.node.uuid}>
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
      <DatabaseTable
        loading={loading}
        databases={institution.operatedDatabases.edges.map((x) => x.node)}
      />
      {institution.operatedDatabases.canCurrentUserAddEdge && (
        <CreateDatabase operatorId={institution.uuid} />
      )}
      <Divider />
      <Typography.Title level={2}>Managed Data Formats</Typography.Title>
      <DataFormatTable
        loading={loading}
        dataFormats={institution.managedDataFormats.edges.map((x) => x.node)}
      />
      {institution.managedDataFormats.canCurrentUserAddEdge && (
        <CreateDataFormat managerId={institution.uuid} />
      )}
      <Divider />
      <Typography.Title level={2}>Managed Methods</Typography.Title>
      <MethodTable
        loading={loading}
        methods={institution.managedMethods.edges.map((x) => x.node)}
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
          <List.Item key={item.node.uuid}>
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
              <List.Item key={item.node.uuid}>
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
          <List.Item key={item.uuid}>
            <Link href={paths.institution(item.uuid)}>{item.name}</Link>
          </List.Item>
        )}
      />
      {institution.managedInstitutions.canCurrentUserAddEdge && (
        <CreateInstitution managerId={institution.uuid} />
      )}
      <Divider />
      <Typography.Title level={2}>Representatives</Typography.Title>
      <List
        size="small"
        dataSource={institution.representatives.edges}
        renderItem={(item) => (
          <List.Item key={item.node.uuid}>
            <Link href={paths.user(item.node.uuid)}>{item.node.name}</Link>
            <Typography.Text>{item.role}</Typography.Text>
          </List.Item>
        )}
      />
      {institution.representatives.canCurrentUserAddEdge &&
        institution.pendingRepresentatives.edges.length >= 1 && (
          <List
            size="small"
            header="Pending"
            dataSource={institution.pendingRepresentatives.edges}
            renderItem={(item) => (
              <List.Item key={item.node.uuid}>
                <Link href={paths.user(item.node.uuid)}>{item.node.name}</Link>
                <Typography.Text>{item.role}</Typography.Text>
              </List.Item>
            )}
          />
        )}
      {institution.representatives.canCurrentUserAddEdge && (
        <AddInstitutionRepresentative institutionId={institution.uuid} />
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
    </PageHeader>
  );
}
