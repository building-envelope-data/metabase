import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  VerifyDatabaseDocument,
  DatabasesDocument,
  PendingDatabasesDocument,
  VerifyDatabaseMutation,
} from "../../queries/databases.generated";
import { Scalars } from "../../__generated__/graphql";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

interface VerifyDatabaseProps {
  databaseId: Scalars["Uuid"]["input"];
}

export default function VerifyDatabase({ databaseId }: VerifyDatabaseProps) {
  const [verifyDatabaseMutation] = useMutation(VerifyDatabaseDocument, {
    refetchQueries: [DatabasesDocument, PendingDatabasesDocument],
  });

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<VerifyDatabaseMutation>({
      getErrors: (data) => data.verifyDatabase.errors,
    });

  const mutate = async () => {
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
    <Button onClick={mutate} loading={mutating}>
      Verify
    </Button>
  );
}
