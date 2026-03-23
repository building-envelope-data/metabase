import { Space, Table, TableProps } from "antd";
import { OpenIdConnectAuthorizationPartialFragment } from "../../../queries/openIdConnect.generated";
import DeleteOpenIdConnectAuthorization from "./DeleteOpenIdConnectAuthorization";
import { ApplicationDocument } from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/graphql";

interface AuthorizationTableProps {
  applicationId: Scalars["Uuid"]["input"];
  authorizations: OpenIdConnectAuthorizationPartialFragment[];
}

export default function OpenIdConnectAutorizationTable({
  applicationId,
  authorizations,
}: AuthorizationTableProps) {
  const authorizationColumns: TableProps<OpenIdConnectAuthorizationPartialFragment>["columns"] =
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
        title: "Action",
        key: "action",
        render: (_, authorization) => (
          <Space size="middle">
            {authorization.isAuthorizedToDeleteNode ? (
              <>
                <DeleteOpenIdConnectAuthorization
                  authorizationId={authorization.uuid}
                  refetchQueries={[
                    {
                      query: ApplicationDocument,
                      variables: {
                        uuid: applicationId,
                      },
                    },
                  ]}
                />
              </>
            ) : (
              <></>
            )}
          </Space>
        ),
      },
    ];

  return (
    <>
      <Table<OpenIdConnectAuthorizationPartialFragment>
        columns={authorizationColumns}
        dataSource={authorizations}
      />
    </>
  );
}
