import Layout from "../components/Layout";
import { Typography } from "antd";

const Index = () => {
  return (
    <Layout>
      <Typography.Paragraph>
        <Typography.Link href="https://www.buildingenvelopedata.org">
          buildingenvelopedata.org
        </Typography.Link>{" "}
        offers you access to a network of databases. It contains detailed
        optical data for thousands of building envelope components and can be
        used for example to calculate the energy performance of buildings. The
        data is ready to be used by software companies and advanced engineering
        offices.
      </Typography.Paragraph>
      <Typography.Paragraph>
        This website offers an overview of the components for which data is
        available and the databases of the network. It can also be used to
        search for data in all databases. In order to identify institutions,
        data formats and methods across the databases, they are managed by a
        metabase together with the components and databases. This website is the
        front end of the metabase.
      </Typography.Paragraph>
      <Typography.Paragraph>
        The metabase can be queried through its GraphQL endpoint. This is the
        most powerful way to query all databases. It is well suited to be used
        by software. The tabs of this website can offer only a part of the
        functions of the GraphQL endpoint.
      </Typography.Paragraph>
    </Layout>
  );
};

export default Index;
