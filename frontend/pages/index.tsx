import Link from 'next/link'
import { useState } from 'react' // useEffect
// import { useRouter } from 'next/router'
import {
  useCurrentUserQuery,
  useChangeUserEmailMutation,
  CurrentUserDocument,
} from '../queries/currentUser.graphql'
// import { initializeApollo } from '../lib/apollo'
import Layout from '../components/Layout'

const Index = () => {
  // const router = useRouter()
  const { loading, error, data } = useCurrentUserQuery()
  const currentUser = data?.currentUser
  // const shouldRedirect = !(loading || error || currentUser)
  const [newEmail, setNewEmail] = useState('')
  const [changeUserEmailMutation] = useChangeUserEmailMutation({
    update(cache, { data }) {
      // Read the data from our cache for this query.
      /* const { currentUser } = cache.readQuery({ query: CurrentUserDocument }) */
      /* const newCurrentUser = { ...currentUser } */
      // Add our comment from the mutation to the end.
      /* newCurrentUser.email = data.changeUserEmail.user.email */
      // Write our data back to the cache.
      if (data?.changeUserEmail?.user)
        cache.writeQuery(
          {
            query: CurrentUserDocument,
            data: {
              currentUser: data.changeUserEmail.user
            }
          }
        )
    }
  })
  const onChangeUserEmail = () => {
    changeUserEmailMutation({
      variables: {
        newEmail: newEmail,
      },
    })
  }

  // useEffect(() => {
  //   if (shouldRedirect) {
  //     router.push('/login')
  //   }
  //   // eslint-disable-next-line react-hooks/exhaustive-deps
  // }, [shouldRedirect])

  if (loading) {
    return (
      <Layout>
        <p>Loading ...</p>
      </Layout>
    )
  }

  if (error) {
    return (
      <Layout>
        <p>{error.message}</p>
      </Layout>
    )
  }

  if (currentUser) {
    return (
      <Layout>
        <div>
          You're signed in as {currentUser.email} and you're {currentUser.id}. Go to the{' '}
          <Link href="/about">
            <a>about</a>
          </Link>{' '}
          page.
          <div>
            <input
              type="text"
              placeholder="your new email..."
              onChange={(e) => setNewEmail(e.target.value)}
            />
            <input type="button" value="change" onClick={onChangeUserEmail} />
          </div>
        </div>
      </Layout>
    )
  }

  return (
    <Layout>You're not sigend in.</Layout>
  )
}

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

export default Index