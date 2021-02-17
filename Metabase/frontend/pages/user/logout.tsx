import { useEffect } from 'react'
import { useRouter } from 'next/router'
import {
    useLogoutUserMutation,
} from '../../lib/currentUser.graphql'
import { initializeApollo } from '../../lib/apollo'
import Layout from '../../components/Layout'

function Logout() {
    const router = useRouter()
    const apolloClient = initializeApollo()
    const [logoutUserMutation] = useLogoutUserMutation()

    useEffect(() => {
        logoutUserMutation().then(() => {
            // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
            apolloClient.resetStore().then(() => {
                router.push('/login')
            })
        })
    }, [router, logoutUserMutation])

    return (
        <Layout>
            <p>Logging out ...</p>
        </Layout>
    )
}

export default Logout