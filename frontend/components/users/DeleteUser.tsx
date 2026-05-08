import { useMutation } from "@apollo/client/react";
import { useRouter } from "next/router";
import paths from "../../paths";
import {
  UsersDocument,
  DeleteUserDocument,
  DeleteUserMutation,
} from "../../queries/users.generated";
import { Scalars } from "../../__generated__/graphql";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";

interface DeleteUserProps {
  userId: Scalars["Uuid"]["input"];
}

export default function DeleteUser({ userId }: DeleteUserProps) {
  const router = useRouter();

  const [deleteUserMutation] = useMutation(DeleteUserDocument, {
    refetchQueries: [UsersDocument],
  });

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<DeleteUserMutation>({
      getErrors: (data) => data.deleteUser.errors,
    });

  const mutate = async () => {
    withMutationHandler(
      () =>
        deleteUserMutation({
          variables: {
            input: {
              userId: userId,
            },
          },
        }),
      {
        onSuccess: () => router.push(paths.users),
        onError: messageErrors,
      },
    );
  };

  return (
    <SafeDeleteButton kind="delete" onConfirm={mutate} deleting={mutating} />
  );
}
