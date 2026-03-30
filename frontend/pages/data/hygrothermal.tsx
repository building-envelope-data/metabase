import Layout from "../../components/Layout";
import { Typography } from "antd";
import Link from "next/link";
import paths from "../../paths";
import PaginatedHygrothermalData from "../../components/data/hygrothermal/PaginatedHygrothermalData";

export default function Page() {
  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: "75ch" }}>
        The network of <Link href={paths.databases}>databases</Link> can be
        queried here for data on building envelope{" "}
        <Link href={paths.components}>components</Link>.
      </Typography.Paragraph>
      <PaginatedHygrothermalData />
      <Typography.Paragraph style={{ maxWidth: "75ch" }}>
        The <Typography.Link href="/graphql/">GraphQL endpoint</Typography.Link>{" "}
        is the most powerful way of querying the databases.
      </Typography.Paragraph>
    </Layout>
  );
}
