import { useQuery } from "@apollo/client/react";
import {
  Divider,
  List,
  Typography,
  Skeleton,
  Result,
  Descriptions,
  Tag,
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
import { ReactNode } from "react";
import { DataFormatTable } from "../dataFormats/DataFormatTable";
import { ComponentTable } from "../components/ComponentTable";
import DatabaseTable from "../databases/DatabaseTable";
import MethodTable from "../methods/MethodTable";
import UpdateInstitution from "./UpdateInstitution";
import DeleteInstitution from "./DeleteInstitution";
import SwitchInstitutionOperatingState from "./SwitchInstitutionOperatingState";
import OpenIdConnectApplicationTable from "../openIdConnect/applications/OpenIdConnectApplicationTable";
import CreateOpenIdConnectApplication from "../openIdConnect/applications/CreateOpenIdConnectApplication";
import GnuPgKeyFingerprintTable from "../gnuPgKeyFingerprints/GnuPgKeyFingerprintTable";
import AddGnuPgKeyFingerprint from "../gnuPgKeyFingerprints/AddGnuPgKeyFingerprint";
import { GnuPgKeyFingerprintsPartialFragment } from "../../queries/gnuPgKeyFingerprints.generated";
import { OpenIdConnectApplicationsPartialFragment } from "../../queries/openIdConnect.generated";
import RemoveInstitutionRepresentative from "./RemoveInstitutionRepresentative";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import ConfirmInstitutionMethodDeveloper from "../methods/ConfirmInstitutionMethodDeveloper";
import { ConfirmComponentManufacturer } from "../components/ConfirmComponentManufacturer";

interface InstitutionProps {
  institutionId: Scalars["Uuid"]["input"];
};

export default function Institution({ institutionId }: InstitutionProps) {
  const { loading, error, data } = useQuery(InstitutionDocument, {
    variables: {
      uuid: institutionId,
    },
  });
  useQueryHandler({ error });
  const institution = data?.institution;

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
                    institution={institution}
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
              {institution.contact.phoneNumber}
            </Descriptions.Item>
          )}
          {institution.contact?.postalAddress && (
            <Descriptions.Item label="Postal Address">
              {institution.contact.postalAddress}
            </Descriptions.Item>
          )}
          {institution.contact?.emailAddress && (
            <Descriptions.Item label="E-Mail">
              {institution.contact.emailAddress}
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
      {institution.pendingManufacturedComponents.isAuthorizedToConfirmEdges &&
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
                <ConfirmComponentManufacturer
                  componentId={item.node.uuid}
                  institutionId={institution.uuid}
                />
              </List.Item>
            )}
          />
        )}
      <Divider />
      <Typography.Title level={2}>Managed Components</Typography.Title>
      <ComponentTable
        loading={loading}
        components={institution.managedComponents.edges.map((x) => x.node)}
      />
      {institution.managedComponents.isAuthorizedToAddEdge && (
        <CreateComponent
          managerId={institution.uuid}
          initialManufacturerId={institution.uuid}
        />
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
            <Link href={paths.method(item.node.uuid)}>{item.node.name}</Link>
          </List.Item>
        )}
      />
      {institution.pendingDevelopedMethods.isAuthorizedToConfirmEdges &&
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
                <ConfirmInstitutionMethodDeveloper
                  methodId={item.node.uuid}
                  institutionId={institution.uuid}
                />
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
          ) as GnuPgKeyFingerprintsPartialFragment[]
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
          <OpenIdConnectApplicationTable
            loading={false}
            applications={
              institution.openIdConnectApplications.edges.map(
                (e) => e.node,
              ) as OpenIdConnectApplicationsPartialFragment[]
            }
          />
        </>
      )}
      {institution.openIdConnectApplications.isAuthorizedToAddEdge && (
        <CreateOpenIdConnectApplication institutionId={institution.uuid} />
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
            <Link href={paths.user(item.node.uuid)}>
              {`${item.node.name} (${item.node.uuid})`}
            </Link>
            <Typography.Text>{item.role}</Typography.Text>
            {item.isAuthorizedToRemoveEdge && (
              <RemoveInstitutionRepresentative
                institutionId={institution.uuid}
                userId={item.node.uuid}
              />
            )}
          </List.Item>
        )}
      />
      {institution.representatives.isAuthorizedToAddEdge &&
        institution.pendingRepresentatives != null &&
        institution.pendingRepresentatives.edges.length >= 1 && (
          <List
            size="small"
            header="Pending"
            dataSource={institution.pendingRepresentatives.edges}
            renderItem={(item) => (
              <List.Item key={item.node.uuid}>
                <Link href={paths.user(item.node.uuid)}>
                  {`${item.node.name} (${item.node.uuid})`}
                </Link>
                <Typography.Text>{item.role}</Typography.Text>
                {item.isAuthorizedToRemoveEdge && (
                  <RemoveInstitutionRepresentative
                    institutionId={institution.uuid}
                    userId={item.node.uuid}
                  />
                )}
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
          <Link href={paths.institution(institution.manager?.node?.uuid)}>
            {institution.manager?.node?.name}
          </Link>
        </>
      )}
    </>
  );
}
