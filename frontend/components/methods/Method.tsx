import { Scalars } from "../../__generated__/__types__";
import {
  MethodDocument,
  MethodsDocument,
  useMethodQuery,
} from "../../queries/methods.graphql";
import {
  Tag,
  Skeleton,
  Result,
  Descriptions,
  Typography,
  List,
  Row,
  Col,
  Button,
  message,
} from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import { useEffect, useState } from "react";
import Link from "next/link";
import paths from "../../paths";
import { Reference } from "../Reference";
import OpenEndedDateTimeRangeX from "../OpenEndedDateTimeRangeX";
import { messageApolloError } from "../../lib/apollo";
import UpdateMethod from "./UpdateMethod";
import { useRemoveInstitutionMethodDeveloperMutation } from "../../queries/institutionMethodDevelopers.graphql";
import { useRemoveUserMethodDeveloperMutation } from "../../queries/userMethodDevelopers.graphql";
import { InstitutionDocument } from "../../queries/institutions.graphql";
import AddInstitutionMethodDeveloper from "./AddInstitutionMethodDeveloper";
import AddUserMethodDeveloper from "./AddUserMethodDeveloper";
import { UserDocument } from "../../queries/users.graphql";

export type MethodProps = {
  methodId: Scalars["Uuid"];
};

export default function Method({ methodId }: MethodProps) {
  const { loading, error, data } = useMethodQuery({
    variables: {
      uuid: methodId,
    },
  });
  const method = data?.method;

  useEffect(() => {
    if (error) {
      messageApolloError(error);
    }
  }, [error]);

  const [removeInstitutionMethodDeveloperMutation] =
    useRemoveInstitutionMethodDeveloperMutation();
  const [
    removingInstitutionMethodDeveloper,
    setRemovingInstitutionMethodDeveloper,
  ] = useState(false);

  const removeInstitutionMethodDeveloper = async (
    institutionId: Scalars["Uuid"]
  ) => {
    try {
      setRemovingInstitutionMethodDeveloper(true);
      const { errors, data } = await removeInstitutionMethodDeveloperMutation({
        variables: {
          methodId: methodId,
          institutionId: institutionId,
        },
        refetchQueries: [
          {
            query: MethodsDocument,
          },
          {
            query: MethodDocument,
            variables: {
              uuid: methodId,
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
      } else if (data?.removeInstitutionMethodDeveloper?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.removeInstitutionMethodDeveloper?.errors
            .map((error) => error.message)
            .join(" ")
        );
      }
    } finally {
      setRemovingInstitutionMethodDeveloper(false);
    }
  };

  const [removeUserMethodDeveloperMutation] =
    useRemoveUserMethodDeveloperMutation();
  const [removingUserMethodDeveloper, setRemovingUserMethodDeveloper] =
    useState(false);

  const removeUserMethodDeveloper = async (userId: Scalars["Uuid"]) => {
    try {
      setRemovingUserMethodDeveloper(true);
      const { errors, data } = await removeUserMethodDeveloperMutation({
        variables: {
          methodId: methodId,
          userId: userId,
        },
        refetchQueries: [
          {
            query: MethodsDocument,
          },
          {
            query: MethodDocument,
            variables: {
              uuid: methodId,
            },
          },
          {
            query: UserDocument,
            variables: {
              uuid: userId,
            },
          },
        ],
      });
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.removeUserMethodDeveloper?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.removeUserMethodDeveloper?.errors
            .map((error) => error.message)
            .join(" ")
        );
      }
    } finally {
      setRemovingUserMethodDeveloper(false);
    }
  };

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!method) {
    return (
      <Result
        status="500"
        title="500"
        subTitle="Sorry, something went wrong."
      />
    );
  }

  return <>
    <PageHeader
      title={method.name}
      subTitle={method.description}
      tags={method.categories.map((x) => (
        <Tag key={x} color="magenta">
          {x}
        </Tag>
      ))}
      extra={
        method.canCurrentUserUpdateNode
          ? [
              <UpdateMethod
                key="updateMethod"
                methodId={method.uuid}
                name={method.name}
                description={method.description}
                validity={method.validity}
                availability={method.availability}
                reference={method.reference}
                calculationLocator={method.calculationLocator}
                categories={method.categories}
                managerId={method.manager.node.uuid}
              />,
            ]
          : []
      }
      backIcon={false}
    >
      <Descriptions size="small" column={1}>
        <Descriptions.Item label="UUID">{method.uuid}</Descriptions.Item>
        <Descriptions.Item label="Valid">
          <OpenEndedDateTimeRangeX range={method.validity} />
        </Descriptions.Item>
        <Descriptions.Item label="Available">
          <OpenEndedDateTimeRangeX range={method.availability} />
        </Descriptions.Item>
        <Descriptions.Item label="Calculation">
          <Typography.Link href={method.calculationLocator}>
            {method.calculationLocator}
          </Typography.Link>
        </Descriptions.Item>
        <Descriptions.Item label="Reference">
          <Reference reference={method.reference} />
        </Descriptions.Item>
        <Descriptions.Item label="Managed by">
          <Typography.Link href={paths.institution(method.manager.node.uuid)}>
            {method.manager.node.name}
          </Typography.Link>
        </Descriptions.Item>
      </Descriptions>
    </PageHeader>
    {(method.developers.edges.length >= 1 ||
      method.developers.canCurrentUserAddInstitutionEdge ||
      method.developers.canCurrentUserAddUserEdge) && (
      <Row gutter={[16, 16]}>
        <Col flex={1}>
          {(method.developers.edges.length >= 1 ||
            method.developers.canCurrentUserAddInstitutionEdge) && (
            <List
              header="Institution Developers"
              bordered={true}
              size="small"
              footer={
                method.developers.canCurrentUserAddInstitutionEdge && (
                  <AddInstitutionMethodDeveloper methodId={method.uuid} />
                )
              }
            >
              {method.developers.edges
                .filter((x) => x.node.__typename == "Institution")
                .map((x) => (
                  <List.Item
                    key={x.node.uuid}
                    actions={
                      x.canCurrentUserRemoveEdge
                        ? [
                            <Button
                              key="remove"
                              onClick={() =>
                                removeInstitutionMethodDeveloper(x.node.uuid)
                              }
                              loading={removingInstitutionMethodDeveloper}
                            >
                              Remove
                            </Button>,
                          ]
                        : []
                    }
                  >
                    <List.Item.Meta
                      title={
                        <Link href={paths.institution(x.node.uuid)} legacyBehavior>
                          {x.node.name}
                        </Link>
                      }
                    />
                  </List.Item>
                ))}
              {method.pendingDevelopers.edges
                .filter((x) => x.node.__typename == "Institution")
                .map((x) => (
                  <List.Item
                    key={x.node.uuid}
                    actions={
                      x.canCurrentUserRemoveEdge
                        ? [
                            <Button
                              key="remove"
                              onClick={() =>
                                removeInstitutionMethodDeveloper(x.node.uuid)
                              }
                              loading={removingInstitutionMethodDeveloper}
                            >
                              Remove
                            </Button>,
                          ]
                        : []
                    }
                  >
                    <List.Item.Meta
                      title={
                        <Link href={paths.institution(x.node.uuid)} legacyBehavior>
                          {x.node.name} (Pending)
                        </Link>
                      }
                    />
                  </List.Item>
                ))}
            </List>
          )}
        </Col>
        <Col flex={1}>
          {(method.developers.edges.length >= 1 ||
            method.developers.canCurrentUserAddUserEdge) && (
            <List
              header="User Developers"
              bordered={true}
              size="small"
              footer={
                method.developers.canCurrentUserAddUserEdge && (
                  <AddUserMethodDeveloper methodId={method.uuid} />
                )
              }
            >
              {method.developers.edges
                .filter((x) => x.node.__typename == "User")
                .map((x) => (
                  <List.Item
                    key={x.node.uuid}
                    actions={
                      x.canCurrentUserRemoveEdge
                        ? [
                            <Button
                              key="remove"
                              onClick={() =>
                                removeUserMethodDeveloper(x.node.uuid)
                              }
                              loading={removingUserMethodDeveloper}
                            >
                              Remove
                            </Button>,
                          ]
                        : []
                    }
                  >
                    <List.Item.Meta
                      title={
                        <Link href={paths.user(x.node.uuid)} legacyBehavior>
                          {x.node.name}
                        </Link>
                      }
                    />
                  </List.Item>
                ))}
              {method.pendingDevelopers.edges
                .filter((x) => x.node.__typename == "User")
                .map((x) => (
                  <List.Item
                    key={x.node.uuid}
                    actions={
                      x.canCurrentUserRemoveEdge
                        ? [
                            <Button
                              key="remove"
                              onClick={() =>
                                removeUserMethodDeveloper(x.node.uuid)
                              }
                              loading={removingUserMethodDeveloper}
                            >
                              Remove
                            </Button>,
                          ]
                        : []
                    }
                  >
                    <List.Item.Meta
                      title={
                        <Link href={paths.user(x.node.uuid)} legacyBehavior>
                          {x.node.name} (Pending)
                        </Link>
                      }
                    />
                  </List.Item>
                ))}
            </List>
          )}
        </Col>
      </Row>
    )}
  </>;
}
