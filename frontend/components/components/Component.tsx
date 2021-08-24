import { Scalars } from "../../__generated__/__types__";
import { useComponentQuery } from "../../queries/components.graphql";
import { Skeleton, Result, PageHeader, Descriptions, Tag, List } from "antd";
import { useEffect } from "react";
import paths from "../../paths";
import Link from "next/link";
import OpenEndedDateTimeRangeX from "../OpenEndedDateTimeRangeX";
import { messageApolloError } from "../../lib/apollo";

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
      </Descriptions>
    </PageHeader>
  );
}
