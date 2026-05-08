import { useQuery } from "@apollo/client/react";
import { Skeleton, Result, Card, Flex, Typography, Space, Divider } from "antd";
import { UserDocument } from "../../queries/users.generated";
import { Scalars } from "../../__generated__/graphql";
import paths from "../../paths";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import ConfirmUserMethodDeveloper from "../methods/ConfirmUserMethodDeveloper";
import ConfirmInstitutionRepresentative from "../institutions/ConfirmInstitutionRepresentative";
import UserSummary from "./UserSummary";
import { asReadonlyMixed, isTruthy } from "../../lib/array";
import EntityLink from "../entities/EntityLink";
import InlineList from "../InlineList";
import EnumTag from "../EnumTag";
import QueryToolbar from "../QueryToolbar";
import RemoveInstitutionRepresentative from "../institutions/RemoveInstitutionRepresentative";
import RemoveUserMethodDeveloper from "../methods/RemoveUserMethodDeveloper";

interface UserProps {
  userId: Scalars["Uuid"]["input"];
}

export default function User({ userId }: UserProps) {
  const queryVariables = {
    uuid: userId,
  };
  const { loading, error, data } = useQuery(UserDocument, {
    variables: queryVariables,
  });
  useQueryHandler({ error });
  const user = data?.user;

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

  const pending = [
    user.pendingRepresentedInstitutions &&
      user.pendingRepresentedInstitutions.isAuthorizedToConfirmEdges &&
      user.pendingRepresentedInstitutions.edges.length > 0 && (
        <div>
          The following institutions asked to add you as representative. Confirm
          or deny their request:{" "}
          <InlineList
            items={asReadonlyMixed(user.pendingRepresentedInstitutions.edges)}
            renderItem={(edge) => (
              <Space key={edge.node.id}>
                <EntityLink entity={edge.node} route={paths.institution} />
                <EnumTag color="grey" variant="outlined">
                  {edge.role}
                </EnumTag>
                <ConfirmInstitutionRepresentative
                  userId={user.uuid}
                  institutionId={edge.node.uuid}
                />
                {edge.isAuthorizedToRemoveEdge && (
                  <RemoveInstitutionRepresentative
                    userId={user.uuid}
                    institutionId={edge.node.uuid}
                  >
                    Deny
                  </RemoveInstitutionRepresentative>
                )}
              </Space>
            )}
          />
        </div>
      ),
    user.pendingUserDevelopedMethods != null &&
      user.pendingUserDevelopedMethods.isAuthorizedToConfirmEdges &&
      user.pendingUserDevelopedMethods.edges.length > 0 && (
        <div>
          The developers of the following methods asked to add you as a
          developer. Confirm or deny their request:{" "}
          <InlineList
            items={user.pendingUserDevelopedMethods.edges}
            renderItem={(edge) => (
              <Space key={edge.node.uuid}>
                <EntityLink entity={edge.node} route={paths.method} />
                <ConfirmUserMethodDeveloper
                  userId={user.uuid}
                  methodId={edge.node.uuid}
                />
                {edge.isAuthorizedToRemoveEdge && (
                  <RemoveUserMethodDeveloper
                    userId={user.uuid}
                    methodId={edge.node.uuid}
                  >
                    Deny
                  </RemoveUserMethodDeveloper>
                )}
              </Space>
            )}
          />
        </div>
      ),
  ].filter(isTruthy);

  return (
    <div>
      <Card style={{ marginBottom: "1em" }}>
        <UserSummary entity={user} />
        {pending.length > 0 && (
          <>
            <Divider />
            <Typography.Title level={4}>Pending</Typography.Title>
            <Flex vertical gap="medium">
              {pending}
            </Flex>
          </>
        )}
      </Card>
      <QueryToolbar query={UserDocument} variables={queryVariables} />
    </div>
  );
}
