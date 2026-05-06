import Layout from "../../components/Layout";
import { Typography } from "antd";
import paths from "../../paths";
import Link from "next/link";
import PaginatedComponents from "../../components/components/PaginatedComponents";

export default function Page() {
  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: "75ch" }}>
        The building envelope components for which{" "}
        <Link href={paths.allData}>data</Link> is available are presented here.
      </Typography.Paragraph>
      <PaginatedComponents showJump />
      <Typography.Paragraph style={{ marginTop: "1em", maxWidth: "75ch" }}>
        The <Typography.Link href="/graphql/">GraphQL endpoint</Typography.Link>{" "}
        provides all information about components.
      </Typography.Paragraph>
    </Layout>
  );
}
