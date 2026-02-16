import { useQuery, useMutation } from "@apollo/client/react";
import {
  Tag,
  Button,
  Divider,
  Typography,
  Skeleton,
  Descriptions,
  List,
  Result,
  App,
} from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import { SyncOutlined } from "@ant-design/icons";
import {
  UsersDocument,
  UserDocument,
  DeleteUserDocument,
  RemoveUserRoleDocument,
} from "../../queries/users.generated";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { MethodDocument } from "../../queries/methods.generated";
import { ConfirmInstitutionRepresentativeDocument } from "../../queries/institutionRepresentatives.generated";
import { ConfirmUserMethodDeveloperDocument } from "../../queries/userMethodDevelopers.generated";
import { Scalars, UserRole } from "../../__generated__/graphql";
import { useRouter } from "next/router";
import paths from "../../paths";
import { useEffect, useState } from "react";
import Link from "next/link";
import AddUserRole from "./AddUserRole";
import { stringifyApolloError } from "../../lib/apollo";

export type UserProps = {
  userId: Scalars["Uuid"]["input"];
};

export default function User({ userId }: UserProps) {
  const router = useRouter();
  const { loading, error, data } = useQuery(UserDocument, {
    variables: {
      uuid: userId,
    },
  });
  const user = data?.user;
  const rolesCurrentUserCanAndMayWantToAdd =
    user?.rolesCurrentUserCanAdd?.filter((role) => !user.roles?.includes(role));

  const [confirmInstitutionRepresentativeMutation] = useMutation(
    ConfirmInstitutionRepresentativeDocument,
  );
  const [
    confirmingInstitutionRepresentative,
    setConfirmingInstitutionRepresentative,
  ] = useState(false);

  const confirmInstitutionRepresentative = async (
    institutionId: Scalars["Uuid"]["input"],
  ) => {
    try {
      setConfirmingInstitutionRepresentative(true);
      const { error, data } = await confirmInstitutionRepresentativeMutation({
        variables: {
          input: {
            institutionId: institutionId,
            userId: userId,
          },
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
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.confirmInstitutionRepresentative?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.confirmInstitutionRepresentative?.errors
            .map((error) => error.message)
            .join(" "),
        );
      }
    } finally {
      setConfirmingInstitutionRepresentative(false);
    }
  };

  const [confirmUserMethodDeveloperMutation] = useMutation(
    ConfirmUserMethodDeveloperDocument,
  );
  const [confirmingUserMethodDeveloper, setConfirmingUserMethodDeveloper] =
    useState(false);

  const confirmUserMethodDeveloper = async (
    methodId: Scalars["Uuid"]["input"],
  ) => {
    try {
      setConfirmingUserMethodDeveloper(true);
      const { error, data } = await confirmUserMethodDeveloperMutation({
        variables: {
          input: {
            methodId: methodId,
            userId: userId,
          },
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
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.confirmUserMethodDeveloper?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.confirmUserMethodDeveloper?.errors
            .map((error) => error.message)
            .join(" "),
        );
      }
    } finally {
      setConfirmingUserMethodDeveloper(false);
    }
  };

  const [deleteUserMutation] = useMutation(DeleteUserDocument, {
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
      const { error, data } = await deleteUserMutation({
        variables: {
          input: {
            userId: userId,
          },
        },
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.deleteUser?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.deleteUser?.errors.map((error) => error.message).join(" "),
        );
      } else {
        await router.push(paths.users);
      }
    } finally {
      setDeletingUser(false);
    }
  };

  const [removeUserRoleMutation] = useMutation(RemoveUserRoleDocument, {
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
      const { error, data } = await removeUserRoleMutation({
        variables: {
          input: {
            userId: userId,
            role: role,
          },
        },
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.removeUserRole?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.removeUserRole?.errors.map((error) => error.message).join(" "),
        );
      }
    } finally {
      setRemovingUserRole(false);
    }
  };

  const { message } = App.useApp();

  useEffect(() => {
    if (error) {
      message.error(stringifyApolloError(error));
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
    <>
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
          user.isAuthorizedToDeleteUser && (
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
        {user.pendingRepresentedInstitutions != null &&
          user.pendingRepresentedInstitutions.isAuthorizedToConfirmEdges &&
          user.pendingRepresentedInstitutions.edges.length >= 1 && (
            <List
              size="small"
              header="Pending"
              dataSource={user.pendingRepresentedInstitutions?.edges}
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
              <Link href={paths.method(item.node.uuid)} legacyBehavior>
                {item.node.name}
              </Link>
            </List.Item>
          )}
        />
        {user.pendingDevelopedMethods != null &&
          user.pendingDevelopedMethods.isAuthorizedToConfirmEdges &&
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
    </>
  );
}
