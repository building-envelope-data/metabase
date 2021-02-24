import { AppProps } from 'next/app'
import { ApolloProvider } from '@apollo/client'
import { useApollo } from '../lib/apollo'

import 'antd/dist/antd.css'
import '../styles/vars.css'
import '../styles/global.css'

export default function App({ Component, pageProps }: AppProps) {
  const apolloClient = useApollo(pageProps.initialApolloState)

  return (
    <ApolloProvider client={apolloClient}>
      <Component {...pageProps} />
    </ApolloProvider>
  )
}
