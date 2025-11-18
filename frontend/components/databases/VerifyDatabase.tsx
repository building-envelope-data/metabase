import { useMutation } from '@apollo/client/react';
import { Button, message } from "antd";
import {
  VerifyDatabaseDocument,
  DatabasesDocument,
  PendingDatabasesDocument,
  DatabaseDocument,
} from "../../queries/databases.generated";
import { Scalars } from "../../__generated__/__types__";
import { useState } from "react";

export type VerifyDatabaseProps = {
  databaseId: Scalars["Uuid"];
};

export default function VerifyDatabase({ databaseId }: VerifyDatabaseProps) {
  const [verifyDatabaseMutation] = useMutation(VerifyDatabaseDocument, {
    // TODO Verify the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-verifys
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: DatabaseDocument,
        variables: {
          uuid: databaseId,
        },
      },
      {
        query: DatabasesDocument,
      },
      {
        query: PendingDatabasesDocument,
      },
    ],
  });
  const [verifying, setVerifying] = useState(false);

  const verify = async () => {
    try {
      setVerifying(true);
      // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
      const { error, data } = await verifyDatabaseMutation({
        variables: {
          databaseId: databaseId,
        },
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.verifyDatabase?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.verifyDatabase?.errors.map((error) => error.message).join(" ")
        );
      }
    } catch (error) {
      // TODO Handle properly.
      console.log("Failed:", error);
    } finally {
      setVerifying(false);
    }
  };

  return (
    <Button onClick={() => verify()} loading={verifying}>
      Verify
    </Button>
  );
}
