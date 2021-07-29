import { Scalars } from "../../__generated__/__types__";
import { useDatabaseQuery } from "../../queries/databases.graphql";
import { message, Skeleton, Result, Typography } from "antd";
import { useEffect } from "react";

export type DatabaseProps = {
  databaseId: Scalars["Uuid"];
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
      message.error(error);
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
      <Typography.Title>{database.name}</Typography.Title>
    </>
  );
}
