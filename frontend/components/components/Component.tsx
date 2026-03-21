import { useMutation } from "@apollo/client/react";
import { useQuery } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  ComponentsDocument,
  ComponentDocument,
} from "../../queries/components.generated";
import { RemoveComponentAssemblyDocument } from "../../queries/componentAssemblies.generated";
import { RemoveComponentVariantDocument } from "../../queries/componentVariants.generated";
import {
  Skeleton,
  Result,
  Descriptions,
  Tag,
  List,
  Button,
  Row,
  Col,
  Space,
  App,
} from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import { ReactNode, useEffect, useState } from "react";
import paths from "../../paths";
import Link from "next/link";
import OpenEndedDateTimeRangeX from "../OpenEndedDateTimeRangeX";
import { stringifyApolloError } from "../../lib/apollo";
import AddPartOfComponent from "./AddPartOfComponent";
import AddAssembledOfComponent from "./AddAssembledOfComponent";
import UpdateComponentAssembly from "./UpdateComponentAssembly";
import AddVariantOfComponent from "./AddVariantOfComponent";
import AddConcretizationOfComponent from "./AddConcretizationOfComponent";
import AddGeneralizationOfComponent from "./AddGeneralizationOfComponent";
import { RemoveComponentGeneralizationDocument } from "../../queries/componentGeneralizations.generated";
import UpdateComponent from "./UpdateComponent";
import AddComponentManufacturer from "./AddComponentManufacturer";
import { RemoveComponentManufacturer } from "./RemoveComponentManufacturer";

export type ComponentProps = {
  componentId: Scalars["Uuid"]["input"];
};

export default function Component({ componentId }: ComponentProps) {
  const { loading, error, data } = useQuery(ComponentDocument, {
    variables: {
      uuid: componentId,
    },
  });
  const component = data?.component;
  const { message } = App.useApp();

  useEffect(() => {
    if (error) {
      message.error(stringifyApolloError(error));
    }
  }, [error]);

  const [removeComponentAssemblyMutation] = useMutation(
    RemoveComponentAssemblyDocument,
  );
  const [removingComponentAssembly, setRemovingComponentAssembly] =
    useState(false);

  const removeComponentAssembly = async (
    assembledComponentId: Scalars["Uuid"]["input"],
    partComponentId: Scalars["Uuid"]["input"],
  ) => {
    try {
      setRemovingComponentAssembly(true);
      const { error, data } = await removeComponentAssemblyMutation({
        variables: {
          input: {
            assembledComponentId: assembledComponentId,
            partComponentId: partComponentId,
          },
        },
        refetchQueries: [
          {
            query: ComponentsDocument,
          },
          {
            query: ComponentDocument,
            variables: {
              uuid: assembledComponentId,
            },
          },
          {
            query: ComponentDocument,
            variables: {
              uuid: partComponentId,
            },
          },
        ],
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.removeComponentAssembly?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.removeComponentAssembly?.errors
            .map((error) => error.message)
            .join(" "),
        );
      }
    } finally {
      setRemovingComponentAssembly(false);
    }
  };

  const [removeComponentVariantMutation] = useMutation(
    RemoveComponentVariantDocument,
  );
  const [removingComponentVariant, setRemovingComponentVariant] =
    useState(false);

  const removeComponentVariant = async (
    oneComponentId: Scalars["Uuid"]["input"],
    otherComponentId: Scalars["Uuid"]["input"],
  ) => {
    try {
      setRemovingComponentVariant(true);
      const { error, data } = await removeComponentVariantMutation({
        variables: {
          input: {
            oneComponentId: oneComponentId,
            otherComponentId: otherComponentId,
          },
        },
        refetchQueries: [
          {
            query: ComponentsDocument,
          },
          {
            query: ComponentDocument,
            variables: {
              uuid: oneComponentId,
            },
          },
          {
            query: ComponentDocument,
            variables: {
              uuid: otherComponentId,
            },
          },
        ],
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.removeComponentVariant?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.removeComponentVariant?.errors
            .map((error) => error.message)
            .join(" "),
        );
      }
    } finally {
      setRemovingComponentVariant(false);
    }
  };

  const [removeComponentGeneralizationMutation] = useMutation(
    RemoveComponentGeneralizationDocument,
  );
  const [removingComponentGeneralization, setRemovingComponentGeneralization] =
    useState(false);

  const removeComponentGeneralization = async (
    generalComponentId: Scalars["Uuid"]["input"],
    concreteComponentId: Scalars["Uuid"]["input"],
  ) => {
    try {
      setRemovingComponentGeneralization(true);
      const { error, data } = await removeComponentGeneralizationMutation({
        variables: {
          input: {
            generalComponentId: generalComponentId,
            concreteComponentId: concreteComponentId,
          },
        },
        refetchQueries: [
          {
            query: ComponentsDocument,
          },
          {
            query: ComponentDocument,
            variables: {
              uuid: generalComponentId,
            },
          },
          {
            query: ComponentDocument,
            variables: {
              uuid: concreteComponentId,
            },
          },
        ],
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.removeComponentGeneralization?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.removeComponentGeneralization?.errors
            .map((error) => error.message)
            .join(" "),
        );
      }
    } finally {
      setRemovingComponentGeneralization(false);
    }
  };

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
            ? [
                <UpdateComponent
                  key="updateComponent"
                  componentId={component.uuid}
                  name={component.name}
                  abbreviation={component.abbreviation}
                  description={component.description}
                  availability={component.availability}
                  categories={component.categories}
                  primeSurface={component.prime?.surface}
                  primeDirection={component.prime?.direction}
                  switchableLayers={component.switchableLayers}
                />,
              ]
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
        </Descriptions>
      </PageHeader>
      <Space direction="vertical" style={{ display: "flex" }}>
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
                            key={`removeComponentManufacturer-${component.uuid}-${x.node.uuid}`}
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
                            key={`removeComponentManufacturer-${component.uuid}-${x.node.uuid}`}
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
                                  key="update"
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
                                <Button
                                  key="remove"
                                  onClick={() =>
                                    removeComponentAssembly(
                                      component.uuid,
                                      x.node.uuid,
                                    )
                                  }
                                  loading={removingComponentAssembly}
                                >
                                  Remove
                                </Button>,
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
                              <Tag color="purple">Layer {x.index}</Tag>
                              <Tag color="volcano">
                                Prime Surface {x.primeSurface}
                              </Tag>
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
                                  key="update"
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
                                <Button
                                  key="remove"
                                  onClick={() =>
                                    removeComponentAssembly(
                                      x.node.uuid,
                                      component.uuid,
                                    )
                                  }
                                  loading={removingComponentAssembly}
                                >
                                  Remove
                                </Button>,
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
                              <Tag color="purple">Layer {x.index}</Tag>
                              <Tag color="volcano">
                                Prime Surface {x.primeSurface}
                              </Tag>
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
                              <Button
                                key="remove"
                                onClick={() =>
                                  removeComponentGeneralization(
                                    x.node.uuid,
                                    component.uuid,
                                  )
                                }
                                loading={removingComponentGeneralization}
                              >
                                Remove
                              </Button>,
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
                              <Button
                                key="remove"
                                onClick={() =>
                                  removeComponentGeneralization(
                                    component.uuid,
                                    x.node.uuid,
                                  )
                                }
                                loading={removingComponentGeneralization}
                              >
                                Remove
                              </Button>,
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
                            <Button
                              key="remove"
                              onClick={() =>
                                removeComponentVariant(
                                  component.uuid,
                                  x.node.uuid,
                                )
                              }
                              loading={removingComponentVariant}
                            >
                              Remove
                            </Button>,
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
