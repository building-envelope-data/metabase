import { useRouter } from "next/router";
import { useEffect } from "react";
import { useQuery } from "@apollo/client/react";
import {
  CurrentUserDocument,
  CurrentUserPartialFragment,
} from "../../queries/currentUser.generated";
import { redirectToLoginPage } from "../redirect";
import { Route } from "next";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";

interface UseRequireAuthProps {
  returnTo: Route;
}

type UseRequireAuthResponse =
  | { authenticated: true; currentUser: CurrentUserPartialFragment }
  | { authenticated: false; currentUser: null | undefined };

export function useRequireAuth({
  returnTo,
}: UseRequireAuthProps): UseRequireAuthResponse {
  const router = useRouter();

  const { loading, data, error } = useQuery(CurrentUserDocument);
  useQueryHandler({ error });
  const currentUser = data?.currentUser;
  const shouldRedirect = !loading && !error && !currentUser;
  const authenticated = !loading && !error && currentUser;

  useEffect(() => {
    if (router.isReady && shouldRedirect) {
      redirectToLoginPage(router, returnTo);
    }
  }, [router, shouldRedirect, returnTo]);

  if (!authenticated) {
    return { authenticated: false, currentUser: null };
  }
  return { authenticated: true, currentUser };
}
