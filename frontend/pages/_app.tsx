import { AppProps } from "next/app";
import { apolloClient } from "../lib/apollo";
import { ApolloProvider } from "@apollo/client/react";
import { CookiesProvider } from "react-cookie";
import { App, ConfigProvider } from "antd";
import { ReactNode, useEffect, useState } from 'react';
import paths from "../paths";
import "../styles/global.css";

function AntiforgeryProvider({ children }: { children: ReactNode }) {
  const [loaded, setLoaded] = useState(false);

  useEffect(() => {
    fetch(paths.antiforgeryToken).then((_) => {
      setLoaded(true);
    });
  }, []);

  if (!loaded) {
    return null;
  }

  return children;
}

export default function NextApp({ Component, pageProps }: AppProps) {
  return (
    <ConfigProvider>
      <ApolloProvider client={apolloClient}>
        <CookiesProvider>
          <AntiforgeryProvider>
            <App>
              <Component {...pageProps} />
            </App>
          </AntiforgeryProvider>
        </CookiesProvider>
      </ApolloProvider>
    </ConfigProvider>
  );
}
