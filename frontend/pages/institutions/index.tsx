import { useQuery } from "@apollo/client/react";
import Layout from "../../components/Layout";
import Link from "next/link";
import paths from "../../paths";
import { Typography, Divider } from "antd";
import { InstitutionsDocument } from "../../queries/institutions.generated";
import { CurrentUserDocument } from "../../queries/currentUser.generated";
import PendingInstitutions from "../../components/institutions/PendingInstitutions";
import { UserRole } from "../../__generated__/graphql";
import { notEmpty } from "../../lib/array";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import InstitutionTable from "../../components/institutions/InstitutionTable";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Page() {
  const { loading, error, data } = useQuery(InstitutionsDocument);
  const nodes =
    data?.institutions?.edges?.map((e) => e.node).filter(notEmpty) || [];

  const currentUser = useQuery(CurrentUserDocument)?.data?.currentUser;

  useQueryHandler({ error });

  return (
    <Layout>
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        Institutions can manufacture{" "}
        <Link href={paths.components}>components</Link>, operate{" "}
        <Link href={paths.databases}>databases</Link> and create{" "}
        <Link href={paths.dataFormats}>data formats</Link> and{" "}
        <Link href={paths.methods}>methods</Link>.
      </Typography.Paragraph>
      <InstitutionTable loading={loading} institutions={nodes} />
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        The <Typography.Link href="/graphql/">GraphQL endpoint</Typography.Link>{" "}
        provides all information about institutions.
      </Typography.Paragraph>
      {currentUser && currentUser?.roles?.includes(UserRole.Verifier) && (
        <>
          <Divider />
          <Typography.Title level={2}>Pending Institutions</Typography.Title>
          <PendingInstitutions />
        </>
      )}
      {currentUser && (
        <Link href={paths.institutionCreate}>Create Institution</Link>
      )}
    </Layout>
  );
}

export default Page;
