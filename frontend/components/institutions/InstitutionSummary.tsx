import { Space, Typography } from "antd";
import { asReadonlyMixed, isTruthy } from "../../lib/array";
import paths from "../../paths";
import {
  InstitutionsPartialFragment,
  InstitutionPartialFragment,
  PendingInstitutionsPartialFragment,
} from "../../queries/institutions.generated";
import ContactInformation from "../ContactInformation";
import EntityLink from "../entities/EntityLink";
import EntitySummary from "../entities/EntitySummary";
import InlineList from "../InlineList";
import JsonView from "../JsonView";
import Manager from "../Manager";
import DeleteInstitution from "./DeleteInstitution";
import RemoveInstitutionRepresentative from "./RemoveInstitutionRepresentative";
import SwitchInstitutionOperatingState from "./SwitchInstitutionOperatingState";
import UpdateInstitution from "./UpdateInstitution";
import VerifyInstitution from "./VerifyInstitution";
import { InstitutionState, Scalars } from "../../__generated__/graphql";
import Link from "next/link";
import AddInstitutionRepresentative from "./AddInstitutionRepresentative";
import EnumTag from "../EnumTag";

const renderRepresentativeList = (
  representatives:
    | NonNullable<InstitutionsPartialFragment["representatives"]>
    | NonNullable<InstitutionPartialFragment["representatives"]>
    | NonNullable<InstitutionPartialFragment["pendingRepresentatives"]>,
  institutionId: Scalars["Uuid"]["output"],
  hideInputControls: boolean | undefined,
) => (
  <InlineList
    items={asReadonlyMixed(representatives.edges)}
    renderItem={(edge) => (
      <span key={edge.node.id}>
        <Space>
          <EntityLink entity={edge.node} route={paths.user} />
          <EnumTag color="grey" variant="outlined">
            {edge.role}
          </EnumTag>
        </Space>
        {!hideInputControls &&
          "isAuthorizedToRemoveEdge" in edge &&
          edge.isAuthorizedToRemoveEdge && (
            <RemoveInstitutionRepresentative
              institutionId={institutionId}
              userId={edge.node.uuid}
            />
          )}
      </span>
    )}
  />
);

export default function InstitutionSummary({
  entity,
  hideInputControls = false,
  showVerifyAnyway = false,
}: {
  entity:
    | InstitutionsPartialFragment
    | PendingInstitutionsPartialFragment
    | InstitutionPartialFragment;
  hideInputControls?: boolean;
  showVerifyAnyway?: boolean;
}) {
  return (
    <EntitySummary
      entity={entity}
      route={paths.institution}
      tags={[
        <EnumTag key={entity.state} color="magenta">
          {entity.state}
        </EnumTag>,
        <EnumTag key={entity.operatingState} color="blue">
          {entity.operatingState}
        </EnumTag>,
      ]}
      extra={[
        (!hideInputControls || showVerifyAnyway) &&
          "isAuthorizedToVerifyNode" in entity &&
          entity.isAuthorizedToVerifyNode &&
          entity.state == InstitutionState.Pending && (
            <VerifyInstitution institutionId={entity.uuid} />
          ),
        !hideInputControls &&
          "isAuthorizedToUpdateNode" in entity &&
          entity.isAuthorizedToUpdateNode && (
            <UpdateInstitution institution={entity} />
          ),
        !hideInputControls &&
          "isAuthorizedToSwitchOperatingStateOfNode" in entity &&
          entity.isAuthorizedToSwitchOperatingStateOfNode && (
            <SwitchInstitutionOperatingState institutionId={entity.uuid} />
          ),
        !hideInputControls &&
          "isAuthorizedToDeleteNode" in entity &&
          entity.isAuthorizedToDeleteNode && (
            <DeleteInstitution institutionId={entity.uuid} />
          ),
      ].filter(isTruthy)}
    >
      {entity.state == InstitutionState.Pending && (
        <Typography.Paragraph style={{ maxWidth: "75ch" }}>
          The institution is awaiting verification by one of the verifiers. Once
          it is verified, it will be listed on{" "}
          <Link href={paths.institutions}>Institutions</Link> and its
          representatives will be able to manage its components, databases, data
          formats, methods, OpenID Connect applications, and other
          representatives.
        </Typography.Paragraph>
      )}
      <ContactInformation contact={entity.contact} />
      {entity.representatives.edges.length > 0 && (
        <div>
          Represented by{" "}
          {renderRepresentativeList(
            entity.representatives,
            entity.uuid,
            hideInputControls,
          )}
          {"pendingRepresentatives" in entity &&
            entity.pendingRepresentatives &&
            entity.pendingRepresentatives.edges.length > 0 && (
              <>
                {" "}
                and awaiting confirmation of
                {renderRepresentativeList(
                  entity.pendingRepresentatives,
                  entity.uuid,
                  hideInputControls,
                )}
              </>
            )}
          {!hideInputControls &&
            "isAuthorizedToAddEdge" in entity.representatives &&
            entity.representatives.isAuthorizedToAddEdge && (
              <AddInstitutionRepresentative institutionId={entity.uuid} />
            )}
        </div>
      )}
      {"manager" in entity && entity.manager?.node && (
        <div>
          <Manager data={entity.manager.node} />
        </div>
      )}
      {"extras" in entity && entity.extras != null && (
        <JsonView data={entity.extras} />
      )}
    </EntitySummary>
  );
}
