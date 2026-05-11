import { useQuery } from "@apollo/client/react";
import { Divider, Typography, Skeleton, Result, Card } from "antd";
import {
  InstitutionDocument,
  InstitutionPartialFragment,
} from "../../queries/institutions.generated";
import { Scalars, SortEnumType } from "../../__generated__/graphql";
import CreateComponent from "../components/CreateComponent";
import CreateMethod from "../methods/CreateMethod";
import CreateDataFormat from "../dataFormats/CreateDataFormat";
import CreateInstitution from "../institutions/CreateInstitution";
import CreateDatabase from "../databases/CreateDatabase";
import CreateOpenIdConnectApplication from "../openIdConnect/applications/CreateOpenIdConnectApplication";
import AddGnuPgKeyFingerprint from "../gnuPgKeys/AddGnuPgKeyFingerprint";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import ConfirmInstitutionMethodDeveloper from "../methods/ConfirmInstitutionMethodDeveloper";
import ConfirmComponentManufacturer from "../components/ConfirmComponentManufacturer";
import { isTruthy } from "../../lib/array";
import PaginatedMethods from "../methods/PaginatedMethods";
import PaginatedDataFormats from "../dataFormats/PaginatedDataFormats";
import PaginatedInstitutions from "./PaginatedInstitutions";
import PaginatedOpenIdConnectApplications from "../openIdConnect/applications/PaginatedOpenIdConnectApplications";
import PaginatedComponents from "../components/PaginatedComponents";
import LazyTabs, { LazyTabsProps } from "../LazyTabs";
import QueryToolbar from "../QueryToolbar";
import PaginatedGnuPgKeys from "../gnuPgKeys/PaginatedGnuPgKeys";
import { useMemo } from "react";
import InstitutionSummary from "./InstitutionSummary";
import PaginatedAnyDatabases from "../databases/PaginatedAnyDatabases";
import EntityItem from "../entities/EntityItem";
import EntityList from "../entities/EntityList";
import ComponentSummary from "../components/ComponentSummary";
import MethodSummary from "../methods/MethodSummary";

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
          order={{ createdAt: SortEnumType.Desc }}
          extra={
            institution.managedComponents.isAuthorizedToAddEdge && (
              <CreateComponent
                initialManagerId={institution.uuid}
                initialManufacturerId={institution.uuid}
              />
            )
          }
        />
      ),
    },
    {
      key: "methods",
      count: institution.institutionDevelopedMethods.totalCount,
      label: "Developed Methods",
      children: (
        <PaginatedMethods
          where={{
            institutionDevelopers: {
              some: { id: { equalTo: institution.uuid } },
            },
          }}
          order={{ createdAt: SortEnumType.Desc }}
          extra={
            institution.managedMethods.isAuthorizedToAddEdge && (
              <CreateMethod
                initialManagerId={institution.uuid}
                initialInstitutionDeveloperIds={[institution.uuid]}
              />
            )
          }
        />
      ),
    },
    {
      key: "databases",
      count: institution.operatedDatabases.totalCount,
      label: "Operated Databases",
      children: (
        <PaginatedAnyDatabases
          where={{
            operator: {
              id: { equalTo: institution.uuid },
            },
          }}
          order={{ createdAt: SortEnumType.Desc }}
          extra={
            institution.operatedDatabases.isAuthorizedToAddEdge && (
              <CreateDatabase initialOperatorId={institution.uuid} />
            )
          }
        />
      ),
    },
    (institution.gnuPgKeyFingerprints.edges.length > 0 ||
      institution.gnuPgKeyFingerprints.isAuthorizedToAddEdge) && {
      key: "gnuPgKeyFingerprints",
      count: institution.gnuPgKeyFingerprints.totalCount,
      label: "GnuPG Key Fingerprints",
      children: (
        <PaginatedGnuPgKeys
          where={{
            institution: {
              id: { equalTo: institution.uuid },
            },
          }}
          order={{ createdAt: SortEnumType.Desc }}
          extra={
            institution.gnuPgKeyFingerprints.isAuthorizedToAddEdge && (
              <AddGnuPgKeyFingerprint institutionId={institution.uuid} />
            )
          }
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
          order={{ createdAt: SortEnumType.Desc }}
          extra={
            institution.managedComponents.isAuthorizedToAddEdge && (
              <CreateComponent
                initialManagerId={institution.uuid}
                initialManufacturerId={institution.uuid}
              />
            )
          }
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
          order={{ createdAt: SortEnumType.Desc }}
          extra={
            institution.managedMethods.isAuthorizedToAddEdge && (
              <CreateMethod initialManagerId={institution.uuid} />
            )
          }
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
          order={{ createdAt: SortEnumType.Desc }}
          extra={
            institution.managedDataFormats.isAuthorizedToAddEdge && (
              <CreateDataFormat initialManagerId={institution.uuid} />
            )
          }
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
          order={{ createdAt: SortEnumType.Desc }}
          extra={
            institution.managedInstitutions.isAuthorizedToAddEdge && (
              <CreateInstitution initialManagerId={institution.uuid} />
            )
          }
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
          order={{ createdAt: SortEnumType.Desc }}
          extra={
            institution.openIdConnectApplications.isAuthorizedToAddEdge && (
              <CreateOpenIdConnectApplication
                initialOwnerId={institution.uuid}
              />
            )
          }
        />
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
        count: institution.pendingManufacturedComponents.totalCount,
        label: "Components",
        children: (
          <EntityList
            loading={false}
            dataSource={institution.pendingManufacturedComponents.edges.map(
              (edge) => edge.node,
            )}
            renderItem={(node) => (
              <EntityItem>
                <ComponentSummary
                  hideInputControls
                  entity={node}
                  extra={
                    <ConfirmComponentManufacturer
                      componentId={node.uuid}
                      institutionId={institution.uuid}
                    />
                  }
                />
              </EntityItem>
            )}
          />
        ),
      },
    institution.pendingInstitutionDevelopedMethods.isAuthorizedToConfirmEdges &&
      institution.pendingInstitutionDevelopedMethods.edges.length > 0 && {
        key: "methods",
        count: institution.pendingInstitutionDevelopedMethods.totalCount,
        label: "Methods",
        children: (
          <EntityList
            loading={false}
            dataSource={institution.pendingInstitutionDevelopedMethods.edges.map(
              (edge) => edge.node,
            )}
            renderItem={(node) => (
              <EntityItem>
                <MethodSummary
                  hideInputControls
                  entity={node}
                  extra={
                    <ConfirmInstitutionMethodDeveloper
                      methodId={node.uuid}
                      institutionId={institution.uuid}
                    />
                  }
                />
              </EntityItem>
            )}
          />
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
      pending: getPendingTabs(institution),
    };
  }, [institution]);

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
    <div>
      <Card style={{ marginBottom: "1em" }}>
        <InstitutionSummary entity={institution} />
      </Card>
      <QueryToolbar query={InstitutionDocument} variables={queryVariables} />
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
      {tabs?.pending && tabs.pending.length > 0 && (
        <>
          <Divider />
          <Typography.Title level={4} id="pending-entities">
            Pending Entities
          </Typography.Title>
          <LazyTabs items={tabs.pending} />
        </>
      )}
    </div>
  );
}
