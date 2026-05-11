import {
  UserPartialFragment,
  UsersPartialFragment,
} from "../../queries/users.generated";
import paths from "../../paths";
import UserRoleTag from "./UserRoleTag";
import InlineList from "../InlineList";
import EntityLink from "../entities/EntityLink";
import ContactInformation from "../ContactInformation";
import EntitySummary from "../entities/EntitySummary";
import { asReadonlyMixed, isTruthy, pluralize } from "../../lib/array";
import DeleteUser from "./DeleteUser";
import RemoveInstitutionRepresentative from "../institutions/RemoveInstitutionRepresentative";
import AddUserRole from "./AddUserRole";
import EnumTag from "../EnumTag";
import { Space } from "antd";
import { Scalars } from "../../__generated__/graphql";
import ConfirmInstitutionRepresentative from "../institutions/ConfirmInstitutionRepresentative";
import RemoveUserMethodDeveloper from "../methods/RemoveUserMethodDeveloper";
import ConfirmUserMethodDeveloper from "../methods/ConfirmUserMethodDeveloper";

const renderRepresentedInstitutionList = (
  representedInstitutions:
    | NonNullable<UsersPartialFragment["representedInstitutions"]>
    | NonNullable<UserPartialFragment["representedInstitutions"]>
    | NonNullable<UserPartialFragment["pendingRepresentedInstitutions"]>,
  userId: Scalars["Uuid"]["output"],
  hideInputControls: boolean | undefined,
) => (
  <InlineList
    items={asReadonlyMixed(representedInstitutions.edges)}
    renderItem={(edge) => (
      <span key={edge.node.id}>
        <Space>
          <EntityLink entity={edge.node} route={paths.institution} />
          <EnumTag color="grey" variant="outlined">
            {edge.role}
          </EnumTag>
        </Space>
        {!hideInputControls &&
          "isAuthorizedToConfirmEdges" in representedInstitutions &&
          representedInstitutions.isAuthorizedToConfirmEdges && (
            <ConfirmInstitutionRepresentative
              institutionId={edge.node.uuid}
              userId={userId}
            />
          )}
        {!hideInputControls &&
          "isAuthorizedToRemoveEdge" in edge &&
          edge.isAuthorizedToRemoveEdge && (
            <RemoveInstitutionRepresentative
              institutionId={edge.node.uuid}
              userId={userId}
            >
              Deny
            </RemoveInstitutionRepresentative>
          )}
      </span>
    )}
  />
);

const renderUserDevelopedMethodList = (
  userDevelopedMethods:
    | NonNullable<UserPartialFragment["userDevelopedMethods"]>
    | NonNullable<UserPartialFragment["pendingUserDevelopedMethods"]>,
  userId: Scalars["Uuid"]["output"],
  hideInputControls: boolean | undefined,
) => (
  <InlineList
    items={userDevelopedMethods.edges}
    renderItem={(edge) => (
      <span key={edge.node.id}>
        <EntityLink
          key={edge.node.id}
          entity={edge.node}
          route={paths.method}
        />
        {!hideInputControls &&
          "isAuthorizedToConfirmEdges" in userDevelopedMethods &&
          userDevelopedMethods.isAuthorizedToConfirmEdges && (
            <ConfirmUserMethodDeveloper
              userId={userId}
              methodId={edge.node.uuid}
            />
          )}
        {!hideInputControls && edge.isAuthorizedToRemoveEdge && (
          <RemoveUserMethodDeveloper userId={userId} methodId={edge.node.uuid}>
            Deny
          </RemoveUserMethodDeveloper>
        )}
      </span>
    )}
  />
);

export default function UserSummary({
  entity,
  hideInputControls = false,
}: {
  entity: UsersPartialFragment | UserPartialFragment;
  hideInputControls?: boolean;
}) {
  const rolesCurrentUserCanAndMayWantToAdd =
    "rolesCurrentUserCanAdd" in entity &&
    entity?.rolesCurrentUserCanAdd?.filter(
      (role) => !entity.roles?.includes(role),
    );

  return (
    <EntitySummary
      entity={entity}
      route={paths.user}
      tags={[
        ...(entity.roles?.map((role) => (
          <UserRoleTag
            key={role}
            userId={entity.uuid}
            role={role}
            canRemove={
              !hideInputControls &&
              "rolesCurrentUserCanRemove" in entity &&
              entity.rolesCurrentUserCanRemove?.includes(role)
            }
          />
        )) ?? []),
        !hideInputControls &&
          rolesCurrentUserCanAndMayWantToAdd &&
          rolesCurrentUserCanAndMayWantToAdd.length > 0 && (
            <AddUserRole
              userId={entity.uuid}
              roles={rolesCurrentUserCanAndMayWantToAdd}
            />
          ),
      ].filter(isTruthy)}
      extra={
        !hideInputControls &&
        [
          "isAuthorizedToDeleteUser" in entity &&
            entity.isAuthorizedToDeleteUser && (
              <DeleteUser userId={entity.uuid} />
            ),
        ].filter(isTruthy)
      }
    >
      <ContactInformation contact={entity.contact} />
      {entity.representedInstitutions.edges.length > 0 && (
        <div>
          Represents{" "}
          {renderRepresentedInstitutionList(
            entity.representedInstitutions,
            entity.uuid,
            hideInputControls,
          )}
          {"pendingRepresentedInstitutions" in entity &&
            entity.pendingRepresentedInstitutions &&
            entity.pendingRepresentedInstitutions.edges.length > 0 && (
              <>
                {" "}
                and was asked to represent{" "}
                {renderRepresentedInstitutionList(
                  entity.pendingRepresentedInstitutions,
                  entity.uuid,
                  hideInputControls,
                )}
              </>
            )}
        </div>
      )}
      {"userDevelopedMethods" in entity &&
        entity.userDevelopedMethods.edges.length > 0 && (
          <div>
            Developed the{" "}
            {pluralize(entity.userDevelopedMethods.edges.length, "method")}{" "}
            {renderUserDevelopedMethodList(
              entity.userDevelopedMethods,
              entity.uuid,
              hideInputControls,
            )}
            {"pendingUserDevelopedMethods" in entity &&
              entity.pendingUserDevelopedMethods &&
              entity.pendingUserDevelopedMethods.edges.length > 0 && (
                <>
                  {" "}
                  and was asked whether he/she/they developed the{" "}
                  {pluralize(
                    entity.userDevelopedMethods.edges.length,
                    "method",
                  )}{" "}
                  {renderUserDevelopedMethodList(
                    entity.pendingUserDevelopedMethods,
                    entity.uuid,
                    hideInputControls,
                  )}
                </>
              )}
          </div>
        )}
    </EntitySummary>
  );
}
