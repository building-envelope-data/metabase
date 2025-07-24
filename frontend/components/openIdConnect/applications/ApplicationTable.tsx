import { Skeleton, Table, TableProps } from "antd";
import paths from "../../../paths";
import { ApplicationPartialFragment } from "../../../queries/openIdConnectApplications.graphql";
import { getUuidColumnProps } from "../../../lib/table";
import { useState } from "react";
import { setMapValue } from "../../../lib/freeTextFilter";
import { DocumentNode } from "graphql";
import DeleteApplication from "./DeleteApplication";

export type ApplicationsProps = {
    loading: boolean;
    applications: ApplicationPartialFragment[];
    refetchQueries: {query: DocumentNode, variables?: {[key: string]: any}}[];
};

export default function ApplicationTable({ loading, applications, refetchQueries }: ApplicationsProps) {
    const [filterText, setFilterText] = useState(() => new Map<string, string>());
    const onFilterTextChange = setMapValue(filterText, setFilterText);

    if (loading) {
        return <Skeleton active avatar title />;
    }

    const applicationColumns: TableProps<ApplicationPartialFragment>['columns'] = [
        getUuidColumnProps<(typeof applications)[0]>(
          onFilterTextChange,
          (x) => filterText.get(x),
          paths.openIdConnectApplication
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
        {
            title: 'Action',
            key: 'action',
            render: (_, application) => application.canCurrentUserManageNode ? (
                        <>
                            <DeleteApplication applicationId={application.uuid} refetchQueries={refetchQueries} />
                        </>
                    )
                        : <></>
        },
    ];

    return <Table<ApplicationPartialFragment>
        loading={loading}
        columns={applicationColumns}
        dataSource={applications}
    />;
}