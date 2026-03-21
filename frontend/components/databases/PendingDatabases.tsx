import { useQuery } from "@apollo/client/react";
import { List } from "antd";
import { PendingDatabasesDocument } from "../../queries/databases.generated";
import Link from "next/link";
import paths from "../../paths";
import VerifyDatabase from "./VerifyDatabase";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";

export type PendingDatabasesProps = {};

export default function PendingDatabases({}: PendingDatabasesProps) {
  const { data, loading, error } = useQuery(PendingDatabasesDocument);
  useQueryHandler({ error });

  return (
    <>
      <List
        size="small"
        loading={loading}
        dataSource={data?.pendingDatabases?.edges?.map((e) => e.node) || []}
        renderItem={(item) => (
          <List.Item>
            <Link href={paths.database(item?.uuid)}>{item?.name}</Link>
            {item.isAuthorizedToVerifyNode && (
              <VerifyDatabase databaseId={item?.uuid} />
            )}
          </List.Item>
        )}
      />
    </>
  );
}
