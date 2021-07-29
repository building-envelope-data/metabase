import { Scalars } from "../../__generated__/__types__";
import { useMethodQuery } from "../../queries/methods.graphql";
import { message, Skeleton, Result, Typography } from "antd";
import { useEffect } from "react";

export type MethodProps = {
  methodId: Scalars["Uuid"];
};

export const Method: React.FunctionComponent<MethodProps> = ({ methodId }) => {
  const { loading, error, data } = useMethodQuery({
    variables: {
      uuid: methodId,
    },
  });
  const method = data?.method;

  useEffect(() => {
    if (error) {
      message.error(error);
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
    <>
      <Typography.Title>{method.name}</Typography.Title>
    </>
  );
};

export default Method;
