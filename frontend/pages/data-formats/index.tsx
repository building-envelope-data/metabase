import { useQuery } from "@apollo/client/react";
import { stringifyApolloError } from "../../lib/apollo";
import Layout from "../../components/Layout";
import { Typography, App } from "antd";
import { DataFormatsDocument } from "../../queries/dataFormats.generated";
import { useEffect } from "react";
import Link from "next/link";
import paths from "../../paths";
import { DataFormatTable } from "../../components/dataFormats/DataFormatTable";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Page() {
  const { loading, error, data } = useQuery(DataFormatsDocument);
  const { message } = App.useApp();

  useEffect(() => {
    if (error) {
      message.error(stringifyApolloError(error));
    }
  }, [error]);

  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        <Link href={paths.data}>Data</Link> is shared as resources. Each
        resource has one of the following data formats:
      </Typography.Paragraph>
      <DataFormatTable
        loading={loading}
        dataFormats={data?.dataFormats?.edges?.map((e) => e.node) || []}
      />
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        The{" "}
        <Typography.Link
          href="/graphql/"
        >
          GraphQL endpoint
        </Typography.Link>{" "}
        provides all information about data formats.
      </Typography.Paragraph>
    </Layout>
  );
}

export default Page;
