import { useMutation } from "@apollo/client/react";
import { useQuery } from "@apollo/client/react";
import { List, Button, message } from "antd";
import { useEffect, useState } from "react";
import {
  DatabaseDocument,
  DatabasesDocument,
  PendingDatabasesDocument,
  VerifyDatabaseDocument,
} from "../../queries/databases.generated";
import { Scalars } from "../../__generated__/graphql";
import Link from "next/link";
import paths from "../../paths";
import { stringifyApolloError } from "../../lib/apollo";

export type PendingDatabasesProps = {};

export default function PendingDatabases({ }: PendingDatabasesProps) {
  const { data, loading, error } = useQuery(PendingDatabasesDocument);
  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    if (error) {
      messageApi.error(stringifyApolloError(error));
    }
  }, [error]);

  const [verifyDatabaseMutation] = useMutation(VerifyDatabaseDocument);
  const [verifyingDatabase, setVerifyingDatabase] = useState(false);

  const verifyDatabase = async (databaseId: Scalars["Uuid"]["input"]) => {
    try {
      setVerifyingDatabase(true);
      const { error, data } = await verifyDatabaseMutation({
        variables: {
          input: {
            databaseId: databaseId,
          },
        },
        refetchQueries: [
          {
            query: DatabasesDocument,
          },
          {
            query: PendingDatabasesDocument,
          },
          {
            query: DatabaseDocument,
            variables: {
              uuid: databaseId,
            },
          },
        ],
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.verifyDatabase?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.verifyDatabase?.errors.map((error) => error.message).join(" "),
        );
      }
    } finally {
      setVerifyingDatabase(false);
    }
  };

  return (
    <>
      {contextHolder}
      <List
        size="small"
        loading={loading}
        dataSource={data?.pendingDatabases?.edges?.map((e) => e.node) || []}
        renderItem={(item) => (
          <List.Item>
            <Link href={paths.database(item?.uuid)} legacyBehavior>
              {item?.name}
            </Link>
            {item.isAuthorizedToVerifyNode && (
              <Button
                onClick={() => verifyDatabase(item?.uuid)}
                loading={verifyingDatabase}
              >
                Verify
              </Button>
            )}
          </List.Item>
        )}
      />
    </>
  );
}
