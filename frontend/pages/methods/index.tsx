import { useQuery } from "@apollo/client/react";
import { stringifyApolloError } from "../../lib/apollo";
import Layout from "../../components/Layout";
import { Typography, App } from "antd";
import { MethodsDocument } from "../../queries/methods.generated";
import { useEffect } from "react";
import paths from "../../paths";
import Link from "next/link";
import MethodTable from "../../components/methods/MethodTable";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Page() {
  const { loading, error, data } = useQuery(MethodsDocument);
  const nodes = data?.methods?.edges?.map((e) => e.node) || [];
  const { message } = App.useApp();

  useEffect(() => {
    if (error) {
      message.error(stringifyApolloError(error));
    }
  }, [error]);

  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        <Link href={paths.data}>Data</Link> is created by applying a method.
        Methods can be defined for example by a standard.
      </Typography.Paragraph>
      <MethodTable loading={loading} methods={nodes} />
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        The{" "}
        <Typography.Link
          href="/graphql/"
        >
          GraphQL endpoint
        </Typography.Link>{" "}
        provides all information about methods.
      </Typography.Paragraph>
    </Layout>
  );
}

export default Page;
