import { useQuery } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import { MethodDocument } from "../../queries/methods.generated";
import {
  Tag,
  Skeleton,
  Result,
  Descriptions,
  Typography,
  List,
  Row,
  Col,
} from "antd";
import { PageHeader } from "@ant-design/pro-layout";
import Link from "next/link";
import paths from "../../paths";
import { Reference } from "../Reference";
import OpenEndedDateTimeRangeX from "../OpenEndedDateTimeRangeX";
import UpdateMethod from "./UpdateMethod";
import AddInstitutionMethodDeveloper from "./AddInstitutionMethodDeveloper";
import AddUserMethodDeveloper from "./AddUserMethodDeveloper";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import RemoveInstitutionMethodDeveloper from "./RemoveInstitutionMethodDeveloper";
import RemoveUserMethodDeveloper from "./RemoveUserMethodDeveloper";

export type MethodProps = {
  methodId: Scalars["Uuid"]["input"];
};

export default function Method({ methodId }: MethodProps) {
  const { loading, error, data } = useQuery(MethodDocument, {
    variables: {
      uuid: methodId,
    },
  });
  useQueryHandler({ error });
  const method = data?.method;

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
                  method={method}
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
                              <RemoveInstitutionMethodDeveloper
                                key={`removeInstitutionMethodDeveloper-${x.node.uuid}`}
                                methodId={method.uuid}
                                institutionId={x.node.uuid}
                              />,
                            ]
                          : []
                      }
                    >
                      <List.Item.Meta
                        title={
                          <Link href={paths.institution(x.node.uuid)}>
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
                              <RemoveInstitutionMethodDeveloper
                                key={`removeInstitutionMethodDeveloper-${x.node.uuid}`}
                                methodId={method.uuid}
                                institutionId={x.node.uuid}
                              />,
                            ]
                          : []
                      }
                    >
                      <List.Item.Meta
                        title={
                          <Link href={paths.institution(x.node.uuid)}>
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
                              <RemoveUserMethodDeveloper
                                key={`removeUserMethodDeveloper-${x.node.uuid}`}
                                methodId={method.uuid}
                                userId={x.node.uuid}
                              />,
                            ]
                          : []
                      }
                    >
                      <List.Item.Meta
                        title={
                          <Link href={paths.user(x.node.uuid)}>
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
                              <RemoveUserMethodDeveloper
                                key={`removeUserMethodDeveloper-${x.node.uuid}`}
                                methodId={method.uuid}
                                userId={x.node.uuid}
                              />,
                            ]
                          : []
                      }
                    >
                      <List.Item.Meta
                        title={
                          <Link href={paths.user(x.node.uuid)}>
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
