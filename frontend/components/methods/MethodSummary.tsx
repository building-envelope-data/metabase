import { Typography } from "antd";
import { asReadonlyMixed, isTruthy } from "../../lib/array";
import paths from "../../paths";
import Manager from "../Manager";
import OpenEndedDateTimeRangeX from "../OpenEndedDateTimeRangeX";
import EntitySummary from "../entities/EntitySummary";
import Reference from "../Reference";
import UpdateMethod from "./UpdateMethod";
import {
  MethodPartialFragment,
  MethodsPartialFragment,
} from "../../queries/methods.generated";
import EntityLink from "../entities/EntityLink";
import InlineList from "../InlineList";
import RemoveInstitutionMethodDeveloper from "./RemoveInstitutionMethodDeveloper";
import RemoveUserMethodDeveloper from "./RemoveUserMethodDeveloper";
import AddInstitutionMethodDeveloper from "./AddInstitutionMethodDeveloper";
import AddUserMethodDeveloper from "./AddUserMethodDeveloper";
import { Scalars } from "../../__generated__/graphql";
import JsonView from "../JsonView";
import EnumTag from "../EnumTag";

const renderDeveloperList = (
  developers:
    | NonNullable<MethodsPartialFragment["developers"]>
    | NonNullable<MethodPartialFragment["developers"]>
    | NonNullable<MethodPartialFragment["pendingDevelopers"]>,
  methodId: Scalars["Uuid"]["output"],
  hideExtra: boolean | undefined,
) => (
  <InlineList
    items={asReadonlyMixed(developers.edges)}
    renderItem={(edge) => (
      <span key={edge.node.id}>
        <EntityLink
          entity={edge.node}
          route={
            edge.node.__typename == "Institution"
              ? paths.institution
              : paths.user
          }
        />
        {!hideExtra &&
          "isAuthorizedToRemoveEdge" in edge &&
          edge.isAuthorizedToRemoveEdge &&
          (edge.node.__typename == "Institution" ? (
            <RemoveInstitutionMethodDeveloper
              methodId={methodId}
              institutionId={edge.node.uuid}
            />
          ) : (
            <RemoveUserMethodDeveloper
              methodId={methodId}
              userId={edge.node.uuid}
            />
          ))}
      </span>
    )}
  />
);

export default function MethodSummary({
  entity,
  hideExtra = false,
}: {
  entity: MethodsPartialFragment | MethodPartialFragment;
  hideExtra?: boolean;
}) {
  const dateTimeRanges = [
    (entity.validity?.from || entity.validity?.to) && (
      <div key="validity">
        Valid <OpenEndedDateTimeRangeX range={entity.validity} />
      </div>
    ),
    (entity.availability?.from || entity.availability?.to) && (
      <div key="availability">
        Available <OpenEndedDateTimeRangeX range={entity.availability} />
      </div>
    ),
  ].filter(isTruthy);

  return (
    <EntitySummary
      entity={entity}
      route={paths.method}
      tags={entity.categories.map((x) => (
        <EnumTag key={x} color="magenta">
          {x}
        </EnumTag>
      ))}
      extra={
        !hideExtra &&
        [
          "isAuthorizedToUpdateNode" in entity &&
            entity.isAuthorizedToUpdateNode && (
              <UpdateMethod key="updateMethod" method={entity} />
            ),
        ].filter(isTruthy)
      }
    >
      {dateTimeRanges.length > 0 && <div>{dateTimeRanges}</div>}
      {(entity.parameters.length > 0 || entity.sources.length > 0) && (
        <div>
          {entity.parameters.length > 0 && (
            <div>
              Parameterized by{" "}
              <InlineList
                items={entity.parameters}
                renderItem={(item) => (
                  <span key={item.name}>
                    <code>{item.name}</code> of type{" "}
                    <JsonView inline data={item.type} />
                  </span>
                )}
              />
            </div>
          )}
          {entity.sources.length > 0 && (
            <div>
              Operates on the sources{" "}
              <InlineList
                items={entity.sources}
                renderItem={(item) => (
                  <span key={item.name}>
                    &ldquo;{item.name}&rdquo; ({item.description})
                  </span>
                )}
              />
            </div>
          )}
        </div>
      )}
      {entity.calculationLocator && (
        <div>
          Calculate on{" "}
          <Typography.Link href={entity.calculationLocator} target="_blank">
            {entity.calculationLocator}
          </Typography.Link>
        </div>
      )}
      {entity.developers.edges.length > 0 && (
        <div>
          Developed by{" "}
          {renderDeveloperList(entity.developers, entity.uuid, hideExtra)}
          {"pendingDevelopers" in entity &&
            entity.pendingDevelopers &&
            entity.pendingDevelopers.edges.length > 0 && (
              <>
                Awaiting verification of
                {renderDeveloperList(
                  entity.pendingDevelopers,
                  entity.uuid,
                  hideExtra,
                )}
              </>
            )}
          {!hideExtra &&
            "isAuthorizedToAddInstitutionEdge" in entity.developers &&
            entity.developers.isAuthorizedToAddInstitutionEdge && (
              <AddInstitutionMethodDeveloper methodId={entity.uuid} />
            )}
          {!hideExtra &&
            "isAuthorizedToAddUserEdge" in entity.developers &&
            entity.developers.isAuthorizedToAddUserEdge && (
              <AddUserMethodDeveloper methodId={entity.uuid} />
            )}
        </div>
      )}
      {entity.reference && <Reference data={entity.reference} />}
      {"manager" in entity && (
        <div>
          <Manager data={entity.manager.node} />
        </div>
      )}
    </EntitySummary>
  );
}
