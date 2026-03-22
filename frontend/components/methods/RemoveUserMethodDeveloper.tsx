import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  MethodDocument,
  MethodsDocument,
} from "../../queries/methods.generated";
import { Scalars } from "../../__generated__/graphql";
import { UserDocument } from "../../queries/users.generated";
import {
  RemoveUserMethodDeveloperDocument,
  RemoveUserMethodDeveloperMutation,
} from "../../queries/userMethodDevelopers.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

interface Props {
  methodId: Scalars["Uuid"]["input"];
  userId: Scalars["Uuid"]["input"];
}

export default function RemoveUserMethodDeveloper({ methodId, userId }: Props) {
  const [removeUserMethodDeveloperMutation] = useMutation(
    RemoveUserMethodDeveloperDocument,
    {
      refetchQueries: [
        {
          query: MethodsDocument,
        },
        {
          query: MethodDocument,
          variables: {
            uuid: methodId,
          },
        },
        {
          query: UserDocument,
          variables: {
            uuid: userId,
          },
        },
      ],
    },
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<RemoveUserMethodDeveloperMutation>({
      getErrors: (data) => data.removeUserMethodDeveloper.errors,
    });

  const mutate = async () => {
    withMutationHandler(
      () =>
        removeUserMethodDeveloperMutation({
          variables: {
            input: {
              methodId: methodId,
              userId: userId,
            },
          },
        }),
      {
        onError: messageErrors,
      },
    );
  };

  return (
    <Button danger type="primary" onClick={mutate} loading={mutating}>
      Remove
    </Button>
  );
}
