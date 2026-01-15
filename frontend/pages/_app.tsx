import { AppProps } from "next/app";
import { useApollo } from "../lib/apollo";
import { ApolloProvider } from "@apollo/client/react";
import { CookiesProvider } from "react-cookie";
import { App, ConfigProvider, message } from "antd";
import "../styles/global.css";

export default function NextApp({ Component, pageProps }: AppProps) {
  const apolloClient = useApollo(pageProps.initialApolloState);

  message.config({
    top: 100,
    duration: 2,
  });

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
