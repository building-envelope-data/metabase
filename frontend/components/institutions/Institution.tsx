import { useQuery } from "@apollo/client/react";
import { Divider, List, Typography, Skeleton, Result, Card } from "antd";
import {
  InstitutionDocument,
  InstitutionPartialFragment,
} from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";
import CreateComponent from "../components/CreateComponent";
import CreateMethod from "../methods/CreateMethod";
import CreateDataFormat from "../dataFormats/CreateDataFormat";
import CreateInstitution from "../institutions/CreateInstitution";
import CreateDatabase from "../databases/CreateDatabase";
import Link from "next/link";
import paths from "../../paths";
import CreateOpenIdConnectApplication from "../openIdConnect/applications/CreateOpenIdConnectApplication";
import AddGnuPgKeyFingerprint from "../gnuPgKeyFingerprints/AddGnuPgKeyFingerprint";
import RemoveInstitutionRepresentative from "./RemoveInstitutionRepresentative";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import ConfirmInstitutionMethodDeveloper from "../methods/ConfirmInstitutionMethodDeveloper";
import { ConfirmComponentManufacturer } from "../components/ConfirmComponentManufacturer";
import { isTruthy } from "../../lib/array";
import PaginatedMethods from "../methods/PaginatedMethods";
import PaginatedDatabases from "../databases/PaginatedDatabases";
import PaginatedDataFormats from "../dataFormats/PaginatedDataFormats";
import PaginatedInstitutions from "./PaginatedInstitutions";
import PaginatedOpenIdConnectApplications from "../openIdConnect/applications/PaginatedOpenIdConnectApplications";
import PaginatedComponents from "../components/PaginatedComponents";
import LazyTabs, { LazyTabsProps } from "../LazyTabs";
import QueryToolbar from "../QueryToolbar";
import PaginatedGnuPgKeyFingerprints from "../gnuPgKeyFingerprints/PaginatedGnuPgKeyFingerprints";
import { useMemo } from "react";
import InstitutionSummary from "./InstitutionSummary";

const getMainTabs = (
  institution: InstitutionPartialFragment,
): LazyTabsProps["items"] =>
  [
    {
      key: "components",
      count: institution.manufacturedComponents.totalCount,
      label: "Manufactured Components",
      children: (
        <PaginatedComponents
          where={{
            manufacturers: {
              some: { id: { equalTo: institution.uuid } },
            },
          }}
        />
      ),
    },
    {
      key: "methods",
      count: institution.developedMethods.totalCount,
      label: "Developed Methods",
      children: (
        <PaginatedMethods
          where={{
            institutionDevelopers: {
              some: { id: { equalTo: institution.uuid } },
            },
          }}
        />
      ),
    },
    {
      key: "databases",
      count: institution.operatedDatabases.totalCount,
      label: "Operated Databases",
      children: (
        <PaginatedDatabases
          where={{
            operator: {
              id: { equalTo: institution.uuid },
            },
          }}
        />
      ),
    },
    (institution.gnuPgKeyFingerprints.edges.length > 0 ||
      institution.gnuPgKeyFingerprints.isAuthorizedToAddEdge) && {
      key: "gnuPgKeyFingerprints",
      count: institution.gnuPgKeyFingerprints.totalCount,
      label: "GnuPG Key Fingerprints",
      children: (
        <PaginatedGnuPgKeyFingerprints
          where={{
            institution: {
              id: { equalTo: institution.uuid },
            },
          }}
        />
      ),
    },
  ].filter(isTruthy);

const getManagedTabs = (
  institution: InstitutionPartialFragment,
): LazyTabsProps["items"] =>
  [
    {
      key: "components",
      count: institution.managedComponents.totalCount,
      label: "Components",
      children: (
        <PaginatedComponents
          where={{
            manager: {
              id: { equalTo: institution.uuid },
            },
          }}
        />
      ),
    },
    {
      key: "methods",
      count: institution.managedMethods.totalCount,
      label: "Methods",
      children: (
        <PaginatedMethods
          where={{
            manager: {
              id: { equalTo: institution.uuid },
            },
          }}
        />
      ),
    },
    {
      key: "dataFormats",
      count: institution.managedDataFormats.totalCount,
      label: "Data Formats",
      children: (
        <PaginatedDataFormats
          where={{
            manager: {
              id: { equalTo: institution.uuid },
            },
          }}
        />
      ),
    },
    {
      key: "institutions",
      count: institution.managedInstitutions.totalCount,
      label: "Institutions",
      children: (
        <PaginatedInstitutions
          where={{
            manager: {
              id: { equalTo: institution.uuid },
            },
          }}
        />
      ),
    },
    institution.openIdConnectApplications.isAuthorizedToAddEdge && {
      key: "openIdConnectApplications",
      count: institution.openIdConnectApplications.totalCount,
      label: "OpenId Connect Applications",
      children: (
        <PaginatedOpenIdConnectApplications
          where={{
            owner: {
              id: { equalTo: institution.uuid },
            },
          }}
        />
      ),
    },
  ].filter(isTruthy);

const getCreateTabs = (
  institution: InstitutionPartialFragment,
): LazyTabsProps["items"] =>
  [
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
  ].filter(isTruthy);

const getPendingTabs = (
  institution: InstitutionPartialFragment,
): LazyTabsProps["items"] =>
  [
    institution.pendingManufacturedComponents.isAuthorizedToConfirmEdges &&
      institution.pendingManufacturedComponents.edges.length > 0 && {
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
      institution.pendingDevelopedMethods.edges.length > 0 && {
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
      institution.pendingRepresentatives.edges.length > 0 && {
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

interface Props {
  institutionId: Scalars["Uuid"]["input"];
}

export default function Institution({ institutionId }: Props) {
  const queryVariables = {
    uuid: institutionId,
  };
  const { loading, error, data } = useQuery(InstitutionDocument, {
    variables: queryVariables,
  });
  useQueryHandler({ error });
  const institution = data?.institution;

  const tabs = useMemo(() => {
    if (!institution) return null;
    return {
      main: getMainTabs(institution),
      managed: getManagedTabs(institution),
      create: getCreateTabs(institution),
      pending: getPendingTabs(institution),
    };
  }, [institution]);

  if (loading) {
    return <Skeleton active avatar title />;
  }
  Card;
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
      <InstitutionSummary entity={institution} />
      <Divider />
      {tabs?.main && <LazyTabs items={tabs?.main} />}
      {tabs?.managed && tabs.managed.length > 0 && (
        <>
          <Divider />
          <Typography.Title level={4}>
            Managed &amp; Owned Entities
          </Typography.Title>
          <LazyTabs items={tabs.managed} />
        </>
      )}
      {tabs?.create && tabs.create.length > 0 && (
        <>
          <Divider />
          <Typography.Title level={4}>
            Create &amp; Add Entities
          </Typography.Title>
          <LazyTabs items={tabs.create} />
        </>
      )}
      {tabs?.pending && tabs.pending.length > 0 && (
        <>
          <Divider />
          <Typography.Title level={4}>Pending Entities</Typography.Title>
          <LazyTabs items={tabs.pending} />
        </>
      )}
      <Divider />
      <QueryToolbar query={InstitutionDocument} variables={queryVariables} />
    </>
  );
}
