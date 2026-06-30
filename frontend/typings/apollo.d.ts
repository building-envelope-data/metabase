import "@apollo/client";

// Inspired by https://www.apollographql.com/docs/react/data/typescript#how-to-declare-default-options
declare module "@apollo/client" {
  namespace ApolloClient {
    namespace DeclareDefaultOptions {
      // Affects client.watchQuery() and React hooks (useQuery, useSuspenseQuery, etc.)
      interface WatchQuery {
        errorPolicy: "all";
      }
      // Affects client.query()
      interface Query {
        errorPolicy: "all";
      }
      // Affects client.mutate()
      interface Mutate {
        errorPolicy: "all";
      }
    }
  }
}
