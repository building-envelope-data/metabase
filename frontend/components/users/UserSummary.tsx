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

export default function UserSummary({
  entity,
  hideExtra = false,
}: {
  entity: UsersPartialFragment | UserPartialFragment;
  hideExtra?: boolean;
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
              !hideExtra &&
              "rolesCurrentUserCanRemove" in entity &&
              entity.rolesCurrentUserCanRemove?.includes(role)
            }
          />
        )) ?? []),
        !hideExtra &&
          rolesCurrentUserCanAndMayWantToAdd &&
          rolesCurrentUserCanAndMayWantToAdd.length > 0 && (
            <AddUserRole
              userId={entity.uuid}
              roles={rolesCurrentUserCanAndMayWantToAdd}
            />
          ),
      ].filter(isTruthy)}
      extra={
        !hideExtra &&
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
          <InlineList
            items={asReadonlyMixed(entity.representedInstitutions.edges)}
            renderItem={(edge) => (
              <span key={edge.node.id}>
                <Space>
                  <EntityLink entity={edge.node} route={paths.institution} />
                  <EnumTag color="grey" variant="outlined">
                    {edge.role}
                  </EnumTag>
                </Space>
                {!hideExtra &&
                  "isAuthorizedToRemoveEdge" in edge &&
                  edge.isAuthorizedToRemoveEdge && (
                    <RemoveInstitutionRepresentative
                      institutionId={edge.node.uuid}
                      userId={entity.uuid}
                    />
                  )}
              </span>
            )}
          />
        </div>
      )}
      {"userDevelopedMethods" in entity &&
        entity.userDevelopedMethods.edges.length > 0 && (
          <div>
            Developed the{" "}
            {pluralize(entity.userDevelopedMethods.edges.length, "method")}{" "}
            <InlineList
              items={entity.userDevelopedMethods.edges}
              renderItem={(item) => (
                <EntityLink
                  key={item.node.id}
                  entity={item.node}
                  route={paths.method}
                />
              )}
            />
          </div>
        )}
    </EntitySummary>
  );
}
