import { messageApolloError } from "../../lib/apollo";
import Layout from "../../components/Layout";
import { useEffect } from "react";
import { useCurrentUserQuery } from "../../queries/currentUser.graphql";
import ApplicationTable from "../../components/openIdConnect/applications/ApplicationTable";
import { useRouter } from "next/router";
import paths, { redirectToLoginPage } from "../../paths";
import { ApplicationPartialFragment, useApplicationsQuery } from "../../queries/openIdConnect.graphql";

function Page() {
  const { loading, error, data } = useApplicationsQuery();
  const currentUser = useCurrentUserQuery()?.data?.currentUser;
  const router = useRouter();
  const shouldRedirect = !(loading || error || currentUser);

  useEffect(() => {
    if (error) {
      messageApolloError(error);
    }
  }, [error]);

  useEffect(() => {
    if (router.isReady && shouldRedirect) {
      redirectToLoginPage(router, paths.openIdConnect);
    }
  }, [shouldRedirect, router]);

  return (
    <Layout>
      <ApplicationTable
        loading={loading}
        applications={data?.openIdConnectApplications as ApplicationPartialFragment[] || []}
      />
    </Layout>
  );
}

export default Page;
