import { Scalars } from "../../__generated__/__types__";
import { useComponentQuery } from "../../queries/components.graphql";
import { message, Skeleton, Result, Typography } from "antd";
import { useEffect } from "react";

export type ComponentProps = {
  componentId: Scalars["Uuid"];
};

export const Component = (
  {
    componentId
  }: ComponentProps
) => {
  const { loading, error, data } = useComponentQuery({
    variables: {
      uuid: componentId,
    },
  });
  const component = data?.component;

  useEffect(() => {
    if (error) {
      message.error(error);
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
    <>
      <Typography.Title>{component.name}</Typography.Title>
    </>
  );
};

export default Component;
