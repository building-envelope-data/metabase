import Layout from '../../components/Layout'
import Link from 'next/link'
import paths from "../../paths"
import { Skeleton, List, message } from "antd"
import { useInstitutionsQuery } from "../../queries/institutions.graphql"
// import { useState } from 'react'

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/
// {
// variables: {
//     first: pageSize,
//     after: cursor
// }
// }

function Institutions() {
    const pageSize = 100
    const { loading, error, data } = useInstitutionsQuery()
    // const [institutions, setInstitutions] = useState([])

    if (error) {
        message.error(error)
    }

    return (
        <Layout>
            {/* Inspired by https://ant.design/components/skeleton/#components-skeleton-demo-list */}
            <List
                itemLayout="vertical"
                size="large"
                dataSource={data?.institutions?.nodes || []}
                pagination={{
                    total: data?.institutions?.totalCount,
                    defaultCurrent: 1,
                    defaultPageSize: pageSize,
                    hideOnSinglePage: false,
                    responsive: true,
                    position: "bottom",
                    // onChange: (page, pageSize) => // TODO
                }}
                renderItem={item =>
                    <List.Item
                        key={item?.name}
                    >
                        <Skeleton
                            loading={item === null || loading}
                            active
                            avatar
                        >
                            <List.Item.Meta
                                //   avatar={<Avatar src={item.avatar} />}
                                title={
                                    <Link href={paths.institution(item?.uuid)}>
                                        {item?.name}
                                    </Link>}
                                description={item?.abbreviation}
                            />
                            {item?.description}
                        </Skeleton>
                    </List.Item>
                }
            >
            </List>
            <Link href={paths.institutionCreate}>Create Institution</Link>
        </Layout >
    )
}

export default Institutions