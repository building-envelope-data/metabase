import { useMutation } from "@apollo/client/react";
import { useEffect, useRef } from "react";
import { useRouter } from "next/router";
import {
  LogoutUserDocument,
  LogoutUserMutation,
} from "../../queries/currentUser.generated";
import { apolloClient } from "../../lib/apollo";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

function Logout() {
  const router = useRouter();
  const hasCalledMutation = useRef(false);

  const [logoutUserMutation] = useMutation(LogoutUserDocument);
  const { withMutationHandler, messageErrors } =
    useMutationHandler<LogoutUserMutation>({
      getErrors: (data) => data.logoutUser.errors,
    });

  useEffect(() => {
    if (!router.isReady) return;
    if (hasCalledMutation.current) return;
    const logout = async () => {
      hasCalledMutation.current = true;
      withMutationHandler(logoutUserMutation, {
        onSuccess: async () => {
          // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
          await apolloClient.resetStore();
          await fetch(paths.antiforgeryToken);
          await router.push(paths.openIdConnectClientLogin);
        },
        onError: messageErrors,
      });
    };
    logout();
  }, [router, logoutUserMutation, withMutationHandler, messageErrors]);

  return (
    <Layout>
      <p>Logging out ...</p>
    </Layout>
  );
}

export default Logout;
