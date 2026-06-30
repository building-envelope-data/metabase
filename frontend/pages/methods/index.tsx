import Layout from "../../components/Layout";
import { Typography } from "antd";
import paths from "../../paths";
import Link from "next/link";
import PaginatedMethods from "../../components/methods/PaginatedMethods";

export default function Page() {
  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: "75ch" }}>
        <Link href={paths.allData}>Data</Link> is created by applying a method.
        Methods can be defined for example by a standard.
      </Typography.Paragraph>
      <PaginatedMethods showJump />
      <Typography.Paragraph style={{ marginTop: "1em", maxWidth: "75ch" }}>
        The <Typography.Link href="/graphql/">GraphQL endpoint</Typography.Link>{" "}
        provides all information about methods.
      </Typography.Paragraph>
    </Layout>
  );
}
