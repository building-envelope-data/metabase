import { useQuery } from "@apollo/client/react";
import { useEffect } from "react";
import { useRouter } from "next/router";
import { CurrentUserDocument } from "../../queries/currentUser.generated";
import Layout from "../../components/Layout";
import paths from "../../paths";

function Page() {
  const router = useRouter();

  const { loading, error, data } = useQuery(CurrentUserDocument);
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

export default Page;
