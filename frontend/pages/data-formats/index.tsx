import Layout from "../../components/Layout";
import { Typography } from "antd";
import paths from "../../paths";
import Link from "next/link";
import PaginatedDataFormats from "../../components/dataFormats/PaginatedDataFormats";

export default function Page() {
  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: "75ch" }}>
        <Link href={paths.allData}>Data</Link> is shared as resources. Each
        resource has one of the following data formats:
      </Typography.Paragraph>
      <PaginatedDataFormats showJump />
      <Typography.Paragraph style={{ marginTop: "1em", maxWidth: "75ch" }}>
        The <Typography.Link href="/graphql/">GraphQL endpoint</Typography.Link>{" "}
        provides all information about data formats.
      </Typography.Paragraph>
    </Layout>
  );
}
