import { useQuery } from "@apollo/client/react";
import { Skeleton, Result, Card, Typography, Divider } from "antd";
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
  CurrentUserQuery,
} from "../../queries/currentUser.generated";
import PendingDatabaseList from "../databases/PendingDatabaseList";
import LazyTabs, { LazyTabsProps } from "../LazyTabs";
import { useMemo } from "react";
import { getPendingTabsOfInstitution } from "../institutions/Institution";

const getPendingTabsOfUser = (
  user: UserPartialFragment,
  currentUserData?: CurrentUserQuery,
): LazyTabsProps["items"] =>
  [
    ...user.representedInstitutions.edges.flatMap(({ node }) =>
      getPendingTabsOfInstitution(node),
    ),
    user.roles?.includes(UserRole.Verifier) && {
      key: "institutions",
      label: "Institutions",
      count: currentUserData?.pendingInstitutionCount?.totalCount,
      children: <PendingInstitutionList />,
    },
    user.roles?.includes(UserRole.Administrator) && {
      key: "databases",
      label: "Databases",
      count: currentUserData?.pendingDatabaseCount?.totalCount,
      children: <PendingDatabaseList />,
    },
  ].filter(isTruthy);

interface UserProps {
  userId: Scalars["Uuid"]["input"];
}

export default function User({ userId }: UserProps) {
  const currentUserData = useQuery(CurrentUserDocument)?.data;
  const currentUser = currentUserData?.currentUser;

  const queryVariables = {
    id: userId,
  };
  const { loading, error, data } = useQuery(UserDocument, {
    variables: queryVariables,
  });
  useQueryHandler({ error });
  const user = data?.user;

  const showInputControls =
    (currentUser &&
      user &&
      (currentUser.uuid == user.uuid ||
        currentUser.roles?.includes(UserRole.Administrator))) ??
    false;

  const pendingTabs = useMemo(() => {
    return showInputControls && user
      ? getPendingTabsOfUser(user, currentUserData)
      : null;
  }, [showInputControls, user, currentUserData]);

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
