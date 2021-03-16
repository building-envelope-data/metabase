import { AppProps } from "next/app";
import { Provider } from "next-auth/client";
import { ApolloProvider } from "@apollo/client";
import { useApollo } from "../lib/apollo";
import { useSession } from "next-auth/client";

import "antd/dist/antd.css";
import "../styles/vars.css";
import "../styles/global.css";

// We first use `Provider` from `next-auth/client` to allow the usage of
// `useSession`. We then use `useSession` to fetch the session, in particular,
// its access token. The access token is then used in `useApollo` as `Bearer` in
// the `Authorization` header for all GraphQL requests subsequently done via
// Apollo.

export default function App(appProps: AppProps) {
  return (
    <Provider session={appProps.pageProps.session}>
      <UseSession {...appProps} />
    </Provider>
  );
}

function UseSession(appProps: AppProps) {
  const [session, loading] = useSession();

  // `useSession` above makes a request to `/api/auth/session` and waits for it
  // to finish making it the bottleneck. Instead of doing that to add the access
  // token as `Authorization` Bearer request header to the Apollo client, we
  // could add an endpoint `/api/graphql` that redirects to the backend GraphQL
  // endpoint as remarked on https://github.com/nextauthjs/next-auth/issues/643
  // (see also https://next-auth.js.org/tutorials/securing-pages-and-api-routes)
  // TODO Do what is suggested above?
  if (loading) {
    return null;
  }

  return <UseApollo session={session} {...appProps} />;
}

function UseApollo({
  session,
  Component,
  pageProps,
}: { session: { accessToken?: string } | null | undefined } & AppProps) {
  const apolloClient = useApollo(
    pageProps.initialApolloState,
    session?.accessToken
  );

  return (
    <ApolloProvider client={apolloClient}>
      <Component {...pageProps} />
    </ApolloProvider>
  );
}
