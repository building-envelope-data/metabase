import { Space, Table, TableProps } from "antd";
import { AuthorizationPartialFragment } from "../../../queries/openIdConnect.generated";
import DeleteAuthorization from "./DeleteAuthorization";
import { ApplicationDocument } from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/__types__";

export type AuthorizationTableProps = {
    applicationId: Scalars["Uuid"];
    authorizations: AuthorizationPartialFragment[];
};

export default function AutorizationTable({ applicationId, authorizations }: AuthorizationTableProps) {
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
                    {authorization.isAuthorizedToDeleteNode ? (
                        <>
                            <DeleteAuthorization
                                authorizationId={authorization.uuid}
                                refetchQueries={[{
                                    query: ApplicationDocument,
                                    variables: {
                                        uuid: applicationId,
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
            columns={authorizationColumns}
            dataSource={authorizations}
        />
    </>;
}