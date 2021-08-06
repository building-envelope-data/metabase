import {
  Button,
  Divider,
  Typography,
  message,
  Skeleton,
  Descriptions,
  List,
} from "antd";
import {
  UsersDocument,
  useUserQuery,
  useDeleteUserMutation,
} from "../../queries/users.graphql";
import { InstitutionDocument } from "../../queries/institutions.graphql";
import { MethodDocument } from "../../queries/methods.graphql";
import { useConfirmInstitutionRepresentativeMutation } from "../../queries/institutionRepresentatives.graphql";
import { useConfirmUserMethodDeveloperMutation } from "../../queries/userMethodDevelopers.graphql";
import { Scalars } from "../../__generated__/__types__";
import { useCurrentUserQuery } from "../../queries/currentUser.graphql";
import { useEffect } from "react";
import { useRouter } from "next/router";
import paths from "../../paths";
import { useState } from "react";
import Link from "next/link";
import { UserDocument } from "../../__generated__/queries/users.graphql";

export type UserProps = {
  userId: Scalars["Uuid"];
};

export default function User({ userId }: UserProps) {
  const router = useRouter();
  const { loading, error, data } = useUserQuery({
    variables: {
      uuid: userId,
    },
  });
  const user = data?.user;
  const currentUser = useCurrentUserQuery()?.data?.currentUser;

  const [confirmInstitutionRepresentativeMutation] =
    useConfirmInstitutionRepresentativeMutation();
  const [
    confirmingInstitutionRepresentative,
    setConfirmingInstitutionRepresentative,
  ] = useState(false);

  const confirmInstitutionRepresentative = async (
    institutionId: Scalars["Uuid"]
  ) => {
    try {
      setConfirmingInstitutionRepresentative(true);
      const { errors, data } = await confirmInstitutionRepresentativeMutation({
        variables: {
          institutionId: institutionId,
          userId: userId,
        },
        refetchQueries: [
          {
            query: UserDocument,
            variables: {
              uuid: userId,
            },
          },
          {
            query: InstitutionDocument,
            variables: {
              uuid: institutionId,
            },
          },
        ],
      });
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.confirmInstitutionRepresentative?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.confirmInstitutionRepresentative?.errors
            .map((error) => error.message)
            .join(" ")
        );
      }
    } finally {
      setConfirmingInstitutionRepresentative(false);
    }
  };

  const [confirmUserMethodDeveloperMutation] =
    useConfirmUserMethodDeveloperMutation();
  const [confirmingUserMethodDeveloper, setConfirmingUserMethodDeveloper] =
    useState(false);

  const confirmUserMethodDeveloper = async (methodId: Scalars["Uuid"]) => {
    try {
      setConfirmingUserMethodDeveloper(true);
      const { errors, data } = await confirmUserMethodDeveloperMutation({
        variables: {
          methodId: methodId,
          userId: userId,
        },
        refetchQueries: [
          {
            query: UserDocument,
            variables: {
              uuid: userId,
            },
          },
          {
            query: MethodDocument,
            variables: {
              uuid: methodId,
            },
          },
        ],
      });
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.confirmUserMethodDeveloper?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.confirmUserMethodDeveloper?.errors
            .map((error) => error.message)
            .join(" ")
        );
      }
    } finally {
      setConfirmingUserMethodDeveloper(false);
    }
  };

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
      <Typography.Title level={1}>{user.name}</Typography.Title>
      <Descriptions column={1}>
        <Descriptions.Item label="UUID">{user.uuid}</Descriptions.Item>
        <Descriptions.Item label="Email Address">
          {user.email}
        </Descriptions.Item>
        <Descriptions.Item label="Phone Number">
          {user.phoneNumber}
        </Descriptions.Item>
        <Descriptions.Item label="Website">
          <Typography.Link href={user.websiteLocator}>
            {user.websiteLocator}
          </Typography.Link>
        </Descriptions.Item>
      </Descriptions>

      <Divider />
      <Typography.Title level={2}>Represented Institutions</Typography.Title>
      <List
        size="small"
        dataSource={user.representedInstitutions.edges}
        renderItem={(item) => (
          <List.Item>
            <Link href={paths.institution(item.node.uuid)}>
              {item.node.name}
            </Link>
          </List.Item>
        )}
      />
      {user.pendingRepresentedInstitutions.canCurrentUserConfirmEdge &&
        user.pendingRepresentedInstitutions.edges.length >= 1 && (
          <List
            size="small"
            header="Pending"
            dataSource={user.pendingRepresentedInstitutions.edges}
            renderItem={(item) => (
              <List.Item>
                <Link href={paths.institution(item.node.uuid)}>
                  {item.node.name}
                </Link>
                <Button
                  onClick={() =>
                    confirmInstitutionRepresentative(item.node.uuid)
                  }
                  loading={confirmingInstitutionRepresentative}
                >
                  Confirm
                </Button>
              </List.Item>
            )}
          />
        )}

      <Divider />
      <Typography.Title level={2}>Developed Methods</Typography.Title>
      <List
        size="small"
        dataSource={user.developedMethods.edges}
        renderItem={(item) => (
          <List.Item>
            <Link href={paths.method(item.node.uuid)}>{item.node.name}</Link>
          </List.Item>
        )}
      />
      {user.pendingDevelopedMethods.canCurrentUserConfirmEdge &&
        user.pendingDevelopedMethods.edges.length >= 1 && (
          <List
            size="small"
            header="Pending"
            dataSource={user.pendingDevelopedMethods.edges}
            renderItem={(item) => (
              <List.Item>
                <Link href={paths.method(item.node.uuid)}>
                  {item.node.name}
                </Link>
                <Button
                  onClick={() => confirmUserMethodDeveloper(item.node.uuid)}
                  loading={confirmingUserMethodDeveloper}
                >
                  Confirm
                </Button>
              </List.Item>
            )}
          />
        )}

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
}
