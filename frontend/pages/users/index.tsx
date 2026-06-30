import Layout from "../../components/Layout";
import { Typography } from "antd";
import paths from "../../paths";
import Link from "next/link";
import PaginatedUsers from "../../components/users/PaginatedUsers";

function Page() {
  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: "75ch" }}>
        A user is usually affiliated to an{" "}
        <Link href={paths.institutions}>institution</Link>. In further steps,
        users can receive the permission for example to add{" "}
        <Link href={paths.components}>components</Link> manufactured by this{" "}
        <Link href={paths.institutions}>institution</Link>.
      </Typography.Paragraph>
      <PaginatedUsers showJump />
      <Typography.Paragraph style={{ marginTop: "1em", maxWidth: "75ch" }}>
        The <Typography.Link href="/graphql/">GraphQL endpoint</Typography.Link>{" "}
        can as well be used to find, for example, the users of your{" "}
        <Link href={paths.institutions}>institution</Link>.
      </Typography.Paragraph>
    </Layout>
  );
}

export default Page;
