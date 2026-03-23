import { Scalars } from "../../__generated__/graphql";
import { DataFormatDocument } from "../../queries/dataFormats.generated";
import { Skeleton, Result, Descriptions, Typography } from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import paths from "../../paths";
import { Reference } from "../Reference";
import UpdateDataFormat from "./UpdateDataFormat";
import { useQuery } from "@apollo/client/react";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";

export type DataFormatProps = {
  dataFormatId: Scalars["Uuid"]["input"];
};

export default function DataFormat({ dataFormatId }: DataFormatProps) {
  const { loading, error, data } = useQuery(DataFormatDocument, {
    variables: {
      uuid: dataFormatId,
    },
  });
  useQueryHandler({ error });
  const dataFormat = data?.dataFormat;

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
      <PageHeader
        title={dataFormat.name}
        subTitle={dataFormat.description}
        extra={
          dataFormat.isAuthorizedToUpdateNode
            ? [
                <UpdateDataFormat
                  key="updateDataFormat"
                  dataFormat={dataFormat}
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
          {dataFormat.schemaLocator && (
            <Descriptions.Item label="Schema">
              <Typography.Link href={dataFormat.schemaLocator}>
                {dataFormat.schemaLocator}
              </Typography.Link>
            </Descriptions.Item>
          )}
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
    </>
  );
}
