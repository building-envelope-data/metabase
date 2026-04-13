import { useQuery } from "@apollo/client/react";
import Layout from "../../components/Layout";
import OpenIdConnectApplicationTable from "../../components/openIdConnect/applications/OpenIdConnectApplicationTable";
import paths from "../../paths";
import { ApplicationsDocument } from "../../queries/openIdConnect.generated";
import { useRequireAuth } from "../../lib/hooks/useRequireAuth";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";

function Page() {
  const { authenticated } = useRequireAuth({ returnTo: paths.openIdConnect });
  const { loading, error, data } = useQuery(ApplicationsDocument);
  useQueryHandler({ error });

  return (
    <Layout>
      <OpenIdConnectApplicationTable
        loading={!authenticated || loading}
        applications={data?.openIdConnectApplications || []}
      />
    </Layout>
  );
}

export default Page;
