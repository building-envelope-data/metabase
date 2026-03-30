import { useQuery } from "@apollo/client/react";
import Layout from "../../components/Layout";
import Link from "next/link";
import paths from "../../paths";
import { Typography, Divider } from "antd";
import { CurrentUserDocument } from "../../queries/currentUser.generated";
import PendingInstitutionList from "../../components/institutions/PendingInstitutionList";
import { UserRole } from "../../__generated__/graphql";
import PaginatedInstitutions from "../../components/institutions/PaginatedInstitutions";

export default function Page() {
  const currentUser = useQuery(CurrentUserDocument)?.data?.currentUser;

  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: "75ch" }}>
        Institutions can manufacture{" "}
        <Link href={paths.components}>components</Link>, operate{" "}
        <Link href={paths.databases}>databases</Link> and create{" "}
        <Link href={paths.dataFormats}>data formats</Link> and{" "}
        <Link href={paths.methods}>methods</Link>.
      </Typography.Paragraph>
      {currentUser && (
        <Link href={paths.institutionCreate}>Create Institution</Link>
      )}
      <PaginatedInstitutions showJump />
      <Typography.Paragraph style={{ marginTop: "1em", maxWidth: "75ch" }}>
        The <Typography.Link href="/graphql/">GraphQL endpoint</Typography.Link>{" "}
        provides all information about institutions.
      </Typography.Paragraph>
      {currentUser?.roles?.includes(UserRole.Verifier) && (
        <div>
          <Divider />
          <Typography.Title level={4}>Pending Institutions</Typography.Title>
          <PendingInstitutionList />
        </div>
      )}
    </Layout>
  );
}
