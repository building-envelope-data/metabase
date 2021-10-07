import { Scalars } from "../../__generated__/__types__";
import { useDatabaseQuery } from "../../queries/databases.graphql";
import { Skeleton, Result, PageHeader, Descriptions, Typography } from "antd";
import { useEffect } from "react";
import Link from "next/link";
import paths from "../../paths";
import { messageApolloError } from "../../lib/apollo";

export type DatabaseProps = {
  databaseId: Scalars["UUID"];
};

export default function Database({ databaseId }: DatabaseProps) {
  const { loading, error, data } = useDatabaseQuery({
    variables: {
      uuid: databaseId,
    },
  });
  const database = data?.database;

  useEffect(() => {
    if (error) {
      messageApolloError(error);
    }
  }, [error]);

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!database) {
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
      title={database.name}
      subTitle={database.description}
      backIcon={false}
    >
      <Descriptions size="small" column={1}>
        <Descriptions.Item label="UUID">{database.uuid}</Descriptions.Item>
        <Descriptions.Item label="Located at">
          <Typography.Link href={database.locator}>
            {database.locator}
          </Typography.Link>
        </Descriptions.Item>
        <Descriptions.Item label="Operated by">
          <Link href={paths.database(database.operator.node.uuid)}>
            {database.operator.node.name}
          </Link>
        </Descriptions.Item>
      </Descriptions>
    </PageHeader>
  );
}
