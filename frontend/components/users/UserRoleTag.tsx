import { useMutation } from "@apollo/client/react";
import { Scalars, UserRole } from "../../__generated__/graphql";
import {
  RemoveUserRoleDocument,
  RemoveUserRoleMutation,
} from "../../queries/users.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";
import EnumTag from "../EnumTag";

interface Props {
  userId: Scalars["Uuid"]["input"];
  role: UserRole;
  canRemove: boolean;
}

export default function UserRoleTag({ userId, role, canRemove }: Props) {
  const [removeUserRoleMutation] = useMutation(RemoveUserRoleDocument);

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
