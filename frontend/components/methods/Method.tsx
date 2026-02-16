import { useMutation } from "@apollo/client/react";
import { useQuery } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  MethodsDocument,
  MethodDocument,
} from "../../queries/methods.generated";
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
  App,
} from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import { useEffect, useState } from "react";
import Link from "next/link";
import paths from "../../paths";
import { Reference } from "../Reference";
import OpenEndedDateTimeRangeX from "../OpenEndedDateTimeRangeX";
import { stringifyApolloError } from "../../lib/apollo";
import UpdateMethod from "./UpdateMethod";
import { RemoveInstitutionMethodDeveloperDocument } from "../../queries/institutionMethodDevelopers.generated";
import { RemoveUserMethodDeveloperDocument } from "../../queries/userMethodDevelopers.generated";
import { InstitutionDocument } from "../../queries/institutions.generated";
import AddInstitutionMethodDeveloper from "./AddInstitutionMethodDeveloper";
import AddUserMethodDeveloper from "./AddUserMethodDeveloper";
import { UserDocument } from "../../queries/users.generated";

export type MethodProps = {
  methodId: Scalars["Uuid"]["input"];
};

export default function Method({ methodId }: MethodProps) {
  const { loading, error, data } = useQuery(MethodDocument, {
    variables: {
      uuid: methodId,
    },
  });
  const method = data?.method;
  const { message } = App.useApp();

  useEffect(() => {
    if (error) {
      message.error(stringifyApolloError(error));
    }
  }, [error]);

  const [removeInstitutionMethodDeveloperMutation] = useMutation(
    RemoveInstitutionMethodDeveloperDocument,
  );
  const [
    removingInstitutionMethodDeveloper,
    setRemovingInstitutionMethodDeveloper,
  ] = useState(false);

  const removeInstitutionMethodDeveloper = async (
    institutionId: Scalars["Uuid"]["input"],
  ) => {
    try {
      setRemovingInstitutionMethodDeveloper(true);
      const { error, data } = await removeInstitutionMethodDeveloperMutation({
        variables: {
          input: {
            methodId: methodId,
            institutionId: institutionId,
          },
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
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.removeInstitutionMethodDeveloper?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.removeInstitutionMethodDeveloper?.errors
            .map((error) => error.message)
            .join(" "),
        );
      }
    } finally {
      setRemovingInstitutionMethodDeveloper(false);
    }
  };

  const [removeUserMethodDeveloperMutation] = useMutation(
    RemoveUserMethodDeveloperDocument,
  );
  const [removingUserMethodDeveloper, setRemovingUserMethodDeveloper] =
    useState(false);

  const removeUserMethodDeveloper = async (
    userId: Scalars["Uuid"]["input"],
  ) => {
    try {
      setRemovingUserMethodDeveloper(true);
      const { error, data } = await removeUserMethodDeveloperMutation({
        variables: {
          input: {
            methodId: methodId,
            userId: userId,
          },
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
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.removeUserMethodDeveloper?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.removeUserMethodDeveloper?.errors
            .map((error) => error.message)
            .join(" "),
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

  return (
    <>
      <PageHeader
        title={method.name}
        subTitle={method.description}
        tags={method.categories.map((x) => (
          <Tag key={x} color="magenta">
            {x}
          </Tag>
        ))}
        extra={
          method.isAuthorizedToUpdateNode
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
          {method.calculationLocator && (
            <Descriptions.Item label="Calculation">
              <Typography.Link href={method.calculationLocator}>
                {method.calculationLocator}
              </Typography.Link>
            </Descriptions.Item>
          )}
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
        method.developers.isAuthorizedToAddInstitutionEdge ||
        method.developers.isAuthorizedToAddUserEdge) && (
          <Row gutter={[16, 16]}>
            <Col flex={1}>
              {(method.developers.edges.length >= 1 ||
                method.developers.isAuthorizedToAddInstitutionEdge) && (
                  <List
                    header="Institution Developers"
                    bordered={true}
                    size="small"
                    footer={
                      method.developers.isAuthorizedToAddInstitutionEdge && (
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
                            x.isAuthorizedToRemoveEdge
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
                              <Link
                                href={paths.institution(x.node.uuid)}
                                legacyBehavior
                              >
                                {x.node.name}
                              </Link>
                            }
                          />
                        </List.Item>
                      ))}
                    {method.pendingDevelopers?.edges
                      .filter((x) => x.node.__typename == "Institution")
                      .map((x) => (
                        <List.Item
                          key={x.node.uuid}
                          actions={
                            x.isAuthorizedToRemoveEdge
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
                              <Link
                                href={paths.institution(x.node.uuid)}
                                legacyBehavior
                              >
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
                method.developers.isAuthorizedToAddUserEdge) && (
                  <List
                    header="User Developers"
                    bordered={true}
                    size="small"
                    footer={
                      method.developers.isAuthorizedToAddUserEdge && (
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
                            x.isAuthorizedToRemoveEdge
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
                    {method.pendingDevelopers?.edges
                      .filter((x) => x.node.__typename == "User")
                      .map((x) => (
                        <List.Item
                          key={x.node.uuid}
                          actions={
                            x.isAuthorizedToRemoveEdge
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
    </>
  );
}
