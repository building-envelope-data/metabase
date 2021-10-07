import { Scalars } from "../../__generated__/__types__";
import { useMethodQuery } from "../../queries/methods.graphql";
import {
  Tag,
  Skeleton,
  Result,
  PageHeader,
  Descriptions,
  Typography,
  List,
} from "antd";
import { useEffect } from "react";
import Link from "next/link";
import paths from "../../paths";
import { Reference } from "../Reference";
import OpenEndedDateTimeRangeX from "../OpenEndedDateTimeRangeX";
import { messageApolloError } from "../../lib/apollo";

function getDeveloperHref(
  node:
    | {
        __typename?: "Institution" | undefined;
        id: string;
        uuid: any;
        name: string;
      }
    | { __typename?: "User" | undefined; id: string; uuid: any; name: string }
) {
  switch (node.__typename) {
    case "Institution":
      return paths.institution(node.uuid);
    case "User":
      return paths.user(node.uuid);
    default:
      // TODO What shall we do in this supposedly impossible case?
      return "https://www.buildingenvelopedata.org";
  }
}

export type MethodProps = {
  methodId: Scalars["UUID"];
};

export default function Method({ methodId }: MethodProps) {
  const { loading, error, data } = useMethodQuery({
    variables: {
      uuid: methodId,
    },
  });
  const method = data?.method;

  useEffect(() => {
    if (error) {
      messageApolloError(error);
    }
  }, [error]);

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!method) {
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
      title={method.name}
      subTitle={method.description}
      tags={method.categories.map((x) => (
        <Tag key={x} color="magenta">
          {x}
        </Tag>
      ))}
      backIcon={false}
    >
      <Descriptions size="small" column={1}>
        <Descriptions.Item label="UUID">{method.uuid}</Descriptions.Item>
        <Descriptions.Item label="Valid">
          <OpenEndedDateTimeRangeX range={method.validity} />
        </Descriptions.Item>
        <Descriptions.Item label="Available">
          <OpenEndedDateTimeRangeX range={method.availability} />
        </Descriptions.Item>
        <Descriptions.Item label="Calculation">
          <Typography.Link href={method.calculationLocator}>
            {method.calculationLocator}
          </Typography.Link>
        </Descriptions.Item>
        <Descriptions.Item label="Reference">
          <Reference reference={method.reference} />
        </Descriptions.Item>
        <Descriptions.Item label="Developed by">
          {method.developers.edges.length == 1 ? (
            <Link href={getDeveloperHref(method.developers.edges[0].node)}>
              {method.developers.edges[0].node.name}
            </Link>
          ) : (
            <List size="small">
              {method.developers.edges.map((x) => (
                <List.Item key={x.node.uuid}>
                  <Link href={getDeveloperHref(x.node)}>{x.node.name}</Link>
                </List.Item>
              ))}
            </List>
          )}
        </Descriptions.Item>
        <Descriptions.Item label="Managed by">
          <Typography.Link href={paths.institution(method.manager.node.uuid)}>
            {method.manager.node.name}
          </Typography.Link>
        </Descriptions.Item>
      </Descriptions>
    </PageHeader>
  );
}
