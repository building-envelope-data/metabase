import { useEffect } from "react";
import { useRouter } from "next/router";
import { useLogoutUserMutation } from "../../queries/currentUser.graphql";
import { initializeApollo } from "../../lib/apollo";
import Layout from "../../components/Layout";
import paths from "../../paths";

function Logout() {
  const router = useRouter();
  const apolloClient = initializeApollo();
  const [logoutUserMutation] = useLogoutUserMutation();

  useEffect(() => {
    const logout = async () => {
      if (router.isReady) {
        await logoutUserMutation();
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        await apolloClient.resetStore();
        await router.push(paths.userLogin);
      }
    };
    logout();
  }, [router]);

  return (
    <Layout>
      <p>Logging out ...</p>
    </Layout>
  );
}

export default Logout;
