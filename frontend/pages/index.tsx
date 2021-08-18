import Layout from "../components/Layout";
import { Typography } from "antd";
import Link from "next/link";
import paths from "../paths";

function Page() {
  return (
    <Layout>
      <div style={{ maxWidth: 768 }}>
        <Typography.Paragraph>
          <Link href={paths.home}>buildingenvelopedata.org</Link> offers you
          access to a network of <Link href={paths.databases}>databases</Link>.
          It contains detailed <Link href={paths.data}>optical data</Link> for
          thousands of building envelope{" "}
          <Link href={paths.components}>components</Link> and can be used for
          example to calculate the energy performance of buildings. The{" "}
          <Link href={paths.data}>data</Link> is ready to be used by software
          companies and advanced engineering offices.
        </Typography.Paragraph>
        <Typography.Paragraph>
          This website offers an overview of the{" "}
          <Link href={paths.components}>components</Link> for which{" "}
          <Link href={paths.data}>data</Link> is available and the{" "}
          <Link href={paths.databases}>databases</Link> of the network. It can
          also be used to search for <Link href={paths.data}>data</Link> in all{" "}
          <Link href={paths.databases}>databases</Link>. In order to identify{" "}
          <Link href={paths.institutions}>institutions</Link>,{" "}
          <Link href={paths.dataFormats}>data formats</Link> and{" "}
          <Link href={paths.methods}>methods</Link> across the{" "}
          <Link href={paths.databases}>databases</Link>, they are managed by a
          metabase together with the{" "}
          <Link href={paths.components}>components</Link> and{" "}
          <Link href={paths.databases}>databases</Link>. This website is the
          front end of the metabase.
        </Typography.Paragraph>
        <Typography.Paragraph>
          The metabase can be queried through its{" "}
          <Typography.Link
            href={`${process.env.NEXT_PUBLIC_METABASE_URL}/graphql/`}
          >
            GraphQL endpoint
          </Typography.Link>
          . This is the most powerful way to query all{" "}
          <Link href={paths.databases}>databases</Link>. It is well suited to be
          used by software. The tabs of this website can offer only a part of
          the functions of the{" "}
          <Typography.Link
            href={`${process.env.NEXT_PUBLIC_METABASE_URL}/graphql/`}
          >
            GraphQL endpoint
          </Typography.Link>
          .
        </Typography.Paragraph>
      </div>
    </Layout>
  );
}

export default Page;
