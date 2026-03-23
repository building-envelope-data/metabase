import { Space, Table, TableProps } from "antd";
import { TokenPartialFragment } from "../../../queries/openIdConnect.generated";
import RevokeToken from "./RevokeToken";

export type TokenTableProps = {
  tokens: TokenPartialFragment[];
};

export default function TokenTable({ tokens }: TokenTableProps) {
  const tokenColumns: TableProps<TokenPartialFragment>["columns"] = [
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
              <RevokeToken tokenId={token.uuid} />
            </>
          ) : (
            <></>
          )}
        </Space>
      ),
    },
  ];

  return (
    <Table<TokenPartialFragment> columns={tokenColumns} dataSource={tokens} />
  );
}
