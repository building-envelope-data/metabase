import { Scalars } from "../../__generated__/__types__";
import { useDataFormatQuery } from "../../queries/dataFormats.graphql";
import { message, Skeleton, Result, Typography } from "antd";
import { useEffect } from "react";

export type DataFormatProps = {
  dataFormatId: Scalars["Uuid"];
};

export default function DataFormat({ dataFormatId }: DataFormatProps) {
  const { loading, error, data } = useDataFormatQuery({
    variables: {
      uuid: dataFormatId,
    },
  });
  const dataFormat = data?.dataFormat;

  useEffect(() => {
    if (error) {
      message.error(error);
    }
  }, [error]);

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!dataFormat) {
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
      <Typography.Title>{dataFormat.name}</Typography.Title>
    </>
  );
}
