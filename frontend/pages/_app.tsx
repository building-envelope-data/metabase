import { AppProps } from "next/app";
import { useApollo } from "../lib/apollo";
import { ApolloProvider } from "@apollo/client/react";
import { CookiesProvider } from "react-cookie";
import { ConfigProvider, message } from "antd";
import '../styles/global.css';

export default function App({ Component, pageProps }: AppProps) {
  const apolloClient = useApollo(pageProps.initialApolloState);

  message.config({
    top: 100,
    duration: 2,
  });

  return (
    <ConfigProvider >
      <ApolloProvider client={apolloClient}>
        <CookiesProvider>
          <Component {...pageProps} />
        </CookiesProvider>
      </ApolloProvider>
    </ConfigProvider>
  );
}
