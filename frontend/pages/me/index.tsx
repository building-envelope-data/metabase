import { useEffect } from "react";
import { useRouter } from "next/router";
import { useCurrentUserQuery } from "../../queries/currentUser.graphql";
import Layout from "../../components/Layout";
import paths from "../../paths";

function Index() {
  const router = useRouter();

  const { loading, error, data } = useCurrentUserQuery();
  const currentUser = data?.currentUser;
  const shouldRedirect = !(loading || error || currentUser);

  useEffect(() => {
    if (router.isReady && shouldRedirect) {
      router.push({
        pathname: paths.userLogin,
        query: { returnTo: paths.userCurrent },
      });
    }
  }, [router, shouldRedirect]);

  return <Layout></Layout>;
}

export default Index;
