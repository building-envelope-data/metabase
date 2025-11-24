import { useMutation } from "@apollo/client/react";
import { useQuery } from "@apollo/client/react";
import {
  Divider,
  List,
  Typography,
  Skeleton,
  Button,
  Result,
  Descriptions,
  Tag,
  message,
} from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";
import CreateComponent from "../components/CreateComponent";
import CreateMethod from "../methods/CreateMethod";
import CreateDataFormat from "../dataFormats/CreateDataFormat";
import CreateInstitution from "../institutions/CreateInstitution";
import CreateDatabase from "../databases/CreateDatabase";
import AddInstitutionRepresentative from "./AddInstitutionRepresentative";
import Link from "next/link";
import paths from "../../paths";
import { ReactNode, useEffect, useState } from "react";
import { ConfirmInstitutionMethodDeveloperDocument } from "../../queries/institutionMethodDevelopers.generated";
import { MethodDocument } from "../../queries/methods.generated";
import { ConfirmComponentManufacturerDocument } from "../../queries/componentManufacturers.generated";
import { ComponentDocument } from "../../queries/components.generated";
import { DataFormatTable } from "../dataFormats/DataFormatTable";
import { ComponentTable } from "../components/ComponentTable";
import DatabaseTable from "../databases/DatabaseTable";
import MethodTable from "../methods/MethodTable";
import { stringifyApolloError } from "../../lib/apollo";
import UpdateInstitution from "./UpdateInstitution";
import DeleteInstitution from "./DeleteInstitution";
import SwitchInstitutionOperatingState from "./SwitchInstitutionOperatingState";
import ApplicationTable from "../openIdConnect/applications/ApplicationTable";
import CreateApplication from "../openIdConnect/applications/CreateApplication";
import GnuPgKeyFingerprintTable from "../gnuPgKeyFingerprints/GnuPgKeyFingerprintTable";
import AddGnuPgKeyFingerprint from "../gnuPgKeyFingerprints/AddGnuPgKeyFingerprint";
import { GnuPgKeyFingerprintPartialFragment } from "../../queries/gnuPgKeyFingerprints.generated";
import { ApplicationPartialFragment } from "../../queries/openIdConnect.generated";

export type InstitutionProps = {
  institutionId: Scalars["Uuid"]["input"];
};

export default function Institution({ institutionId }: InstitutionProps) {
  const { loading, error, data } = useQuery(InstitutionDocument, {
    variables: {
      uuid: institutionId,
    },
  });
  const institution = data?.institution;

  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    if (error) {
      messageApi.error(stringifyApolloError(error));
    }
  }, [error]);

  const [confirmInstitutionMethodDeveloperMutation] = useMutation(
    ConfirmInstitutionMethodDeveloperDocument,
  );
  const [
    confirmingInstitutionMethodDeveloper,
    setConfirmingInstitutionMethodDeveloper,
  ] = useState(false);

  const confirmInstitutionMethodDeveloper = async (
    methodId: Scalars["Uuid"]["input"],
  ) => {
    try {
      setConfirmingInstitutionMethodDeveloper(true);
      const { error, data } = await confirmInstitutionMethodDeveloperMutation({
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
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.confirmInstitutionMethodDeveloper?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.confirmInstitutionMethodDeveloper?.errors
            .map((error) => error.message)
            .join(" "),
        );
      }
    } finally {
      setConfirmingInstitutionMethodDeveloper(false);
    }
  };

  const [confirmComponentManufacturerMutation] = useMutation(
    ConfirmComponentManufacturerDocument,
  );
  const [confirmingComponentManufacturer, setConfirmingComponentManufacturer] =
    useState(false);

  const confirmComponentManufacturer = async (
    componentId: Scalars["Uuid"]["input"],
  ) => {
    try {
      setConfirmingComponentManufacturer(true);
      const { error, data } = await confirmComponentManufacturerMutation({
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
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.confirmComponentManufacturer?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.confirmComponentManufacturer?.errors
            .map((error) => error.message)
            .join(" "),
        );
      }
    } finally {
      setConfirmingComponentManufacturer(false);
    }
  };

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
    <>
      {contextHolder}
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
          <Tag key={institution.operatingState} color="blue">
            {institution.operatingState}
          </Tag>,
        ]}
        extra={([] as ReactNode[])
          .concat(
            institution.isAuthorizedToUpdateNode
              ? [
                <UpdateInstitution
                  key="updateInstitution"
                  institutionId={institution.uuid}
                  name={institution.name}
                  abbreviation={institution.abbreviation}
                  description={institution.description}
                  contact={institution.contact}
                />,
              ]
              : [],
          )
          .concat(
            institution.isAuthorizedToDeleteNode
              ? [
                <DeleteInstitution
                  key="deleteInstitution"
                  institutionId={institution.uuid}
                />,
              ]
              : [],
          )
          .concat(
            institution.isAuthorizedToSwitchOperatingStateOfNode
              ? [
                <SwitchInstitutionOperatingState
                  key="switchInstitutionOperatingState"
                  institutionId={institution.uuid}
                />,
              ]
              : [],
          )}
        backIcon={false}
      >
        <Descriptions size="small" column={1}>
          <Descriptions.Item label="UUID">{institution.uuid}</Descriptions.Item>
          {institution.contact?.phoneNumber && (
            <Descriptions.Item label="Phone">
              <Typography.Link href={institution.contact.phoneNumber}>
                {institution.contact.phoneNumber}
              </Typography.Link>
            </Descriptions.Item>
          )}
          {institution.contact?.postalAddress && (
            <Descriptions.Item label="Postal Address">
              <Typography.Link href={institution.contact.postalAddress}>
                {institution.contact.postalAddress}
              </Typography.Link>
            </Descriptions.Item>
          )}
          {institution.contact?.emailAddress && (
            <Descriptions.Item label="E-Mail">
              <Typography.Link href={institution.contact.emailAddress}>
                {institution.contact.emailAddress}
              </Typography.Link>
            </Descriptions.Item>
          )}
          {institution.contact?.websiteLocator && (
            <Descriptions.Item label="Website">
              <Typography.Link href={institution.contact.websiteLocator}>
                {institution.contact.websiteLocator}
              </Typography.Link>
            </Descriptions.Item>
          )}
          {institution.extras != undefined && (
            <Descriptions.Item label="Extras">
              {JSON.stringify(institution.extras, null, "\t")}
            </Descriptions.Item>
          )}
        </Descriptions>
      </PageHeader>
      <Divider />
      <Typography.Title level={2}>Manufactured Components</Typography.Title>
      <ComponentTable
        loading={loading}
        components={institution.manufacturedComponents.edges.map((x) => x.node)}
      />
      {institution.pendingManufacturedComponents.isAuthorizedToConfirmEdge &&
        institution.pendingManufacturedComponents.edges.length >= 1 && (
          <List
            size="small"
            header="Pending"
            dataSource={institution.pendingManufacturedComponents.edges}
            renderItem={(item) => (
              <List.Item key={item.node.uuid}>
                <Link href={paths.component(item.node.uuid)} legacyBehavior>
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
      {institution.manufacturedComponents.isAuthorizedToAddEdge && (
        <CreateComponent manufacturerId={institution.uuid} />
      )}
      <Divider />
      <Typography.Title level={2}>Operated Databases</Typography.Title>
      <DatabaseTable
        loading={loading}
        databases={institution.operatedDatabases.edges.map((x) => x.node)}
      />
      {institution.operatedDatabases.isAuthorizedToAddEdge && (
        <CreateDatabase operatorId={institution.uuid} />
      )}
      <Divider />
      <Typography.Title level={2}>Managed Data Formats</Typography.Title>
      <DataFormatTable
        loading={loading}
        dataFormats={institution.managedDataFormats.edges.map((x) => x.node)}
      />
      {institution.managedDataFormats.isAuthorizedToAddEdge && (
        <CreateDataFormat managerId={institution.uuid} />
      )}
      <Divider />
      <Typography.Title level={2}>Managed Methods</Typography.Title>
      <MethodTable
        loading={loading}
        methods={institution.managedMethods.edges.map((x) => x.node)}
      />
      {institution.managedMethods.isAuthorizedToAddEdge && (
        <CreateMethod managerId={institution.uuid} />
      )}
      <Divider />
      <Typography.Title level={2}>Developed Methods</Typography.Title>
      <List
        size="small"
        dataSource={institution.developedMethods.edges}
        renderItem={(item) => (
          <List.Item key={item.node.uuid}>
            <Link href={paths.method(item.node.uuid)} legacyBehavior>
              {item.node.name}
            </Link>
          </List.Item>
        )}
      />
      {institution.pendingDevelopedMethods.isAuthorizedToConfirmEdge &&
        institution.pendingDevelopedMethods.edges.length >= 1 && (
          <List
            size="small"
            header="Pending"
            dataSource={institution.pendingDevelopedMethods.edges}
            renderItem={(item) => (
              <List.Item key={item.node.uuid}>
                <Link href={paths.method(item.node.uuid)} legacyBehavior>
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
      <Typography.Title level={2}>GnuPG Key Fingerprints</Typography.Title>
      <GnuPgKeyFingerprintTable
        loading={false}
        fingerprints={
          institution.gnuPgKeyFingerprints.edges.map(
            (e) => e.node,
          ) as GnuPgKeyFingerprintPartialFragment[]
        }
        institutionId={institution.uuid}
      />
      {institution.gnuPgKeyFingerprints.isAuthorizedToAddEdge && (
        <AddGnuPgKeyFingerprint institutionId={institution.uuid} />
      )}
      {institution.openIdConnectApplications.isAuthorizedToAddEdge && (
        <>
          <Divider />
          <Typography.Title level={2}>
            OpenId Connect Applications
          </Typography.Title>
          <ApplicationTable
            loading={false}
            applications={
              institution.openIdConnectApplications.edges.map(
                (e) => e.node,
              ) as ApplicationPartialFragment[]
            }
          />
        </>
      )}
      {institution.openIdConnectApplications.isAuthorizedToAddEdge && (
        <CreateApplication institutionId={institution.uuid} />
      )}
      <Divider />
      <Typography.Title level={2}>Managed Institutions</Typography.Title>
      <List
        size="small"
        dataSource={institution.managedInstitutions.edges.map((x) => x.node)}
        renderItem={(item) => (
          <List.Item key={item.uuid}>
            <Link href={paths.institution(item.uuid)} legacyBehavior>
              {item.name}
            </Link>
          </List.Item>
        )}
      />
      {institution.managedInstitutions.isAuthorizedToAddEdge && (
        <CreateInstitution managerId={institution.uuid} />
      )}
      <Divider />
      <Typography.Title level={2}>Representatives</Typography.Title>
      <List
        size="small"
        dataSource={institution.representatives.edges}
        renderItem={(item) => (
          <List.Item key={item.node.uuid}>
            <Link href={paths.user(item.node.uuid)} legacyBehavior>
              {item.node.name}
            </Link>
            <Typography.Text>{item.role}</Typography.Text>
          </List.Item>
        )}
      />
      {institution.representatives.isAuthorizedToAddEdge &&
        institution.pendingRepresentatives.edges.length >= 1 && (
          <List
            size="small"
            header="Pending"
            dataSource={institution.pendingRepresentatives.edges}
            renderItem={(item) => (
              <List.Item key={item.node.uuid}>
                <Link href={paths.user(item.node.uuid)} legacyBehavior>
                  {item.node.name}
                </Link>
                <Typography.Text>{item.role}</Typography.Text>
              </List.Item>
            )}
          />
        )}
      {institution.representatives.isAuthorizedToAddEdge && (
        <AddInstitutionRepresentative institutionId={institution.uuid} />
      )}
      {institution.manager?.node && (
        <>
          <Divider />
          <Typography.Title level={2}>Managing Institution</Typography.Title>
          <Link
            href={paths.institution(institution.manager?.node?.uuid)}
            legacyBehavior
          >
            {institution.manager?.node?.name}
          </Link>
        </>
      )}
    </>
  );
}
