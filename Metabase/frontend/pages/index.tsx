import Link from 'next/link'
import { useState } from 'react'
import {
  useCurrentUserQuery,
  useChangeUserEmailMutation,
  CurrentUserDocument,
} from '../lib/currentUser.graphql'
import { initializeApollo } from '../lib/apollo'

const Index = () => {
  const { currentUser } = useCurrentUserQuery().data!
  const [newEmail, setNewEmail] = useState('')
  const [changeUserEmailMutation] = useChangeUserEmailMutation()

  const onChangeEmail = () => {
    changeUserEmailMutation({
      variables: {
        newEmail: newEmail,
      },
      //Follow apollo suggestion to update cache
      //https://www.apollographql.com/docs/angular/features/cache-updates/#update
      update: (
        store,
        {
          data: {
            updateEmail: { email },
          },
        }
      ) => {
        // Read the data from our cache for this query.
        const { currentUser } = store.readQuery({ query: CurrentUserDocument })
        const newCurrentUser = { ...currentUser }
        // Add our comment from the mutation to the end.
        newCurrentUser.email = email
        // Write our data back to the cache.
        store.writeQuery({ query: CurrentUserDocument, data: { currentUser: newCurrentUser } })
      },
    })
  }

  if (currentUser)
    {
      return (
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
            <input type="button" value="change" onClick={onChangeEmail} />
          </div>
        </div>
      )
    }
    else
      {
        return (
          <div>You're not signed in.</div>
        )
      }
}

export async function getStaticProps() {
  const apolloClient = initializeApollo()

  await apolloClient.query({
    query: CurrentUserDocument,
  })

  return {
    props: {
      initialApolloState: apolloClient.cache.extract(),
    },
  }
}

export default Index
