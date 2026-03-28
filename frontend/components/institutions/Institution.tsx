import { useQuery } from "@apollo/client/react";
import {
  Divider,
  List,
  Typography,
  Skeleton,
  Result,
  Tag,
  Space,
  Tabs,
  TabsProps,
} from "antd";
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
import ContactInformation from "../ContactInformation";
import JsonViewer from "../JsonViewer";
import PageHeader from "../PageHeader";
import { isTruthy } from "../../lib/array";
import InstitutionTable from "./InstitutionTable";

interface Props {
  institutionId: Scalars["Uuid"]["input"];
}

export default function Institution({ institutionId }: Props) {
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

  const mainTabs: TabsProps["items"] = [
    (institution.manufacturedComponents.edges.length >= 1 ||
      institution.managedComponents.isAuthorizedToAddEdge) && {
      key: "components",
      label: "Manufactured Components",
      children: (
        <ComponentTable
          loading={loading}
          components={institution.manufacturedComponents.edges.map(
            (x) => x.node,
          )}
        />
      ),
    },
    (institution.developedMethods.edges.length >= 1 ||
      institution.managedMethods.isAuthorizedToAddEdge) && {
      key: "methods",
      label: "Developed Methods",
      children: (
        <MethodTable
          loading={loading}
          methods={institution.developedMethods.edges.map((x) => x.node)}
        />
      ),
    },
    (institution.operatedDatabases.edges.length >= 1 ||
      institution.operatedDatabases.isAuthorizedToAddEdge) && {
      key: "databases",
      label: "Operated Databases",
      children: (
        <DatabaseTable
          loading={loading}
          databases={institution.operatedDatabases.edges.map((x) => x.node)}
        />
      ),
    },
    (institution.gnuPgKeyFingerprints.edges.length >= 1 ||
      institution.gnuPgKeyFingerprints.isAuthorizedToAddEdge) && {
      key: "gnuPgKeyFingerprints",
      label: "GnuPG Key Fingerprints",
      children: (
        <GnuPgKeyFingerprintTable
          loading={false}
          fingerprints={
            institution.gnuPgKeyFingerprints.edges.map(
              (e) => e.node,
            ) as GnuPgKeyFingerprintsPartialFragment[]
          }
          institutionId={institution.uuid}
        />
      ),
    },
  ].filter(isTruthy);

  const managedTabs: TabsProps["items"] = [
    (institution.managedComponents.edges.length >= 1 ||
      institution.managedComponents.isAuthorizedToAddEdge) && {
      key: "components",
      label: "Components",
      children: (
        <ComponentTable
          loading={loading}
          components={institution.managedComponents.edges.map((x) => x.node)}
        />
      ),
    },
    (institution.managedMethods.edges.length >= 1 ||
      institution.managedMethods.isAuthorizedToAddEdge) && {
      key: "methods",
      label: "Methods",
      children: (
        <MethodTable
          loading={loading}
          methods={institution.managedMethods.edges.map((x) => x.node)}
        />
      ),
    },
    (institution.managedDataFormats.edges.length >= 1 ||
      institution.managedDataFormats.isAuthorizedToAddEdge) && {
      key: "dataFormats",
      label: "Data Formats",
      children: (
        <DataFormatTable
          loading={loading}
          dataFormats={institution.managedDataFormats.edges.map((x) => x.node)}
        />
      ),
    },
    (institution.managedInstitutions.edges.length >= 1 ||
      institution.managedInstitutions.isAuthorizedToAddEdge) && {
      key: "institutions",
      label: "Institutions",
      children: (
        <InstitutionTable
          loading={loading}
          institutions={institution.managedInstitutions.edges.map(
            (x) => x.node,
          )}
        />
      ),
    },
    institution.openIdConnectApplications.isAuthorizedToAddEdge && {
      key: "openIdConnectApplications",
      label: "OpenId Connect Applications",
      children: (
        <OpenIdConnectApplicationTable
          loading={false}
          applications={
            institution.openIdConnectApplications.edges.map(
              (e) => e.node,
            ) as OpenIdConnectApplicationsPartialFragment[]
          }
        />
      ),
    },
  ].filter(isTruthy);

  const createTabs: TabsProps["items"] = [
    institution.managedComponents.isAuthorizedToAddEdge && {
      key: "components",
      label: "Components",
      children: (
        <CreateComponent
          managerId={institution.uuid}
          initialManufacturerId={institution.uuid}
        />
      ),
    },
    institution.managedMethods.isAuthorizedToAddEdge && {
      key: "methods",
      label: "Methods",
      children: <CreateMethod managerId={institution.uuid} />,
    },
    institution.managedDataFormats.isAuthorizedToAddEdge && {
      key: "dataFormats",
      label: "Data Formats",
      children: <CreateDataFormat managerId={institution.uuid} />,
    },
    institution.managedInstitutions.isAuthorizedToAddEdge && {
      key: "institutions",
      label: "Institutions",
      children: <CreateInstitution managerId={institution.uuid} />,
    },
    institution.operatedDatabases.isAuthorizedToAddEdge && {
      key: "databases",
      label: "Databases",
      children: <CreateDatabase operatorId={institution.uuid} />,
    },
    institution.gnuPgKeyFingerprints.isAuthorizedToAddEdge && {
      key: "gnuPgKeyFingerprints",
      label: "GnuPG Key Fingerprints",
      children: <AddGnuPgKeyFingerprint institutionId={institution.uuid} />,
    },
    institution.openIdConnectApplications.isAuthorizedToAddEdge && {
      key: "openIdConnectApplications",
      label: "OpenId Connect Applications",
      children: (
        <CreateOpenIdConnectApplication institutionId={institution.uuid} />
      ),
    },
    institution.representatives.isAuthorizedToAddEdge && {
      key: "representatives",
      label: "Representatives",
      children: (
        <>
          <AddInstitutionRepresentative institutionId={institution.uuid} />
        </>
      ),
    },
  ].filter(isTruthy);

  const pendingTabs: TabsProps["items"] = [
    institution.pendingManufacturedComponents.isAuthorizedToConfirmEdges &&
      institution.pendingManufacturedComponents.edges.length >= 1 && {
        key: "components",
        label: "Components",
        children: (
          <>
            <List
              size="small"
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
          </>
        ),
      },
    institution.pendingDevelopedMethods.isAuthorizedToConfirmEdges &&
      institution.pendingDevelopedMethods.edges.length >= 1 && {
        key: "methods",
        label: "Methods",
        children: (
          <>
            <List
              size="small"
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
          </>
        ),
      },
    institution.representatives.isAuthorizedToAddEdge &&
      institution.pendingRepresentatives != null &&
      institution.pendingRepresentatives.edges.length >= 1 && {
        key: "representatives",
        label: "Representatives",
        children: (
          <>
            <List
              size="small"
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
          </>
        ),
      },
  ].filter(isTruthy);

  return (
    <>
      <PageHeader
        id={institution.uuid}
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
        extra={[
          institution.isAuthorizedToUpdateNode && (
            <UpdateInstitution institution={institution} />
          ),
          institution.isAuthorizedToSwitchOperatingStateOfNode && (
            <SwitchInstitutionOperatingState institutionId={institution.uuid} />
          ),
          institution.isAuthorizedToDeleteNode && (
            <DeleteInstitution institutionId={institution.uuid} />
          ),
        ].filter(isTruthy)}
      >
        <Space orientation="vertical">
          {institution.extras != null && (
            <JsonViewer jsonData={institution.extras} />
          )}
          <ContactInformation contact={institution.contact} />
          {institution.representatives.edges.length >= 1 && (
            <div>
              <>
                Represented by{" "}
                {institution.representatives.edges.map((edge, index) => (
                  <span key={edge.node.uuid}>
                    <Link href={paths.user(edge.node.uuid)}>
                      {`${edge.node.name} (${edge.node.uuid})`}
                    </Link>{" "}
                    as <Typography.Text>{edge.role}</Typography.Text>
                    {edge.isAuthorizedToRemoveEdge && (
                      <RemoveInstitutionRepresentative
                        institutionId={institution.uuid}
                        userId={edge.node.uuid}
                      />
                    )}
                    {index < institution.representatives.edges.length - 2 &&
                      ", "}
                    {index < institution.representatives.edges.length - 1 &&
                      " and "}
                  </span>
                ))}
              </>
            </div>
          )}
          {institution.manager?.node && (
            <div>
              <>
                Managed by{" "}
                <Link href={paths.institution(institution.manager?.node?.uuid)}>
                  {institution.manager?.node?.name}
                </Link>
              </>
            </div>
          )}
        </Space>
      </PageHeader>
      <Divider />
      {mainTabs.length >= 1 && <Tabs items={mainTabs} />}
      {managedTabs.length >= 1 && (
        <>
          <Divider />
          <Typography.Title level={2}>
            Managed &amp; Owned Entities
          </Typography.Title>
          <Tabs items={managedTabs} />
        </>
      )}
      {createTabs.length >= 1 && (
        <>
          <Divider />
          <Typography.Title level={2}>
            Create &amp; Add Entities
          </Typography.Title>
          <Tabs items={createTabs} />
        </>
      )}
      {pendingTabs.length >= 1 && (
        <>
          <Divider />
          <Typography.Title level={2}>Pending Entities</Typography.Title>
          <Tabs items={pendingTabs} />
        </>
      )}
    </>
  );
}
