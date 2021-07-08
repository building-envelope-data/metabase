import { Button, Divider, Typography, message, Skeleton } from "antd";
import {
  UsersDocument,
  useUserQuery,
  useDeleteUserMutation,
} from "../../queries/users.graphql";
import { Scalars } from "../../__generated__/__types__";
import { useCurrentUserQuery } from "../../queries/currentUser.graphql";
import { useEffect } from "react";
import { useRouter } from "next/router";
import paths from "../../paths";
import { useState } from "react";

export type UserProps = {
  userId: Scalars["Uuid"];
};

export const User: React.FunctionComponent<UserProps> = ({ userId }) => {
  const router = useRouter();
  const { loading, error, data } = useUserQuery({
    variables: {
      uuid: userId,
    },
  });
  const user = data?.user;
  const currentUser = useCurrentUserQuery()?.data?.currentUser;

  const [deleteUserMutation] = useDeleteUserMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: UsersDocument,
      },
    ],
  });
  const [deletingUser, setDeletingUser] = useState(false);

  const deleteUser = async () => {
    try {
      setDeletingUser(true);
      const { errors, data } = await deleteUserMutation({
        variables: {
          userId: userId,
        },
      });
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.deleteUser?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.deleteUser?.errors.map((error) => error.message).join(" ")
        );
      } else {
        await router.push(paths.users);
      }
    } finally {
      setDeletingUser(false);
    }
  };

  useEffect(() => {
    if (error) {
      message.error(error);
    }
  }, [error]);

  if (loading || !user) {
    return <Skeleton active avatar title />;
  }

  return (
    <>
      <Typography.Title>{user.name}</Typography.Title>
      <Typography.Text>{user.uuid}</Typography.Text>
      {/* TODO Make role name `Administrator` a constant. */}
      {currentUser && currentUser?.roles?.includes("Administrator") && (
        <>
          <Divider />
          <Typography.Title level={2}>Actions</Typography.Title>
          <Button
            danger
            type="primary"
            onClick={deleteUser}
            loading={deletingUser}
          >
            Delete User
          </Button>
        </>
      )}
    </>
  );
};

export default User;
