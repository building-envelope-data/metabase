import Layout from "../../components/Layout";
import paths from "../../paths";
import { Typography } from "antd";
import Link from "next/link";
import PaginatedGnuPgKeys from "../../components/gnuPgKeys/PaginatedGnuPgKeys";

export default function Page() {
  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: "75ch" }}>
        GnuPG keys of <Link href={paths.institutions}>institutions</Link> used
        for approvals of <Link href={paths.allData}>data</Link> or GraphQL query
        responses.
      </Typography.Paragraph>
      <PaginatedGnuPgKeys showJump />
      <Typography.Paragraph style={{ marginTop: "1em", maxWidth: "75ch" }}>
        The <Typography.Link href="/graphql/">GraphQL endpoint</Typography.Link>{" "}
        provides all information about GnuPG keys.
      </Typography.Paragraph>
    </Layout>
  );
}
