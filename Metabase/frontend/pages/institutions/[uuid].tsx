import Layout from '../../components/Layout'
import { Typography, message, Skeleton } from "antd"
import { useInstitutionQuery } from "../../queries/institutions.graphql"
import { useRouter } from 'next/router'

function Institution() {
    const router = useRouter()
    const { uuid } = router.query
    const { loading, error, data } = useInstitutionQuery({
        variables: {
            uuid: uuid
        }
    })
    const institution = data?.institution

    if (error) {
        message.error(error)
    }

    if (loading || !institution) {
        return (
            <Layout>
                <Skeleton
                    active
                    avatar
                    title
                    loading={loading}
                />
            </Layout>
        )
    }

    return (
        <Layout>
            <Typography.Title>
                {institution.websiteLocator
                    ?
                    <Typography.Link href={institution.websiteLocator}>
                        {`${institution.name} (${institution.abbreviation})`}
                    </Typography.Link>
                    : `${institution.name} (${institution.abbreviation})`}
            </Typography.Title>
            <Typography.Text>
                {institution.description}
            </Typography.Text>
        </Layout >
    )
}

export default Institution