import { useQuery } from "@apollo/client/react";
import { stringifyApolloError } from "../../lib/apollo";
import Layout from "../../components/Layout";
import { useEffect } from "react";
import { CurrentUserDocument } from "../../queries/currentUser.generated";
import ApplicationTable from "../../components/openIdConnect/applications/ApplicationTable";
import { useRouter } from "next/router";
import paths, { redirectToLoginPage } from "../../paths";
import {
  ApplicationPartialFragment,
  ApplicationsDocument,
} from "../../queries/openIdConnect.generated";
import { App } from "antd";

function Page() {
  const { loading, error, data } = useQuery(ApplicationsDocument);
  const currentUser = useQuery(CurrentUserDocument)?.data?.currentUser;
  const router = useRouter();
  const shouldRedirect = !(loading || error || currentUser);
  const { message } = App.useApp();

  useEffect(() => {
    if (error) {
      message.error(stringifyApolloError(error));
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
        applications={
          (data?.openIdConnectApplications as ApplicationPartialFragment[]) ||
          []
        }
      />
    </Layout>
  );
}

export default Page;
