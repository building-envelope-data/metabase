import { Space, Table, TableProps } from "antd";
import { ApplicationDocument, TokenPartialFragment } from "../../../queries/openIdConnect.graphql";
import { Scalars } from "../../../__generated__/__types__";
import RevokeToken from "./RevokeToken";

export type TokenTableProps = {
    applicationId: Scalars["Uuid"];
    tokens: TokenPartialFragment[];
};

export default function TokenTable({ applicationId, tokens }: TokenTableProps) {
    const tokenColumns: TableProps<TokenPartialFragment>['columns'] = [
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
            title: "Expiration Date",
            dataIndex: "expirationDate",
            key: "expirationDate",
        },
        {
            title: 'Action',
            key: 'action',
            render: (_, token) => (
                <Space size="middle">
                    {token.isAuthorizedToRevokeToken ? (
                        <>
                            <RevokeToken
                                tokenId={token.uuid}
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

    return <Table<TokenPartialFragment>
        columns={tokenColumns}
        dataSource={tokens}
    />;
}