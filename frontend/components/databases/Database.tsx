import {
  DatabaseVerificationState,
  Scalars,
} from "../../__generated__/graphql";
import { DatabaseDocument } from "../../queries/databases.generated";
import { Skeleton, Result, Descriptions, Typography, Tag, App } from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import { ReactNode, useEffect } from "react";
import Link from "next/link";
import paths from "../../paths";
import { stringifyApolloError } from "../../lib/apollo";
import UpdateDatabase from "./UpdateDatabase";
import VerifyDatabase from "./VerifyDatabase";
import { useQuery } from "@apollo/client/react";

export type DatabaseProps = {
  databaseId: Scalars["Uuid"]["input"];
};

export default function Database({ databaseId }: DatabaseProps) {
  const { loading, error, data } = useQuery(DatabaseDocument, {
    variables: {
      uuid: databaseId,
    },
  });
  const database = data?.database;
  const { message } = App.useApp();

  useEffect(() => {
    if (error) {
      message.error(stringifyApolloError(error));
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
    <>
      <PageHeader
        title={database.name}
        subTitle={database.description}
        extra={([] as ReactNode[])
          .concat(
            database.isAuthorizedToUpdateNode
              ? [
                  <UpdateDatabase
                    key="updateDatabase"
                    databaseId={database.uuid}
                    name={database.name}
                    description={database.description}
                    locator={database.locator}
                  />,
                ]
              : [],
          )
          .concat(
            database.isAuthorizedToVerifyNode &&
              database.verificationState == DatabaseVerificationState.Pending
              ? [
                  <VerifyDatabase
                    key="verifyDatabase"
                    databaseId={database.uuid}
                  />,
                ]
              : [],
          )}
        tags={[
          <Tag key="verificationState" color="magenta">
            {database.verificationState}
          </Tag>,
        ]}
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
            <Link href={paths.institution(database.operator.node.uuid)}>
              {database.operator.node.name}
            </Link>
          </Descriptions.Item>
        </Descriptions>
      </PageHeader>
      {database.isAuthorizedToVerifyNode &&
        database.verificationState == DatabaseVerificationState.Pending && (
          <Typography.Paragraph>
            Have your database&apos;s GraphQL endpoint return the verification
            code &ldquo;
            {database.verificationCode}&rdquo; (without the quotation marks),
            when queried for the GraphQL query &ldquo;verificationCode&rdquo;.
            Then, press the &ldquo;Verify&rdquo; button above to make the
            metabase assert that the verification codes match which proves that
            you control the GraphQL endpoint {database.locator}. Verified
            databases are publicly listed and included in data searches.
          </Typography.Paragraph>
        )}
    </>
  );
}
