import {
  DatabaseVerificationState,
  Scalars,
} from "../../__generated__/graphql";
import { DatabaseDocument } from "../../queries/databases.generated";
import { Skeleton, Result, Typography, Card, Divider } from "antd";
import { useQuery } from "@apollo/client/react";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import DatabaseSummary from "./DatabaseSummary";
import QueryToolbar from "../QueryToolbar";

interface DatabaseProps {
  databaseId: Scalars["Uuid"]["input"];
}

export default function Database({ databaseId }: DatabaseProps) {
  const queryVariables = {
    uuid: databaseId,
  };
  const { loading, error, data } = useQuery(DatabaseDocument, {
    variables: queryVariables,
  });
  useQueryHandler({ error });
  const database = data?.database;

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
    <Card>
      <DatabaseSummary entity={database} />
      {database.isAuthorizedToVerifyNode &&
        database.verificationState == DatabaseVerificationState.Pending && (
          <Typography.Paragraph style={{ maxWidth: "75ch" }}>
            Have your database&apos;s GraphQL endpoint return the verification
            code &ldquo;{database.verificationCode}&rdquo; (without the
            quotation marks), when queried for the GraphQL query
            &ldquo;verificationCode&rdquo;. Then, press the &ldquo;Verify&rdquo;
            button above to make the metabase assert that the verification codes
            match which proves that you control the GraphQL endpoint{" "}
            {database.locator}. Verified databases are publicly listed and
            included in data searches.
          </Typography.Paragraph>
        )}
      <Divider />
      <QueryToolbar query={DatabaseDocument} variables={queryVariables} />
    </Card>
  );
}
