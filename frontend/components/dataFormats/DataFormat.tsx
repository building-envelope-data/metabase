import { Scalars } from "../../__generated__/__types__";
import { useDataFormatQuery } from "../../queries/dataFormats.graphql";
import { Skeleton, Result, Descriptions, Typography } from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import { useEffect } from "react";
import paths from "../../paths";
import { Reference } from "../Reference";
import { messageApolloError } from "../../lib/apollo";
import UpdateDataFormat from "./UpdateDataFormat";

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
      messageApolloError(error);
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
    <PageHeader
      title={dataFormat.name}
      subTitle={dataFormat.description}
      extra={
        dataFormat.canCurrentUserUpdateNode
          ? [
              <UpdateDataFormat
                key="updateDataFormat"
                dataFormatId={dataFormat.uuid}
                name={dataFormat.name}
                extension={dataFormat.extension}
                description={dataFormat.description}
                mediaType={dataFormat.mediaType}
                schemaLocator={dataFormat.schemaLocator}
                reference={dataFormat.reference}
                managerId={dataFormat.manager.node.uuid}
              />,
            ]
          : []
      }
      backIcon={false}
    >
      <Descriptions size="small" column={1}>
        <Descriptions.Item label="UUID">{dataFormat.uuid}</Descriptions.Item>
        <Descriptions.Item label="Extension">
          {dataFormat.extension}
        </Descriptions.Item>
        <Descriptions.Item label="Media Type">
          <Typography.Link href="http://www.iana.org/assignments/media-types/media-types.xhtml">
            {dataFormat.mediaType}
          </Typography.Link>
        </Descriptions.Item>
        <Descriptions.Item label="Schema">
          <Typography.Link href={dataFormat.schemaLocator}>
            {dataFormat.schemaLocator}
          </Typography.Link>
        </Descriptions.Item>
        <Descriptions.Item label="Reference">
          <Reference reference={dataFormat.reference} />
        </Descriptions.Item>
        <Descriptions.Item label="Managed by">
          <Typography.Link
            href={paths.institution(dataFormat.manager.node.uuid)}
          >
            {dataFormat.manager.node.name}
          </Typography.Link>
        </Descriptions.Item>
      </Descriptions>
    </PageHeader>
  );
}
