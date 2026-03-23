import { Space, Table, TableProps } from "antd";
import { OpenIdConnectTokenPartialFragment } from "../../../queries/openIdConnect.generated";
import RevokeOpenIdConnectToken from "./RevokeOpenIdConnectToken";

interface TokenTableProps {
  tokens: OpenIdConnectTokenPartialFragment[];
}

export default function OpenIdConnectTokenTable({ tokens }: TokenTableProps) {
  const tokenColumns: TableProps<OpenIdConnectTokenPartialFragment>["columns"] =
    [
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
        title: "Action",
        key: "action",
        render: (_, token) => (
          <Space size="middle">
            {token.isAuthorizedToRevokeNode ? (
              <>
                <RevokeOpenIdConnectToken tokenId={token.uuid} />
              </>
            ) : (
              <></>
            )}
          </Space>
        ),
      },
    ];

  return (
    <Table<OpenIdConnectTokenPartialFragment>
      columns={tokenColumns}
      dataSource={tokens}
    />
  );
}
