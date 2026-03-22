import { useMutation } from "@apollo/client/react";
import { Scalars, UserRole } from "../../__generated__/graphql";
import {
  RemoveUserRoleDocument,
  RemoveUserRoleMutation,
  UserDocument,
  UsersDocument,
} from "../../queries/users.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import { Tag } from "antd";
import { SyncOutlined } from "@ant-design/icons";

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
    <Tag
      icon={mutating && <SyncOutlined spin />}
      closable={(!mutating && canRemove) || false}
      onClose={() => remove()}
      color="magenta"
    >
      {role}
    </Tag>
  );
}
