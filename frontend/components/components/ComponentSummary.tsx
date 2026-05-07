import { Descriptions, Space, Tag } from "antd";
import { isTruthy } from "../../lib/array";
import {
  ComponentsPartialFragment,
  ComponentPartialFragment,
} from "../../queries/components.generated";
import EntitySummary from "../entities/EntitySummary";
import JsonViewer from "../JsonViewer";
import Manager from "../Manager";
import UpdateComponent from "./UpdateComponent";
import paths from "../../paths";
import EntityLink from "../entities/EntityLink";
import OpenEndedDateTimeRangeX from "../OpenEndedDateTimeRangeX";
import { RemoveComponentManufacturer } from "./RemoveComponentManufacturer";
import InlineList from "../InlineList";
import { Scalars } from "../../__generated__/graphql";
import AddComponentManufacturer from "./AddComponentManufacturer";
import AddVariantOfComponent from "./AddVariantOfComponent";
import { RemoveComponentVariant } from "./RemoveComponentVariant";
import AddConcretizationOfComponent from "./AddConcretizationOfComponent";
import AddGeneralizationOfComponent from "./AddGeneralizationOfComponent";
import { RemoveComponentGeneralization } from "./RemoveComponentGeneralization";
import AddPartOfComponent from "./AddPartOfComponent";
import { RemoveComponentAssembly } from "./RemoveComponentAssembly";
import UpdateComponentAssembly from "./UpdateComponentAssembly";
import AddAssembledOfComponent from "./AddAssembledOfComponent";
import EnumTag from "../EnumTag";

const renderManufacturerList = (
  manufacturers:
    | NonNullable<ComponentPartialFragment["manufacturers"]>
    | NonNullable<ComponentPartialFragment["pendingManufacturers"]>,
  componentId: Scalars["Uuid"]["output"],
  hideExtra: boolean | undefined,
) => (
  <InlineList
    items={manufacturers.edges}
    renderItem={(edge) => (
      <span key={edge.node.id}>
        <EntityLink entity={edge.node} route={paths.institution} />{" "}
        {!hideExtra && edge.isAuthorizedToRemoveEdge && (
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
  hideExtra = false,
}: {
  entity: ComponentsPartialFragment | ComponentPartialFragment;
  hideExtra?: boolean;
}) {
  return (
    <EntitySummary
      entity={entity}
      route={paths.component}
      tags={entity.categories.map((x) => (
        <EnumTag>{x}</EnumTag>
      ))}
      extra={
        !hideExtra &&
        [
          "isAuthorizedToUpdateNode" in entity &&
            entity.isAuthorizedToUpdateNode && (
              <UpdateComponent key="UpdateComponent" component={entity} />
            ),
        ].filter(isTruthy)
      }
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
              hideExtra,
            )}
            {entity.pendingManufacturers &&
              entity.pendingManufacturers.edges.length > 0 && (
                <>
                  Awaiting verification of
                  {renderManufacturerList(
                    entity.pendingManufacturers,
                    entity.uuid,
                    hideExtra,
                  )}
                </>
              )}
            {!hideExtra && entity.manufacturers.isAuthorizedToAddEdge && (
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
      {"assembledOf" in entity &&
        (entity.assembledOf.edges.length > 0 ||
          (!hideExtra && entity.assembledOf.isAuthorizedToAddEdge)) && (
          <div>
            Assembled of{" "}
            <InlineList
              items={entity.assembledOf.edges}
              renderItem={(edge) => (
                <span key={edge.node.id}>
                  <EntityLink entity={edge.node} route={paths.component} />{" "}
                  {edge.index && <Tag color="purple">Layer {edge.index}</Tag>}
                  {edge.primeSurface && (
                    <Tag color="volcano">
                      Prime Surface "{edge.primeSurface}"
                    </Tag>
                  )}
                  {!hideExtra && edge.isAuthorizedToUpdateEdge && (
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
                  {!hideExtra && edge.isAuthorizedToRemoveEdge && (
                    <RemoveComponentAssembly
                      assembledComponentId={entity.uuid}
                      partComponentId={edge.node.uuid}
                    />
                  )}
                </span>
              )}
            />
            {!hideExtra && entity.assembledOf.isAuthorizedToAddEdge && (
              <AddPartOfComponent assembledComponentId={entity.uuid} />
            )}
          </div>
        )}
      {"partOf" in entity &&
        (entity.partOf.edges.length > 0 ||
          (!hideExtra && entity.partOf.isAuthorizedToAddEdge)) && (
          <div>
            Part of{" "}
            <InlineList
              items={entity.partOf.edges}
              renderItem={(edge) => (
                <span key={edge.node.id}>
                  <Space>
                    <EntityLink entity={edge.node} route={paths.component} />
                    <span>
                      {edge.index && (
                        <Tag color="purple">Layer {edge.index}</Tag>
                      )}
                      {edge.primeSurface && (
                        <Tag color="volcano">
                          Prime Surface "{edge.primeSurface}"
                        </Tag>
                      )}
                    </span>
                  </Space>
                  {!hideExtra && edge.isAuthorizedToUpdateEdge && (
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
                  {!hideExtra && edge.isAuthorizedToRemoveEdge && (
                    <RemoveComponentAssembly
                      assembledComponentId={edge.node.uuid}
                      partComponentId={entity.uuid}
                    />
                  )}
                </span>
              )}
            />
            {!hideExtra && entity.partOf.isAuthorizedToAddEdge && (
              <AddAssembledOfComponent partComponentId={entity.uuid} />
            )}
          </div>
        )}
      {"variantOf" in entity &&
        (entity.variantOf.edges.length > 0 ||
          (!hideExtra && entity.variantOf.isAuthorizedToAddEdge)) && (
          <div>
            Variant of{" "}
            <InlineList
              items={entity.variantOf.edges}
              renderItem={(edge) => (
                <span key={edge.node.id}>
                  <EntityLink entity={edge.node} route={paths.component} />{" "}
                  {!hideExtra && edge.isAuthorizedToRemoveEdge && (
                    <RemoveComponentVariant
                      oneComponentId={entity.uuid}
                      otherComponentId={edge.node.uuid}
                    />
                  )}
                </span>
              )}
            />
            {!hideExtra && entity.variantOf.isAuthorizedToAddEdge && (
              <AddVariantOfComponent componentId={entity.uuid} />
            )}
          </div>
        )}
      {"generalizationOf" in entity &&
        (entity.generalizationOf.edges.length > 0 ||
          (!hideExtra && entity.generalizationOf.isAuthorizedToAddEdge)) && (
          <div>
            Generalization of{" "}
            <InlineList
              items={entity.generalizationOf.edges}
              renderItem={(edge) => (
                <span key={edge.node.id}>
                  <EntityLink entity={edge.node} route={paths.component} />{" "}
                  {!hideExtra && edge.isAuthorizedToRemoveEdge && (
                    <RemoveComponentGeneralization
                      generalComponentId={entity.uuid}
                      concreteComponentId={edge.node.uuid}
                    />
                  )}
                </span>
              )}
            />
            {!hideExtra && entity.generalizationOf.isAuthorizedToAddEdge && (
              <AddConcretizationOfComponent generalComponentId={entity.uuid} />
            )}
          </div>
        )}
      {"concretizationOf" in entity &&
        (entity.concretizationOf.edges.length > 0 ||
          (!hideExtra && entity.concretizationOf.isAuthorizedToAddEdge)) && (
          <div>
            Concretization of{" "}
            <InlineList
              items={entity.concretizationOf.edges}
              renderItem={(edge) => (
                <span key={edge.node.id}>
                  <EntityLink entity={edge.node} route={paths.component} />{" "}
                  {!hideExtra && edge.isAuthorizedToRemoveEdge && (
                    <RemoveComponentGeneralization
                      generalComponentId={edge.node.uuid}
                      concreteComponentId={entity.uuid}
                    />
                  )}
                </span>
              )}
            />
            {!hideExtra && entity.concretizationOf.isAuthorizedToAddEdge && (
              <AddGeneralizationOfComponent concreteComponentId={entity.uuid} />
            )}
          </div>
        )}
      {"manager" in entity && entity.manager?.node && (
        <div>
          <Manager data={entity.manager.node} />
        </div>
      )}
      {"extras" in entity && entity.extras != null && (
        <div>
          <JsonViewer data={entity.extras} />
        </div>
      )}
      <Descriptions size="small" column={1}>
        {"prime" in entity && entity.prime?.surface && (
          <Descriptions.Item label="Prime Surface">
            {entity.prime?.surface?.description}{" "}
            {entity.prime?.surface?.reference?.title}
          </Descriptions.Item>
        )}
        {"prime" in entity && entity.prime?.direction && (
          <Descriptions.Item label="Prime Direction">
            {entity.prime?.direction?.description}{" "}
            {entity.prime?.direction?.reference?.title}
          </Descriptions.Item>
        )}
        {"switchableLayers" in entity && entity.switchableLayers && (
          <Descriptions.Item label="Switchable Layers">
            {entity.switchableLayers?.description}{" "}
            {entity.switchableLayers?.reference?.title}
          </Descriptions.Item>
        )}
      </Descriptions>
    </EntitySummary>
  );
}
