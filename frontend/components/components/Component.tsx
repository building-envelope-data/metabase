import { Scalars } from "../../__generated__/__types__";
import {
  ComponentDocument,
  ComponentsDocument,
  useComponentQuery,
} from "../../queries/components.graphql";
import {
  useAddComponentAssemblyMutation,
  useRemoveComponentAssemblyMutation,
} from "../../queries/componentAssemblies.graphql";
import {
  Skeleton,
  Result,
  Descriptions,
  Tag,
  List,
  Button,
  message,
} from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import { useEffect, useState } from "react";
import paths from "../../paths";
import Link from "next/link";
import OpenEndedDateTimeRangeX from "../OpenEndedDateTimeRangeX";
import { messageApolloError } from "../../lib/apollo";
import AddPartOfComponent from "./AddPartOfComponent";
import AddAssembledOfComponent from "./AddAssembledOfComponent";

export type ComponentProps = {
  componentId: Scalars["Uuid"];
};

export default function Component({ componentId }: ComponentProps) {
  const { loading, error, data } = useComponentQuery({
    variables: {
      uuid: componentId,
    },
  });
  const component = data?.component;

  useEffect(() => {
    if (error) {
      messageApolloError(error);
    }
  }, [error]);

  const [removeComponentAssemblyMutation] =
    useRemoveComponentAssemblyMutation();
  const [removingComponentAssembly, setRemovingComponentAssembly] =
    useState(false);

  const removeComponentAssembly = async (
    assembledComponentId: Scalars["Uuid"],
    partComponentId: Scalars["Uuid"]
  ) => {
    try {
      setRemovingComponentAssembly(true);
      const { errors, data } = await removeComponentAssemblyMutation({
        variables: {
          assembledComponentId: assembledComponentId,
          partComponentId: partComponentId,
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
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.removeComponentAssembly?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.removeComponentAssembly?.errors
            .map((error) => error.message)
            .join(" ")
        );
      }
    } finally {
      setRemovingComponentAssembly(false);
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
      backIcon={false}
    >
      <Descriptions size="small" column={1}>
        <Descriptions.Item label="UUID">{component.uuid}</Descriptions.Item>
        <Descriptions.Item label="Available">
          <OpenEndedDateTimeRangeX range={component.availability} />
        </Descriptions.Item>
        <Descriptions.Item label="Manufactured by">
          {component.manufacturers.edges.length == 1 ? (
            <Link
              href={paths.institution(
                component.manufacturers.edges[0].node.uuid
              )}
            >
              {component.manufacturers.edges[0].node.name}
            </Link>
          ) : (
            <List size="small">
              {component.manufacturers.edges.map((x) => (
                <List.Item key={x.node.uuid}>
                  <Link href={paths.institution(x.node.uuid)}>
                    {x.node.name}
                  </Link>
                </List.Item>
              ))}
            </List>
          )}
        </Descriptions.Item>
        {component.assembledOf.edges.length >= 1 && (
          <Descriptions.Item label="Assembled Of">
            <List size="small">
              {component.assembledOf.edges.map((x) => (
                <List.Item key={x.node.uuid}>
                  <Link href={paths.component(x.node.uuid)}>{x.node.name}</Link>
                  {component.assembledOf.canCurrentUserRemoveEdge && (
                    <Button
                      onClick={() =>
                        removeComponentAssembly(component.uuid, x.node.uuid)
                      }
                      loading={removingComponentAssembly}
                    >
                      Remove
                    </Button>
                  )}
                </List.Item>
              ))}
            </List>
            {component.assembledOf.canCurrentUserAddEdge && (
              <AddPartOfComponent assembledComponentId={component.id} />
            )}
          </Descriptions.Item>
        )}
        {component.partOf.edges.length >= 1 && (
          <Descriptions.Item label="Part Of">
            <List size="small">
              {component.partOf.edges.map((x) => (
                <List.Item key={x.node.uuid}>
                  <Link href={paths.component(x.node.uuid)}>{x.node.name}</Link>
                  {component.partOf.canCurrentUserRemoveEdge && (
                    <Button
                      onClick={() =>
                        removeComponentAssembly(x.node.uuid, component.uuid)
                      }
                      loading={removingComponentAssembly}
                    >
                      Remove
                    </Button>
                  )}
                </List.Item>
              ))}
            </List>
            {component.partOf.canCurrentUserAddEdge && (
              <AddAssembledOfComponent partComponentId={component.id} />
            )}
          </Descriptions.Item>
        )}
        {component.concretizationOf.edges.length >= 1 && (
          <Descriptions.Item label="Concretization Of">
            <List size="small">
              {component.concretizationOf.edges.map((x) => (
                <List.Item key={x.node.uuid}>
                  <Link href={paths.component(x.node.uuid)}>{x.node.name}</Link>
                </List.Item>
              ))}
            </List>
          </Descriptions.Item>
        )}
        {component.generalizationOf.edges.length >= 1 && (
          <Descriptions.Item label="Generalization Of">
            <List size="small">
              {component.generalizationOf.edges.map((x) => (
                <List.Item key={x.node.uuid}>
                  <Link href={paths.component(x.node.uuid)}>{x.node.name}</Link>
                </List.Item>
              ))}
            </List>
          </Descriptions.Item>
        )}
        {component.variantOf.edges.length >= 1 && (
          <Descriptions.Item label="Variant Of">
            <List size="small">
              {component.variantOf.edges.map((x) => (
                <List.Item key={x.node.uuid}>
                  <Link href={paths.component(x.node.uuid)}>{x.node.name}</Link>
                </List.Item>
              ))}
            </List>
          </Descriptions.Item>
        )}
      </Descriptions>
    </PageHeader>
  );
}
