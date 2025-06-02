import { OpenIdConnectAuthorization } from "../../../__generated__/__types__";
import { message, Popconfirm, Skeleton, Space, Table, TableProps } from "antd";
import Link from "next/link";
import { ApplicationProps } from "../applications/Application";
import { useEffect } from "react";
import { messageApolloError } from "../../../lib/apollo";
import { AuthorizationsByApplicationIdDocument, useAuthorizationsByApplicationIdQuery, useDeleteAuthorizationMutation } from "../../../queries/openIdConnectAuthorizations.graphql";

export default function AutorizationsTable({ applicationId }: ApplicationProps) {
    const { loading, error, data } = useAuthorizationsByApplicationIdQuery({
      variables: {
        applicationId: applicationId,
      },
    });
    const authorizations = data?.openIdConnectAuthorizations;
    const [deleteAuthorizationMutation] = useDeleteAuthorizationMutation({
        // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
        // See https://www.apollographql.com/docs/react/data/mutations/#options
        refetchQueries: [
            {
                query: AuthorizationsByApplicationIdDocument,
                variables: {
                    applicationId: applicationId,
                },
            },
        ],
    });

    useEffect(() => {
      if (error) {
        messageApolloError(error);
      }
    }, [error]);

    if (loading) {
        return <Skeleton active avatar title />;
    }

    const authorizationColumns: TableProps<OpenIdConnectAuthorization>['columns'] = [
        {
            title: "Satus",
            dataIndex: "status",
            key: "status",
        },
        {
            title: "Type",
            dataIndex: "type",
            key: "type",
        },
        {
            title: "Subject",
            dataIndex: "subject",
            key: "subject",
        },
        {
            title: 'Action',
            key: 'action',
            render: (_, authentication) => (
                <Space size="middle">
                    {authentication.canCurrentUserDeleteAuthorization ? (
                        <>
                            <Popconfirm
                                title="Are you sure to delete this authorization?"
                                onConfirm={async () => {
                                    const { data } = await deleteAuthorizationMutation({
                                        variables: {
                                            authorizationId: authentication.id!
                                        },
                                    });
                                    if (data?.deleteOpenIdConnectAuthorization.errors) {
                                        message.error(data?.deleteOpenIdConnectAuthorization.errors.toString())
                                    }
                                    else {
                                        message.success('Successfully deleted authorization ' + authentication.id)
                                    }
                                }}
                                okText="Yes"
                                cancelText="No"
                            >
                                <Link href="#">Delete</Link>
                            </Popconfirm>
                        </>
                    )
                        : <></>}

                </Space>
            ),
        },
    ];


    return <>
        <Table<OpenIdConnectAuthorization>
            loading={loading}
            columns={authorizationColumns}
            dataSource={authorizations as OpenIdConnectAuthorization[]}
        />
    </>;
}