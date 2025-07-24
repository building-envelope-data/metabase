import { Result, Skeleton, Space, Table, TableProps } from "antd";
import { useEffect } from "react";
import { messageApolloError } from "../../../lib/apollo";
import { AuthorizationPartialFragment, AuthorizationsDocument, useAuthorizationsQuery } from "../../../queries/openIdConnectAuthorizations.graphql";
import { Scalars } from "../../../__generated__/__types__";
import DeleteAuthorization from "./DeleteAuthorization";

export type AuthorizationTableProps = {
    applicationId: Scalars["Uuid"];
};

export default function AutorizationTable({ applicationId }: AuthorizationTableProps) {
    const { loading, error, data } = useAuthorizationsQuery({
      variables: {
        applicationId: applicationId,
      },
    });
    const authorizations = data?.openIdConnectApplication?.authorizations.edges.map(edge => edge.node) as AuthorizationPartialFragment[];

    useEffect(() => {
      if (error) {
        messageApolloError(error);
      }
    }, [error]);

    if (loading) {
        return <Skeleton active avatar title />;
    }

    if (!authorizations) {
      return (
        <Result
          status="500"
          title="500"
          subTitle="Sorry, something went wrong."
        />
      );
    }

    const authorizationColumns: TableProps<AuthorizationPartialFragment>['columns'] = [
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
            render: (_, authorization) => (
                <Space size="middle">
                    {authorization.canCurrentUserDeleteNode ? (
                        <>
                            <DeleteAuthorization
                                authorizationId={authorization.uuid}
                                refetchQueries={[{
                                    query: AuthorizationsDocument,
                                    variables: {
                                        applicationId: applicationId,
                                    }
                                }]}
                            />
                        </>
                    )
                        : <></>}

                </Space>
            ),
        },
    ];


    return <>
        <Table<AuthorizationPartialFragment>
            loading={loading}
            columns={authorizationColumns}
            dataSource={authorizations}
        />
    </>;
}