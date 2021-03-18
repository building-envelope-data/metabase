import { useCurrentUserQuery } from "../queries/currentUser.graphql";
import Layout from "../components/Layout";
import { Skeleton, Typography } from "antd";

const Index = () => {
  const { loading, error, data } = useCurrentUserQuery();
  const currentUser = data?.currentUser;
  // const shouldRedirect = !(loading || error || currentUser)

  // useEffect(() => {
  //   if (shouldRedirect) {
  //     router.push('/login')
  //   }
  //   // eslint-disable-next-line react-hooks/exhaustive-deps
  // }, [shouldRedirect])

  if (loading) {
    return (
      <Layout>
        <Skeleton />
      </Layout>
    );
  }

  if (error) {
    return (
      <Layout>
        <p>{error.message}</p>
      </Layout>
    );
  }

  if (currentUser) {
    return (
      <Layout>
        <Typography.Paragraph>
          You're signed in as {currentUser.name}, your email address it{" "}
          {currentUser.email}, and your UUID is {currentUser.uuid}.
        </Typography.Paragraph>
      </Layout>
    );
  }

  return <Layout>You're not sigend in.</Layout>;
};

// export async function getStaticProps() {
//   const apolloClient = initializeApollo()

//   await apolloClient.query({
//     query: CurrentUserDocument,
//   })

//   return {
//     props: {
//       initialApolloState: apolloClient.cache.extract(),
//     },
//   }
// }

export default Index;
