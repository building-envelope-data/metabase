import { Scalars } from "../../__generated__/graphql";
import { DataFormatDocument } from "../../queries/dataFormats.generated";
import { Skeleton, Result, Descriptions, Typography, App } from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import { useEffect } from "react";
import paths from "../../paths";
import { Reference } from "../Reference";
import { stringifyApolloError } from "../../lib/apollo";
import UpdateDataFormat from "./UpdateDataFormat";
import { useQuery } from "@apollo/client/react";

export type DataFormatProps = {
  dataFormatId: Scalars["Uuid"]["input"];
};

export default function DataFormat({ dataFormatId }: DataFormatProps) {
  const { loading, error, data } = useQuery(DataFormatDocument, {
    variables: {
      uuid: dataFormatId,
    },
  });
  const dataFormat = data?.dataFormat;
  const { message } = App.useApp();

  useEffect(() => {
    if (error) {
      message.error(stringifyApolloError(error));
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
      <PageHeader
        title={dataFormat.name}
        subTitle={dataFormat.description}
        extra={
          dataFormat.isAuthorizedToUpdateNode
            ? [
                <UpdateDataFormat
                  key="updateDataFormat"
                  dataFormat={dataFormat}
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
