import { Space, Tag } from "antd";
import { isTruthy } from "../../lib/array";
import {
  ComponentsPartialFragment,
  ComponentPartialFragment,
} from "../../queries/components.generated";
import EntitySummary from "../entities/EntitySummary";
import JsonView from "../JsonView";
import Manager from "../Manager";
import UpdateComponent from "./UpdateComponent";
import paths from "../../paths";
import EntityLink from "../entities/EntityLink";
import OpenEndedDateTimeRangeX from "../OpenEndedDateTimeRangeX";
import RemoveComponentManufacturer from "./RemoveComponentManufacturer";
import InlineList from "../InlineList";
import { Scalars } from "../../__generated__/graphql";
import AddComponentManufacturer from "./AddComponentManufacturer";
import AddVariantOfComponent from "./AddVariantOfComponent";
import RemoveComponentVariant from "./RemoveComponentVariant";
import AddConcretizationOfComponent from "./AddConcretizationOfComponent";
import AddGeneralizationOfComponent from "./AddGeneralizationOfComponent";
import RemoveComponentGeneralization from "./RemoveComponentGeneralization";
import AddPartOfComponent from "./AddPartOfComponent";
import RemoveComponentAssembly from "./RemoveComponentAssembly";
import UpdateComponentAssembly from "./UpdateComponentAssembly";
import AddAssembledOfComponent from "./AddAssembledOfComponent";
import EnumTag from "../EnumTag";
import { humanize } from "../../lib/string";
import DescriptionOrReference from "../DescriptionOrReference";

const renderManufacturerList = (
  manufacturers:
    | NonNullable<ComponentPartialFragment["manufacturers"]>
    | NonNullable<ComponentPartialFragment["pendingManufacturers"]>,
  componentId: Scalars["Uuid"]["output"],
  hideInputControls: boolean | undefined,
) => (
  <InlineList
    items={manufacturers.edges}
    renderItem={(edge) => (
      <span key={edge.node.id}>
        <EntityLink entity={edge.node} route={paths.institution} />
        {!hideInputControls && edge.isAuthorizedToRemoveEdge && (
          <RemoveComponentManufacturer
            componentId={componentId}
            institutionId={edge.node.uuid}
          />
        )}
      </span>
    )}
  />
);

export default function ComponentSummary({
  entity,
  extra,
  hideInputControls = false,
}: {
  entity: ComponentsPartialFragment | ComponentPartialFragment;
  extra?: React.ReactNode;
  hideInputControls?: boolean;
}) {
  const reflexiveRelations = [
    "assembledOf" in entity &&
      (entity.assembledOf.edges.length > 0 ||
        (!hideInputControls && entity.assembledOf.isAuthorizedToAddEdge)) && (
        <div>
          Assembled of{" "}
          <InlineList
            items={entity.assembledOf.edges}
            renderItem={(edge) => (
              <span key={edge.node.id}>
                <Space>
                  <EntityLink entity={edge.node} route={paths.component} />
                  {edge.index && <Tag color="purple">Layer {edge.index}</Tag>}
                  {edge.primeSurface && (
                    <Tag color="volcano">
                      Prime Surface "{humanize(edge.primeSurface, "all-upper")}"
                    </Tag>
                  )}
                </Space>
                {!hideInputControls && edge.isAuthorizedToUpdateEdge && (
                  <UpdateComponentAssembly
                    assembledComponent={{
                      uuid: entity.uuid,
                      name: entity.name,
                    }}
                    partComponent={{
                      uuid: edge.node.uuid,
                      name: edge.node.name,
                    }}
                    index={edge.index}
                    primeSurface={edge.primeSurface}
                  />
                )}
                {!hideInputControls && edge.isAuthorizedToRemoveEdge && (
                  <RemoveComponentAssembly
                    assembledComponentId={entity.uuid}
                    partComponentId={edge.node.uuid}
                  />
                )}
              </span>
            )}
          />
          {!hideInputControls && entity.assembledOf.isAuthorizedToAddEdge && (
            <AddPartOfComponent assembledComponentId={entity.uuid} />
          )}
        </div>
      ),
    "partOf" in entity &&
      (entity.partOf.edges.length > 0 ||
        (!hideInputControls && entity.partOf.isAuthorizedToAddEdge)) && (
        <div>
          Part of{" "}
          <InlineList
            items={entity.partOf.edges}
            renderItem={(edge) => (
              <span key={edge.node.id}>
                <Space>
                  <EntityLink entity={edge.node} route={paths.component} />
                  {edge.index && <Tag color="purple">Layer {edge.index}</Tag>}
                  {edge.primeSurface && (
                    <Tag color="volcano">
                      Prime Surface "{humanize(edge.primeSurface, "all-upper")}"
                    </Tag>
                  )}
                </Space>
                {!hideInputControls && edge.isAuthorizedToUpdateEdge && (
                  <UpdateComponentAssembly
                    assembledComponent={{
                      uuid: edge.node.uuid,
                      name: edge.node.name,
                    }}
                    partComponent={{
                      uuid: entity.uuid,
                      name: entity.name,
                    }}
                    index={edge.index}
                    primeSurface={edge.primeSurface}
                  />
                )}
                {!hideInputControls && edge.isAuthorizedToRemoveEdge && (
                  <RemoveComponentAssembly
                    assembledComponentId={edge.node.uuid}
                    partComponentId={entity.uuid}
                  />
                )}
              </span>
            )}
          />
          {!hideInputControls && entity.partOf.isAuthorizedToAddEdge && (
            <AddAssembledOfComponent partComponentId={entity.uuid} />
          )}
        </div>
      ),
    "variantOf" in entity &&
      (entity.variantOf.edges.length > 0 ||
        (!hideInputControls && entity.variantOf.isAuthorizedToAddEdge)) && (
        <div>
          Variant of{" "}
          <InlineList
            items={entity.variantOf.edges}
            renderItem={(edge) => (
              <span key={edge.node.id}>
                <EntityLink entity={edge.node} route={paths.component} />
                {!hideInputControls && edge.isAuthorizedToRemoveEdge && (
                  <RemoveComponentVariant
                    oneComponentId={entity.uuid}
                    otherComponentId={edge.node.uuid}
                  />
                )}
              </span>
            )}
          />
          {!hideInputControls && entity.variantOf.isAuthorizedToAddEdge && (
            <AddVariantOfComponent componentId={entity.uuid} />
          )}
        </div>
      ),
    "generalizationOf" in entity &&
      (entity.generalizationOf.edges.length > 0 ||
        (!hideInputControls &&
          entity.generalizationOf.isAuthorizedToAddEdge)) && (
        <div>
          Generalization of{" "}
          <InlineList
            items={entity.generalizationOf.edges}
            renderItem={(edge) => (
              <span key={edge.node.id}>
                <EntityLink entity={edge.node} route={paths.component} />
                {!hideInputControls && edge.isAuthorizedToRemoveEdge && (
                  <RemoveComponentGeneralization
                    generalComponentId={entity.uuid}
                    concreteComponentId={edge.node.uuid}
                  />
                )}
              </span>
            )}
          />
          {!hideInputControls &&
            entity.generalizationOf.isAuthorizedToAddEdge && (
              <AddConcretizationOfComponent generalComponentId={entity.uuid} />
            )}
        </div>
      ),
    "concretizationOf" in entity &&
      (entity.concretizationOf.edges.length > 0 ||
        (!hideInputControls &&
          entity.concretizationOf.isAuthorizedToAddEdge)) && (
        <div>
          Concretization of{" "}
          <InlineList
            items={entity.concretizationOf.edges}
            renderItem={(edge) => (
              <span key={edge.node.id}>
                <EntityLink entity={edge.node} route={paths.component} />
                {!hideInputControls && edge.isAuthorizedToRemoveEdge && (
                  <RemoveComponentGeneralization
                    generalComponentId={edge.node.uuid}
                    concreteComponentId={entity.uuid}
                  />
                )}
              </span>
            )}
          />
          {!hideInputControls &&
            entity.concretizationOf.isAuthorizedToAddEdge && (
              <AddGeneralizationOfComponent concreteComponentId={entity.uuid} />
            )}
        </div>
      ),
  ].filter(isTruthy);

  return (
    <EntitySummary
      entity={entity}
      route={paths.component}
      tags={entity.categories.map((x) => (
        <EnumTag>{x}</EnumTag>
      ))}
      extra={[
        extra,
        !hideInputControls &&
          "isAuthorizedToUpdateNode" in entity &&
          entity.isAuthorizedToUpdateNode && (
            <UpdateComponent key="UpdateComponent" component={entity} />
          ),
      ].filter(isTruthy)}
    >
      {(entity.availability?.from || entity.availability?.to) && (
        <div>
          Available <OpenEndedDateTimeRangeX range={entity.availability} />
        </div>
      )}
      <div>
        Manufactured by{" "}
        {"pendingManufacturers" in entity ? (
          <>
            {renderManufacturerList(
              entity.manufacturers,
              entity.uuid,
              hideInputControls,
            )}
            {entity.pendingManufacturers &&
              entity.pendingManufacturers.edges.length > 0 && (
                <>
                  {" "}
                  and awaiting verification of
                  {renderManufacturerList(
                    entity.pendingManufacturers,
                    entity.uuid,
                    hideInputControls,
                  )}
                </>
              )}
            {!hideInputControls &&
              entity.manufacturers.isAuthorizedToAddEdge && (
                <AddComponentManufacturer componentId={entity.uuid} />
              )}
          </>
        ) : (
          <InlineList
            items={entity.manufacturers.edges}
            renderItem={(edge) => (
              <span key={edge.node.id}>
                <EntityLink entity={edge.node} route={paths.institution} />
              </span>
            )}
          />
        )}
      </div>
      {reflexiveRelations.length > 0 && <div>{reflexiveRelations}</div>}
      {"prime" in entity && entity.prime?.surface && (
        <DescriptionOrReference
          title="Prime Surface"
          data={entity.prime.surface}
        />
      )}
      {"prime" in entity && entity.prime?.direction && (
        <DescriptionOrReference
          title="Prime Direction"
          data={entity.prime.direction}
        />
      )}
      {"switchableLayers" in entity && entity.switchableLayers && (
        <DescriptionOrReference
          title="Switchable Layers"
          data={entity.switchableLayers}
        />
      )}
      {"extras" in entity && entity.extras != null && (
        <div>
          <JsonView data={entity.extras} />
        </div>
      )}
      {"manager" in entity && entity.manager?.node && (
        <div>
          <Manager data={entity.manager.node} />
        </div>
      )}
    </EntitySummary>
  );
}
