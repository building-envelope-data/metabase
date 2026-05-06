import {
  UserPartialFragment,
  UsersPartialFragment,
} from "../../queries/users.generated";
import paths from "../../paths";
import { UserRoleTag } from "./UserRoleTag";
import InlineList from "../InlineList";
import EntityLink from "../entities/EntityLink";
import ContactInformation from "../ContactInformation";
import EntitySummary from "../entities/EntitySummary";
import { asReadonlyMixed, isTruthy, pluralize } from "../../lib/array";
import DeleteUser from "./DeleteUser";
import RemoveInstitutionRepresentative from "../institutions/RemoveInstitutionRepresentative";
import AddUserRole from "./AddUserRole";
import EnumTag from "../EnumTag";

export default function UserSummary({
  entity,
}: {
  entity: UsersPartialFragment | UserPartialFragment;
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
              "rolesCurrentUserCanRemove" in entity &&
              entity.rolesCurrentUserCanRemove?.includes(role)
            }
          />
        )) ?? []),
        rolesCurrentUserCanAndMayWantToAdd &&
          rolesCurrentUserCanAndMayWantToAdd.length > 0 && (
            <AddUserRole
              userId={entity.uuid}
              roles={rolesCurrentUserCanAndMayWantToAdd}
            />
          ),
      ].filter(isTruthy)}
      extra={[
        "isAuthorizedToDeleteUser" in entity &&
          entity.isAuthorizedToDeleteUser && (
            <DeleteUser userId={entity.uuid} />
          ),
      ].filter(isTruthy)}
    >
      <ContactInformation contact={entity.contact} />
      {entity.representedInstitutions.edges.length > 0 && (
        <div>
          Represents{" "}
          <InlineList
            items={asReadonlyMixed(entity.representedInstitutions.edges)}
            renderItem={(edge) => (
              <span key={edge.node.id}>
                <EntityLink entity={edge.node} route={paths.institution} />{" "}
                <EnumTag color="grey" variant="outlined">
                  {edge.role}
                </EnumTag>
                {"isAuthorizedToRemoveEdge" in edge &&
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
      {"developedMethods" in entity &&
        entity.developedMethods.edges.length > 0 && (
          <div>
            Developed the{" "}
            {pluralize(entity.developedMethods.edges.length, "method")}{" "}
            <InlineList
              items={entity.developedMethods.edges}
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
