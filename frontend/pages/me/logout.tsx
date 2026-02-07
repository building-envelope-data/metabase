import { useMutation } from "@apollo/client/react";
import { useEffect } from "react";
import { useRouter } from "next/router";
import { LogoutUserDocument } from "../../queries/currentUser.generated";
import { apolloClient } from "../../lib/apollo";
import Layout from "../../components/Layout";
import paths from "../../paths";

function Logout() {
  const router = useRouter();
  const [logoutUserMutation] = useMutation(LogoutUserDocument);

  useEffect(() => {
    const logout = async () => {
      if (router.isReady) {
        await logoutUserMutation();
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        await apolloClient.resetStore();
        await fetch(paths.antiforgeryToken);
        await router.push(paths.openIdConnectClientLogin);
      }
    };
    logout();
  }, [router, logoutUserMutation, apolloClient]);

  return (
    <Layout>
      <p>Logging out ...</p>
    </Layout>
  );
}

export default Logout;
