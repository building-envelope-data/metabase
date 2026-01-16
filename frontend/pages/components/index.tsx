import { useQuery } from "@apollo/client/react";
import { stringifyApolloError } from "../../lib/apollo";
import Layout from "../../components/Layout";
import { Typography, App } from "antd";
import { ComponentsDocument } from "../../queries/components.generated";
import { useEffect } from "react";
import paths from "../../paths";
import Link from "next/link";
import { ComponentTable } from "../../components/components/ComponentTable";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Page() {
  const { loading, error, data } = useQuery(ComponentsDocument);
  const nodes = data?.components?.edges?.map((e) => e.node) || [];
  const { message } = App.useApp();

  useEffect(() => {
    if (error) {
      message.error(stringifyApolloError(error));
    }
  }, [error]);

  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        The building envelope components for which{" "}
        <Link href={paths.data}>data</Link> is available are presented here.
      </Typography.Paragraph>
      <ComponentTable loading={loading} components={nodes} />
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        The{" "}
        <Typography.Link
          href={`${process.env.NEXT_PUBLIC_METABASE_URL}/graphql/`}
        >
          GraphQL endpoint
        </Typography.Link>{" "}
        provides all information about components.
      </Typography.Paragraph>
    </Layout>
  );
}

export default Page;
