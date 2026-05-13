import { useQuery } from "@apollo/client/react";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { Divider, Typography } from "antd";
import { CurrentUserDocument } from "../../queries/currentUser.generated";
import PendingDatabaseList from "../../components/databases/PendingDatabaseList";
import Link from "next/link";
import { UserRole } from "../../__generated__/graphql";
import PaginatedDatabases from "../../components/databases/PaginatedDatabases";

export default function Page() {
  const currentUserData = useQuery(CurrentUserDocument)?.data;
  const currentUser = currentUserData?.currentUser;

  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: "75ch" }}>
        The following databases are connected to{" "}
        <Link href={paths.home}>buildingenvelopedata.org</Link> and contain{" "}
        <Link href={paths.allData}>data</Link> on{" "}
        <Link href={paths.components}>components</Link>.
      </Typography.Paragraph>
      <PaginatedDatabases showJump />
      <Typography.Paragraph style={{ marginTop: "1em", maxWidth: "75ch" }}>
        The <Typography.Link href="/graphql/">GraphQL endpoint</Typography.Link>{" "}
        provides all information about databases.
      </Typography.Paragraph>
      {currentUser?.roles?.includes(UserRole.Administrator) &&
        currentUserData?.pendingDatabases &&
        currentUserData.pendingDatabases.totalCount > 0 && (
          <div>
            <Divider />
            <Typography.Title level={4}>Pending Databases</Typography.Title>
            <PendingDatabaseList />
          </div>
        )}
    </Layout>
  );
}
