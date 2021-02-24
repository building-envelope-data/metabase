import { useEffect } from 'react'
import { useRouter } from 'next/router'
import {
    useConfirmUserEmailMutation,
} from '../../queries/currentUser.graphql'
import { initializeApollo } from '../../lib/apollo'
import Layout from '../../components/Layout'
import paths from '../../paths'

function ConfirmUserEmail() {
    const router = useRouter()
    const { email, confirmationCode } = router.query
    const apolloClient = initializeApollo()
    const [confirmUserEmailMutation] = useConfirmUserEmailMutation()

    useEffect(() => {
        const confirmUserEmail = async () => {
            if (router.isReady) {
                if (typeof email === "string" &&
                    typeof confirmationCode === "string"
                ) {
                    await confirmUserEmailMutation({
                        variables: {
                            email: email,
                            confirmationCode: confirmationCode,
                        }
                    })
                    // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-confirmUserEmail
                    await apolloClient.resetStore()
                    await router.push(paths.home)
                }
            }
        }
        confirmUserEmail()
    }, [email, confirmationCode])

    return (
        <Layout>
            <p>Confirming email ...</p>
        </Layout>
    )
}

export default ConfirmUserEmail