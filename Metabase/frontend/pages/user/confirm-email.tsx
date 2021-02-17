import { useEffect } from 'react'
import { useRouter } from 'next/router'
import {
    useConfirmUserEmailMutation,
} from '../../lib/currentUser.graphql'
import { initializeApollo } from '../../lib/apollo'
import Layout from '../../components/Layout'

function ConfirmUserEmail() {
    const router = useRouter()
    const { email, confirmationCode } = router.query
    const apolloClient = initializeApollo()
    const [confirmUserEmailMutation] = useConfirmUserEmailMutation()

    useEffect(() => {
        if (router.isReady) {
            confirmUserEmailMutation({
                variables: {
                    email: email,
                    confirmationCode: confirmationCode,
                }
            }).then(() => {
                // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-confirmUserEmail
                apolloClient.resetStore().then(() => {
                    router.push('/')
                })
            })
        }
    }, [email, confirmationCode])

    return (
        <Layout>
            <p>Confirming email ...</p>
        </Layout>
    )
}

export default ConfirmUserEmail