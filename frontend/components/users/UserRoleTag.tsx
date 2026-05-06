import { useMutation } from "@apollo/client/react";
import { Scalars, UserRole } from "../../__generated__/graphql";
import {
  RemoveUserRoleDocument,
  RemoveUserRoleMutation,
  UserDocument,
  UsersDocument,
} from "../../queries/users.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";
import EnumTag from "../EnumTag";

interface Props {
  userId: Scalars["Uuid"]["input"];
  role: UserRole;
  canRemove: boolean;
}

export function UserRoleTag({ userId, role, canRemove }: Props) {
  const [removeUserRoleMutation] = useMutation(RemoveUserRoleDocument, {
    refetchQueries: [
      {
        query: UsersDocument,
      },
      {
        query: UserDocument,
        variables: {
          uuid: userId,
        },
      },
    ],
  });

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<RemoveUserRoleMutation>({
      getErrors: (data) => data.removeUserRole.errors,
    });

  const remove = () =>
    withMutationHandler(
      () =>
        removeUserRoleMutation({
          variables: {
            input: {
              userId: userId,
              role: role,
            },
          },
        }),
      {
        onError: messageErrors,
      },
    );

  return (
    <EnumTag
      closable={canRemove}
      closeIcon={
        <SafeDeleteButton
          type="icon"
          kind="remove"
          onConfirm={remove}
          deleting={mutating}
        />
      }
      color="magenta"
    >
      {role}
    </EnumTag>
  );
}
