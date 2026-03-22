import { useQuery } from "@apollo/client/react";
import {
  Divider,
  Typography,
  Skeleton,
  Descriptions,
  List,
  Result,
} from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import { UserDocument } from "../../queries/users.generated";
import { Scalars } from "../../__generated__/graphql";
import paths from "../../paths";
import Link from "next/link";
import AddUserRole from "./AddUserRole";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import { UserRoleTag } from "./UserRoleTag";
import ConfirmUserMethodDeveloper from "../methods/ConfirmUserMethodDeveloper";
import ConfirmInstitutionRepresentative from "../institutions/ConfirmInstitutionRepresentative";
import DeleteUser from "./DeleteUser";

export type UserProps = {
  userId: Scalars["Uuid"]["input"];
};

export default function User({ userId }: UserProps) {
  const { loading, error, data } = useQuery(UserDocument, {
    variables: {
      uuid: userId,
    },
  });
  useQueryHandler({ error });
  const user = data?.user;
  const rolesCurrentUserCanAndMayWantToAdd =
    user?.rolesCurrentUserCanAdd?.filter((role) => !user.roles?.includes(role));

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
        tags={user.roles?.map((role) => (
          <UserRoleTag
            key={`${role}-tag`}
            userId={user.uuid}
            role={role}
            canRemove={user.rolesCurrentUserCanRemove?.includes(role)}
          />
        ))}
        extra={[
          user.isAuthorizedToDeleteUser && <DeleteUser userId={user.uuid} />,
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
              <Link href={paths.institution(item.node.uuid)}>
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
                  <Link href={paths.institution(item.node.uuid)}>
                    {item.node.name}
                  </Link>
                  <ConfirmInstitutionRepresentative
                    userId={user.uuid}
                    institutionId={item.node.uuid}
                  />
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
              <Link href={paths.method(item.node.uuid)}>{item.node.name}</Link>
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
                  <Link href={paths.method(item.node.uuid)}>
                    {item.node.name}
                  </Link>
                  <ConfirmUserMethodDeveloper
                    userId={user.uuid}
                    methodId={item.node.uuid}
                  />
                </List.Item>
              )}
            />
          )}
      </PageHeader>
    </>
  );
}
