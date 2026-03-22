import { useQuery } from "@apollo/client/react";
import { List } from "antd";
import { PendingInstitutionsDocument } from "../../queries/institutions.generated";
import Link from "next/link";
import paths from "../../paths";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import VerifyInstitution from "./VerifyInstitution";

export type PendingInstitutionsProps = {};

export default function PendingInstitutions({}: PendingInstitutionsProps) {
  const { data, loading, error } = useQuery(PendingInstitutionsDocument);
  useQueryHandler({ error });

  return (
    <>
      <List
        size="small"
        loading={loading}
        dataSource={data?.pendingInstitutions?.edges?.map((e) => e.node) || []}
        renderItem={(item) => (
          <List.Item>
            <Link href={paths.institution(item.uuid)}>{item.name}</Link>
            {item.isAuthorizedToVerifyNode && (
              <VerifyInstitution institutionId={item.uuid} />
            )}
          </List.Item>
        )}
      />
    </>
  );
}
