import { messageApolloError } from "../../lib/apollo";
import Layout from "../../components/Layout";
import { useEffect } from "react";
import { OpenIdConnectApplication } from "../../__generated__/__types__";
import { useCurrentUserQuery } from "../../queries/currentUser.graphql";
import ApplicationTable from "../../components/openIdConnect/applications/ApplicationTable";
import { useRouter } from "next/router";
import paths, { redirectToLoginPage } from "../../paths";
import { useApplicationsQuery } from "../../queries/openIdConnectApplications.graphql";

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
      <ApplicationTable editable={false} loading={loading} applications={data?.openIdConnectApplications as Array<OpenIdConnectApplication> || []}></ApplicationTable>
    </Layout>
  );
}

export default Page;
