import { AppProps } from "next/app";
import { apolloClient } from "../lib/apollo";
import { ApolloProvider } from "@apollo/client/react";
import { CookiesProvider } from "react-cookie";
import { App, ConfigProvider } from "antd";
import "../styles/global.css";

export default function NextApp({ Component, pageProps }: AppProps) {
  return (
    <ConfigProvider>
      <ApolloProvider client={apolloClient}>
        <CookiesProvider>
          <App>
            <Component {...pageProps} />
          </App>
        </CookiesProvider>
      </ApolloProvider>
    </ConfigProvider>
  );
}
