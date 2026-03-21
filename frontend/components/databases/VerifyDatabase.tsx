import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  VerifyDatabaseDocument,
  DatabasesDocument,
  PendingDatabasesDocument,
  DatabaseDocument,
  VerifyDatabaseMutation,
} from "../../queries/databases.generated";
import { Scalars } from "../../__generated__/graphql";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

export type VerifyDatabaseProps = {
  databaseId: Scalars["Uuid"]["input"];
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

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<VerifyDatabaseMutation>({
      getErrors: (data) => data.verifyDatabase.errors,
    });

  const verify = async () => {
    withMutationHandler(
      () =>
        verifyDatabaseMutation({
          variables: {
            input: {
              databaseId: databaseId,
            },
          },
        }),
      {
        onError: messageErrors,
      },
    );
  };

  return (
    <Button onClick={() => verify()} loading={mutating}>
      Verify
    </Button>
  );
}
