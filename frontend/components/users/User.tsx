import {
  Tag,
  Button,
  Divider,
  Typography,
  message,
  Skeleton,
  Descriptions,
  List,
  Result,
} from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import { SyncOutlined } from "@ant-design/icons";
import {
  UsersDocument,
  useUserQuery,
  useDeleteUserMutation,
  UserDocument,
  useRemoveUserRoleMutation,
} from "../../queries/users.graphql";
import { InstitutionDocument } from "../../queries/institutions.graphql";
import { MethodDocument } from "../../queries/methods.graphql";
import { useConfirmInstitutionRepresentativeMutation } from "../../queries/institutionRepresentatives.graphql";
import { useConfirmUserMethodDeveloperMutation } from "../../queries/userMethodDevelopers.graphql";
import { Scalars, UserRole } from "../../__generated__/__types__";
import { useRouter } from "next/router";
import paths from "../../paths";
import { useEffect, useState } from "react";
import Link from "next/link";
import AddUserRole from "./AddUserRole";
import { messageApolloError } from "../../lib/apollo";

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
  const rolesCurrentUserCanAndMayWantToAdd =
    user?.rolesCurrentUserCanAdd?.filter((role) => !user.roles?.includes(role));

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

  const [removeUserRoleMutation] = useRemoveUserRoleMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
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
  const [removingUserRole, setRemovingUserRole] = useState(false);

  const removeUserRole = async (role: UserRole) => {
    try {
      setRemovingUserRole(true);
      const { errors, data } = await removeUserRoleMutation({
        variables: {
          userId: userId,
          role: role,
        },
      });
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.removeUserRole?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.removeUserRole?.errors.map((error) => error.message).join(" ")
        );
      }
    } finally {
      setRemovingUserRole(false);
    }
  };

  useEffect(() => {
    if (error) {
      messageApolloError(error);
    }
  }, [error]);

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!user) {
    return (
      <Result
        status="500"
        title="500"
        subTitle="Sorry, something went wrong."
      />
    );
  }

  return (
    <PageHeader
      title={user.name}
      tags={user.roles?.map((x) => (
        <Tag
          key={x}
          icon={removingUserRole && <SyncOutlined spin />}
          closable={
            (!removingUserRole &&
              user.rolesCurrentUserCanRemove?.includes(x)) ||
            false
          }
          onClose={() => removeUserRole(x)}
          color="magenta"
        >
          {x}
        </Tag>
      ))}
      extra={[
        user.canCurrentUserDeleteUser && (
          <Button
            danger
            type="primary"
            onClick={deleteUser}
            loading={deletingUser}
          >
            Delete User
          </Button>
        ),
      ].filter((x) => x != null)}
      backIcon={false}
    >
      <Descriptions column={1}>
        <Descriptions.Item label="UUID">{user.uuid}</Descriptions.Item>
        {user.email && (
          <Descriptions.Item label="Email Address">
            <Typography.Link href={`mailto:${user.email}`}>
              {user.email}
            </Typography.Link>
          </Descriptions.Item>
        )}
        {user.phoneNumber && (
          <Descriptions.Item label="Phone Number">
            {user.phoneNumber}
          </Descriptions.Item>
        )}
        {user.websiteLocator && (
          <Descriptions.Item label="Website">
            <Typography.Link href={user.websiteLocator}>
              {user.websiteLocator}
            </Typography.Link>
          </Descriptions.Item>
        )}
      </Descriptions>
      {rolesCurrentUserCanAndMayWantToAdd &&
        rolesCurrentUserCanAndMayWantToAdd.length >= 1 && (
          <AddUserRole
            userId={user.uuid}
            roles={rolesCurrentUserCanAndMayWantToAdd}
          />
        )}

      <Divider />
      <Typography.Title level={2}>Represented Institutions</Typography.Title>
      <List
        size="small"
        dataSource={user.representedInstitutions.edges}
        renderItem={(item) => (
          <List.Item key={item.node.uuid}>
            <Link href={paths.institution(item.node.uuid)} legacyBehavior>
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
              <List.Item key={item.node.uuid}>
                <Link href={paths.institution(item.node.uuid)} legacyBehavior>
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
          <List.Item key={item.node.uuid}>
            <Link href={paths.method(item.node.uuid)} legacyBehavior>{item.node.name}</Link>
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
              <List.Item key={item.node.uuid}>
                <Link href={paths.method(item.node.uuid)} legacyBehavior>
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
    </PageHeader>
  );
}
