import Layout from "../../components/Layout";
import paths from "../../paths";
import { useRequireAuth } from "../../lib/hooks/useRequireAuth";
import PaginatedOpenIdConnectApplications from "../../components/openIdConnect/applications/PaginatedOpenIdConnectApplications";

function Page() {
  const { authenticated } = useRequireAuth({
    returnTo: paths.openIdConnect,
  });

  return (
    <Layout>
      <PaginatedOpenIdConnectApplications showJump loading={!authenticated} />
    </Layout>
  );
}

export default Page;
