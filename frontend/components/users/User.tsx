import { useQuery } from "@apollo/client/react";
import { Skeleton, Result, Card, Typography, Divider, Flex } from "antd";
import {
  UserDocument,
  UserPartialFragment,
} from "../../queries/users.generated";
import { Scalars, UserRole } from "../../__generated__/graphql";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import UserSummary from "./UserSummary";
import { isTruthy } from "../../lib/array";
import QueryToolbar from "../QueryToolbar";
import PendingInstitutionList from "../institutions/PendingInstitutionList";
import {
  CurrentUserDocument,
  CurrentUserPartialFragment,
} from "../../queries/currentUser.generated";
import PendingDatabaseList from "../databases/PendingDatabaseList";
import LazyTabs, { LazyTabsProps } from "../LazyTabs";
import { useMemo } from "react";
import EntityLink from "../entities/EntityLink";
import paths from "../../paths";

const getPendingTabs = (
  currentUser: CurrentUserPartialFragment,
  user: UserPartialFragment,
): LazyTabsProps["items"] =>
  [
    currentUser.uuid == user.uuid &&
      currentUser.representedInstitutions.edges.some(
        (edge) =>
          (edge.node.pendingManufacturedComponents.totalCount > 0 &&
            edge.node.pendingDevelopedMethods.isAuthorizedToConfirmEdges) ||
          (edge.node.pendingDevelopedMethods.totalCount > 0 &&
            edge.node.pendingDevelopedMethods.isAuthorizedToConfirmEdges),
      ) && {
        key: "represented",
        label: "Represented Institutions",
        children: (
          <Flex vertical gap="middle">
            {currentUser.representedInstitutions.edges.map((edge) => (
              <div key={edge.node.id}>
                The institution{" "}
                <EntityLink entity={edge.node} route={paths.institution} /> has
                pending{" "}
                {[
                  edge.node.pendingManufacturedComponents.totalCount > 0 &&
                    edge.node.pendingDevelopedMethods
                      .isAuthorizedToConfirmEdges &&
                    "manufactured components",
                  edge.node.pendingDevelopedMethods.totalCount > 0 &&
                    edge.node.pendingDevelopedMethods
                      .isAuthorizedToConfirmEdges &&
                    "developed methods",
                ]
                  .filter(isTruthy)
                  .join("and")}{" "}
                that are awaiting confirmation or denial on{" "}
                <EntityLink
                  entity={edge.node}
                  route={(id) => `${paths.institution(id)}#pending-entities`}
                />
              </div>
            ))}
          </Flex>
        ),
      },
    currentUser.uuid == user.uuid &&
      user.roles?.includes(UserRole.Verifier) && {
        key: "institutions",
        label: "Institutions",
        children: <PendingInstitutionList />,
      },
    currentUser.uuid == user.uuid &&
      user.roles?.includes(UserRole.Administrator) && {
        key: "databases",
        label: "Databases",
        children: <PendingDatabaseList />,
      },
  ].filter(isTruthy);

interface UserProps {
  userId: Scalars["Uuid"]["input"];
}

export default function User({ userId }: UserProps) {
  const currentUser = useQuery(CurrentUserDocument)?.data?.currentUser;

  const queryVariables = {
    uuid: userId,
  };
  const { loading, error, data } = useQuery(UserDocument, {
    variables: queryVariables,
  });
  useQueryHandler({ error });
  const user = data?.user;

  const pendingTabs = useMemo(() => {
    if (!currentUser || !user) return null;
    return getPendingTabs(currentUser, user);
  }, [currentUser, user]);

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
    <div>
      <Card style={{ marginBottom: "1em" }}>
        <UserSummary entity={user} />
      </Card>
      <QueryToolbar query={UserDocument} variables={queryVariables} />
      {pendingTabs && pendingTabs.length > 0 && (
        <>
          <Divider />
          <Typography.Title level={4}>Pending Entities</Typography.Title>
          <LazyTabs items={pendingTabs} />
        </>
      )}
    </div>
  );
}
