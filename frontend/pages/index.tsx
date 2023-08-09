import Layout from "../components/Layout";
import { Typography } from "antd";
import Link from "next/link";
import paths from "../paths";
import Image from "next/image";
import overviewImage from "../public/overview.png";
import germanFederalMinistry from "../public/german-federal-ministry-for-economic-affairs-and-climate-action.png";

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
        <Typography.Paragraph>
          With{" "}
          <Typography.Link
            href={paths.dataFormat("9ca9e8f5-94bf-4fdd-81e3-31a58d7ca708")}
          >
            BED-JSON
          </Typography.Link>
          , <Link href={paths.home}>buildingenvelopedata.org</Link> offers a
          general format for optical, calorimetric and photovoltaic data sets.
          It is defined by the JSON Schemas of the{" "}
          <Typography.Link href="https://github.com/building-envelope-data/api">
            buildingenvelopedata.org API specification
          </Typography.Link>
          . Other data formats are also available. The{" "}
          <Typography.Link href="https://github.com/building-envelope-data/metabase">
            source code
          </Typography.Link>{" "}
          of the metabase is available at{" "}
          <Typography.Link href="https://github.com">GitHub</Typography.Link>.
        </Typography.Paragraph>
        <Image
          src={overviewImage}
          alt="Schematic depiction of how users like architects, planners, or engineers can use the metabase to find products and data in and across databases."
          style={{ maxWidth: "100%", height: "auto" }}
        />
        <Typography.Title level={2}>Acknowledgements</Typography.Title>
        <Typography.Paragraph>
          This work was funded by the German Federal Ministry for Economic Affairs and Climate Action under Grants 03ET1560A and 03EN1070A, based on a decision by the German Bundestag, by a Fraunhofer ICON Grant and by the Assistant Secretary for Energy Efficiency and Renewable Energy, Building Technologies Program, of the U.S. Department of Energy, under Contract No. DE-AC02-05CH11231.
        </Typography.Paragraph>
        <Image
          src={germanFederalMinistry}
          alt="Coat of arms of Germany with the text: Supported by the German Federal Ministry for Economic Affairs and Climate Action on the basis of a decision by the German Bundestag."
          style={{ maxWidth: "50%", height: "auto" }}
        />
      </div>
    </Layout>
  );
}

export default Page;
