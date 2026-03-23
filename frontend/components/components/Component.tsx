import { useQuery } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import { ComponentDocument } from "../../queries/components.generated";
import {
  Skeleton,
  Result,
  Descriptions,
  Tag,
  List,
  Row,
  Col,
  Space,
} from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import { ReactNode } from "react";
import paths from "../../paths";
import Link from "next/link";
import OpenEndedDateTimeRangeX from "../OpenEndedDateTimeRangeX";
import AddPartOfComponent from "./AddPartOfComponent";
import AddAssembledOfComponent from "./AddAssembledOfComponent";
import UpdateComponentAssembly from "./UpdateComponentAssembly";
import AddVariantOfComponent from "./AddVariantOfComponent";
import AddConcretizationOfComponent from "./AddConcretizationOfComponent";
import AddGeneralizationOfComponent from "./AddGeneralizationOfComponent";
import UpdateComponent from "./UpdateComponent";
import AddComponentManufacturer from "./AddComponentManufacturer";
import { RemoveComponentManufacturer } from "./RemoveComponentManufacturer";
import { RemoveComponentAssembly } from "./RemoveComponentAssembly";
import { RemoveComponentGeneralization } from "./RemoveComponentGeneralization";
import { RemoveComponentVariant } from "./RemoveComponentVariant";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";

interface ComponentProps {
  componentId: Scalars["Uuid"]["input"];
};

export default function Component({ componentId }: ComponentProps) {
  const { loading, error, data } = useQuery(ComponentDocument, {
    variables: {
      uuid: componentId,
    },
  });
  useQueryHandler({ error });
  const component = data?.component;

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!component) {
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
      <PageHeader
        title={[
          component.name,
          component.abbreviation == null ? null : `(${component.abbreviation})`,
        ]
          .filter((x) => x != null)
          .join(" ")}
        subTitle={component.description}
        tags={component.categories.map((x) => (
          <Tag key={x} color="magenta">
            {x}
          </Tag>
        ))}
        extra={
          component.isAuthorizedToUpdateNode
            ? [<UpdateComponent key="UpdateComponent" component={component} />]
            : []
        }
        backIcon={false}
      >
        <Descriptions size="small" column={1}>
          <Descriptions.Item label="UUID">{component.uuid}</Descriptions.Item>
          <Descriptions.Item label="Available">
            <OpenEndedDateTimeRangeX range={component.availability} />
          </Descriptions.Item>
          {component.prime?.surface && (
            <Descriptions.Item label="Prime Surface">
              {component.prime?.surface?.description}{" "}
              {component.prime?.surface?.reference?.title}
            </Descriptions.Item>
          )}
          {component.prime?.direction && (
            <Descriptions.Item label="Prime Direction">
              {component.prime?.direction?.description}{" "}
              {component.prime?.direction?.reference?.title}
            </Descriptions.Item>
          )}
          {component.switchableLayers && (
            <Descriptions.Item label="Switchable Layers">
              {component.switchableLayers?.description}{" "}
              {component.switchableLayers?.reference?.title}
            </Descriptions.Item>
          )}
          {component.extras != undefined && (
            <Descriptions.Item label="Extras">
              {JSON.stringify(component.extras, null, "\t")}
            </Descriptions.Item>
          )}
          <Descriptions.Item label="Manager">
            <Link href={paths.institution(component.manager.node.uuid)}>
              {component.manager.node.name}
            </Link>
          </Descriptions.Item>
        </Descriptions>
      </PageHeader>
      <Space orientation="vertical" style={{ display: "flex" }}>
        <Row gutter={[16, 16]}>
          <Col flex={1}>
            <List
              header="Manufacturers"
              bordered={true}
              size="small"
              footer={
                component.manufacturers.isAuthorizedToAddEdge && (
                  <AddComponentManufacturer componentId={component.uuid} />
                )
              }
            >
              {component.manufacturers.edges.map((x) => (
                <List.Item
                  key={x.node.uuid}
                  actions={([] as ReactNode[]).concat(
                    x.isAuthorizedToRemoveEdge
                      ? [
                          <RemoveComponentManufacturer
                            key={`RemoveComponentManufacturer-${x.node.uuid}`}
                            componentId={component.uuid}
                            institutionId={x.node.uuid}
                          />,
                        ]
                      : [],
                  )}
                >
                  <List.Item.Meta
                    title={
                      <Link href={paths.institution(x.node.uuid)}>
                        {x.node.name}
                      </Link>
                    }
                    description={x.node.description}
                  />
                </List.Item>
              ))}
              {component.pendingManufacturers?.edges.map((x) => (
                <List.Item
                  key={x.node.uuid}
                  actions={([] as ReactNode[]).concat(
                    x.isAuthorizedToRemoveEdge
                      ? [
                          <RemoveComponentManufacturer
                            key={`RemoveComponentManufacturer-${x.node.uuid}`}
                            componentId={component.uuid}
                            institutionId={x.node.uuid}
                          />,
                        ]
                      : [],
                  )}
                >
                  <List.Item.Meta
                    title={
                      <Link href={paths.institution(x.node.uuid)}>
                        {x.node.name} (Pending)
                      </Link>
                    }
                  />
                </List.Item>
              ))}
            </List>
          </Col>
        </Row>
        {(component.assembledOf.edges.length >= 1 ||
          component.assembledOf.isAuthorizedToAddEdge ||
          component.partOf.edges.length >= 1 ||
          component.partOf.isAuthorizedToAddEdge) && (
          <Row gutter={[16, 16]}>
            <Col flex={1}>
              {(component.assembledOf.edges.length >= 1 ||
                component.assembledOf.isAuthorizedToAddEdge) && (
                <List
                  header="Assembled Of"
                  bordered={true}
                  size="small"
                  footer={
                    component.assembledOf.isAuthorizedToAddEdge && (
                      <AddPartOfComponent
                        assembledComponentId={component.uuid}
                      />
                    )
                  }
                >
                  {component.assembledOf.edges.map((x) => (
                    <List.Item
                      key={x.node.uuid}
                      actions={([] as ReactNode[])
                        .concat(
                          x.isAuthorizedToUpdateEdge
                            ? [
                                <UpdateComponentAssembly
                                  key={`UpdateComponentAssembly-${component.uuid}-${x.node.uuid}`}
                                  assembledComponent={{
                                    uuid: component.uuid,
                                    name: component.name,
                                  }}
                                  partComponent={{
                                    uuid: x.node.uuid,
                                    name: x.node.name,
                                  }}
                                  index={x.index}
                                  primeSurface={x.primeSurface}
                                />,
                              ]
                            : [],
                        )
                        .concat(
                          x.isAuthorizedToRemoveEdge
                            ? [
                                <RemoveComponentAssembly
                                  key={`RemoveComponentAssembly-${component.uuid}-${x.node.uuid}`}
                                  assembledComponentId={component.uuid}
                                  partComponentId={x.node.uuid}
                                />,
                              ]
                            : [],
                        )}
                    >
                      <List.Item.Meta
                        title={
                          <Space>
                            <Link href={paths.component(x.node.uuid)}>
                              {x.node.name}
                            </Link>
                            <div>
                              {x.index && (
                                <Tag color="purple">Layer {x.index}</Tag>
                              )}
                              {x.primeSurface && (
                                <Tag color="volcano">
                                  Prime Surface {x.primeSurface}
                                </Tag>
                              )}
                            </div>
                          </Space>
                        }
                        description={x.node.description}
                      />
                    </List.Item>
                  ))}
                </List>
              )}
            </Col>
            <Col flex={1}>
              {(component.partOf.edges.length >= 1 ||
                component.partOf.isAuthorizedToAddEdge) && (
                <List
                  header="Part Of"
                  bordered={true}
                  size="small"
                  footer={
                    component.partOf.isAuthorizedToAddEdge && (
                      <AddAssembledOfComponent
                        partComponentId={component.uuid}
                      />
                    )
                  }
                >
                  {component.partOf.edges.map((x) => (
                    <List.Item
                      key={x.node.uuid}
                      actions={([] as ReactNode[])
                        .concat(
                          x.isAuthorizedToUpdateEdge
                            ? [
                                <UpdateComponentAssembly
                                  key={`UpdateComponentAssembly-${x.node.uuid}-${component.uuid}`}
                                  assembledComponent={{
                                    uuid: x.node.uuid,
                                    name: x.node.name,
                                  }}
                                  partComponent={{
                                    uuid: component.uuid,
                                    name: component.name,
                                  }}
                                  index={x.index}
                                  primeSurface={x.primeSurface}
                                />,
                              ]
                            : [],
                        )
                        .concat(
                          x.isAuthorizedToRemoveEdge
                            ? [
                                <RemoveComponentAssembly
                                  key={`removeComponentAssembly-${x.node.uuid}-${component.uuid}`}
                                  assembledComponentId={x.node.uuid}
                                  partComponentId={component.uuid}
                                />,
                              ]
                            : [],
                        )}
                    >
                      <List.Item.Meta
                        title={
                          <Space>
                            <Link href={paths.component(x.node.uuid)}>
                              {x.node.name}
                            </Link>
                            <div>
                              {x.index && (
                                <Tag color="purple">Layer {x.index}</Tag>
                              )}
                              {x.primeSurface && (
                                <Tag color="volcano">
                                  Prime Surface {x.primeSurface}
                                </Tag>
                              )}
                            </div>
                          </Space>
                        }
                        description={x.node.description}
                      />
                    </List.Item>
                  ))}
                </List>
              )}
            </Col>
          </Row>
        )}
        {(component.concretizationOf.edges.length >= 1 ||
          component.concretizationOf.isAuthorizedToAddEdge ||
          component.generalizationOf.edges.length >= 1 ||
          component.generalizationOf.isAuthorizedToAddEdge) && (
          <Row gutter={[16, 16]}>
            <Col flex={1}>
              {(component.concretizationOf.edges.length >= 1 ||
                component.concretizationOf.isAuthorizedToAddEdge) && (
                <List
                  header="Concretization Of"
                  bordered={true}
                  size="small"
                  footer={
                    component.concretizationOf.isAuthorizedToAddEdge && (
                      <AddGeneralizationOfComponent
                        concreteComponentId={component.uuid}
                      />
                    )
                  }
                >
                  {component.concretizationOf.edges.map((x) => (
                    <List.Item
                      key={x.node.uuid}
                      actions={([] as ReactNode[]).concat(
                        x.isAuthorizedToRemoveEdge
                          ? [
                              <RemoveComponentGeneralization
                                key={`removeComponentGeneralization-${x.node.uuid}`}
                                generalComponentId={x.node.uuid}
                                concreteComponentId={component.uuid}
                              />,
                            ]
                          : [],
                      )}
                    >
                      <List.Item.Meta
                        title={
                          <Link href={paths.component(x.node.uuid)}>
                            {x.node.name}
                          </Link>
                        }
                        description={x.node.description}
                      />
                    </List.Item>
                  ))}
                </List>
              )}
            </Col>
            <Col flex={1}>
              {(component.generalizationOf.edges.length >= 1 ||
                component.generalizationOf.isAuthorizedToAddEdge) && (
                <List
                  header="Generalization Of"
                  bordered={true}
                  size="small"
                  footer={
                    component.generalizationOf.isAuthorizedToAddEdge && (
                      <AddConcretizationOfComponent
                        generalComponentId={component.uuid}
                      />
                    )
                  }
                >
                  {component.generalizationOf.edges.map((x) => (
                    <List.Item
                      key={x.node.uuid}
                      actions={([] as ReactNode[]).concat(
                        x.isAuthorizedToRemoveEdge
                          ? [
                              <RemoveComponentGeneralization
                                key={`removeComponentGeneralization-${x.node.uuid}`}
                                generalComponentId={component.uuid}
                                concreteComponentId={x.node.uuid}
                              />,
                            ]
                          : [],
                      )}
                    >
                      <List.Item.Meta
                        title={
                          <Link href={paths.component(x.node.uuid)}>
                            {x.node.name}
                          </Link>
                        }
                        description={x.node.description}
                      />
                    </List.Item>
                  ))}
                </List>
              )}
            </Col>
          </Row>
        )}
        {(component.variantOf.edges.length >= 1 ||
          component.variantOf.isAuthorizedToAddEdge) && (
          <Row gutter={[16, 16]}>
            <Col flex={1}>
              <List
                header="Variant Of"
                bordered={true}
                size="small"
                footer={
                  component.variantOf.isAuthorizedToAddEdge && (
                    <AddVariantOfComponent componentId={component.uuid} />
                  )
                }
              >
                {component.variantOf.edges.map((x) => (
                  <List.Item
                    key={x.node.uuid}
                    actions={([] as ReactNode[]).concat(
                      x.isAuthorizedToRemoveEdge
                        ? [
                            <RemoveComponentVariant
                              key={`RemoveComponentVariant-${x.node.uuid}`}
                              oneComponentId={component.uuid}
                              otherComponentId={x.node.uuid}
                            />,
                          ]
                        : [],
                    )}
                  >
                    <List.Item.Meta
                      title={
                        <Link href={paths.component(x.node.uuid)}>
                          {x.node.name}
                        </Link>
                      }
                      description={x.node.description}
                    />
                  </List.Item>
                ))}
              </List>
            </Col>
          </Row>
        )}
      </Space>
    </>
  );
}
