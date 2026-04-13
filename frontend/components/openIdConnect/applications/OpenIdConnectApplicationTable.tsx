import { Skeleton, Table, TableProps } from "antd";
import paths from "../../../paths";
import { OpenIdConnectApplicationsPartialFragment } from "../../../queries/openIdConnect.generated";
import { getUuidColumnProps } from "../../../lib/table";
import { useState } from "react";
import { setMapValue } from "../../../lib/freeTextFilter";

interface ApplicationsProps {
  loading: boolean;
  applications: OpenIdConnectApplicationsPartialFragment[];
}

export default function OpenIdConnectApplicationTable({
  loading,
  applications,
}: ApplicationsProps) {
  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

  if (loading) {
    return <Skeleton active avatar title />;
  }

  const applicationColumns: TableProps<OpenIdConnectApplicationsPartialFragment>["columns"] =
    [
      getUuidColumnProps<(typeof applications)[0]>(
        onFilterTextChange,
        (x) => filterText.get(x),
        paths.openIdConnectApplication,
      ),
      {
        title: "Client ID",
        dataIndex: "clientId",
        key: "clientId",
      },
      {
        title: "Name",
        dataIndex: "displayName",
        key: "displayName",
      },
      {
        title: "Redirect URL",
        dataIndex: "redirectUri",
        key: "redirectUri",
      },
      {
        title: "Logout Redirect",
        dataIndex: "postLogoutRedirectUri",
        key: "postLogoutRedirectUri",
      },
    ];

  return (
    <Table<OpenIdConnectApplicationsPartialFragment>
      loading={loading}
      columns={applicationColumns}
      dataSource={applications}
    />
  );
}
