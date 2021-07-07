import Layout from "../../components/Layout";
import {
  Select,
  Input,
  Table,
  InputNumber,
  message,
  Form,
  Button,
  Alert,
  Descriptions,
  Typography,
} from "antd";
import { MinusCircleOutlined, PlusOutlined } from "@ant-design/icons";
import {
  useAllOpticalDataQuery,
  OpticalData,
  Scalars,
  OpticalDataPropositionInput,
} from "../../queries/data.graphql";
import { useState } from "react";
import Link from "next/link";
import paths from "../../paths";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

enum ComponentIdComperator {
  EqualTo = "equalTo",
}

enum NearnormalHemisphericalVisibleTransmittanceComperator {
  EqualTo = "equalTo",
  LessThanOrEqualTo = "lessThanOrEqualTo",
  GreaterThanOrEqualTo = "greaterThanOrEqualTo",
  // InClosedInterval = "inClosedInterval"
}

function Index() {
  const [form] = Form.useForm();
  const [filtering, setFiltering] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [data, setData] = useState<OpticalData[]>([]);
  // Using `skip` is inspired by https://github.com/apollographql/apollo-client/issues/5268#issuecomment-749501801
  // An alternative would be `useLazy...` as told in https://github.com/apollographql/apollo-client/issues/5268#issuecomment-527727653
  // `useLazy...` does not return a `Promise` though as `use...Query.refetch` does which is used below.
  // For error policies see https://www.apollographql.com/docs/react/v2/data/error-handling/#error-policies
  const allOpticalDataQuery = useAllOpticalDataQuery({
    skip: true,
    errorPolicy: "all",
  });

  const onFinish = ({
    componentId,
    componentIdComperator,
    nearnormalHemisphericalVisibleTransmittances,
  }: {
    componentId: Scalars["Uuid"] | undefined;
    componentIdComperator: ComponentIdComperator;
    nearnormalHemisphericalVisibleTransmittances:
      | {
          nearnormalHemisphericalVisibleTransmittance: number | undefined;
          nearnormalHemisphericalVisibleTransmittanceComperator: NearnormalHemisphericalVisibleTransmittanceComperator;
        }[]
      | undefined;
  }) => {
    const filter = async () => {
      try {
        setFiltering(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        var propositions: OpticalDataPropositionInput[] = [];
        if (componentId) {
          propositions.push({
            componentId: { [componentIdComperator]: componentId },
          });
        }
        // Note that `0` evaluates to `false`, so below we cannot use
        // `if (nearnormalHemisphericalVisibleTransmittance)`.
        if (nearnormalHemisphericalVisibleTransmittances) {
          for (var {
            nearnormalHemisphericalVisibleTransmittance,
            nearnormalHemisphericalVisibleTransmittanceComperator,
          } of nearnormalHemisphericalVisibleTransmittances) {
            if (
              nearnormalHemisphericalVisibleTransmittance !== undefined &&
              nearnormalHemisphericalVisibleTransmittance !== null
            ) {
              propositions.push({
                nearnormalHemisphericalVisibleTransmittance: {
                  [nearnormalHemisphericalVisibleTransmittanceComperator]:
                    nearnormalHemisphericalVisibleTransmittance,
                },
              });
            }
          }
        }
        var where: OpticalDataPropositionInput;
        if (propositions.length == 0) {
          where = {};
        } else if (propositions.length == 1) {
          where = propositions[0];
        } else {
          where = { and: propositions };
        }
        const { error, data } = await allOpticalDataQuery.refetch({
          where: where,
        });
        if (error) {
          // TODO Handle properly.
          message.error(error);
        }
        // TODO Add `edge.node.databaseId to nodes?
        var nestedData =
          data?.databases?.edges?.map(
            (edge) => edge?.node?.allOpticalData?.nodes || []
          ) || [];
        var flatData = ([] as OpticalData[]).concat(...nestedData);
        setData(flatData);
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setFiltering(false);
      }
    };
    filter();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <Layout>
      {/* TODO Display error messages in a list? */}
      {globalErrorMessages.length > 0 && (
        <Alert type="error" message={globalErrorMessages.join(" ")} />
      )}
      <Form
        {...layout}
        form={form}
        name="filterData"
        initialValues={{
          componentIdComperator: "equalTo",
          nearnormalHemisphericalVisibleTransmittanceComperator: "equalTo",
        }}
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item label="Component Id">
          <Input.Group>
            <Form.Item noStyle name="componentIdComperator">
              <Select style={{ width: "20%" }}>
                <Select.Option value="equalTo">Equal to</Select.Option>
              </Select>
            </Form.Item>
            <Form.Item noStyle name="componentId">
              <Input style={{ float: "none", width: "80%" }} />
            </Form.Item>
          </Input.Group>
        </Form.Item>

        <Form.List name="nearnormalHemisphericalVisibleTransmittances">
          {(fields, { add, remove }, { errors }) => (
            <>
              {fields.map(({ key, name, fieldKey, ...restField }) => (
                <Form.Item label="Nearnormal hemispherical visible transmittance">
                  <Input.Group>
                    <Form.Item
                      {...restField}
                      name={[
                        name,
                        "nearnormalHemisphericalVisibleTransmittanceComperator",
                      ]}
                      fieldKey={[
                        fieldKey,
                        "nearnormalHemisphericalVisibleTransmittanceComperator",
                      ]}
                      noStyle
                    >
                      <Select style={{ width: "20%" }}>
                        <Select.Option value="equalTo">Equal to</Select.Option>
                        <Select.Option value="greaterThanOrEqualTo">
                          Greater than or equal to
                        </Select.Option>
                        <Select.Option value="lessThanOrEqualTo">
                          Less than or equal to
                        </Select.Option>
                        {/* TODO `inClosedInverval` */}
                      </Select>
                    </Form.Item>
                    <Form.Item
                      {...restField}
                      name={[
                        name,
                        "nearnormalHemisphericalVisibleTransmittance",
                      ]}
                      fieldKey={[
                        fieldKey,
                        "nearnormalHemisphericalVisibleTransmittance",
                      ]}
                      noStyle
                    >
                      <InputNumber
                        min={0}
                        max={1}
                        step="0.01"
                        style={{ width: "70%" }}
                      />
                    </Form.Item>
                    <MinusCircleOutlined
                      style={{ width: "10%" }}
                      onClick={() => remove(name)}
                    />
                  </Input.Group>
                </Form.Item>
              ))}
              <Form.Item {...tailLayout}>
                <Button
                  type="dashed"
                  onClick={() => add()}
                  style={{ width: "60%" }}
                  icon={<PlusOutlined />}
                >
                  Add nearnormal hemispherical visible transmittance proposition
                </Button>
                <Form.ErrorList errors={errors} />
              </Form.Item>
            </>
          )}
        </Form.List>

        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={filtering}>
            Filter
          </Button>
        </Form.Item>
      </Form>
      <Table
        loading={filtering}
        columns={[
          {
            title: "UUID",
            dataIndex: "uuid",
            key: "uuid",
            sorter: (a, b) => a.uuid.localeCompare(b.uuid, "en"),
            sortDirections: ["ascend", "descend"],
          },
          {
            title: "Timestamp",
            dataIndex: "timestamp",
            key: "timestamp",
            sorter: (a, b) => b.timestamp - a.timestamp,
            sortDirections: ["ascend", "descend"],
          },
          {
            title: "Component UUID",
            dataIndex: "componentId",
            key: "componentId",
          },
          {
            title: "Database UUID",
            dataIndex: "databaseId",
            key: "databaseId",
          },
          {
            title: "Applied Method",
            dataIndex: "appliedMethod",
            key: "appliedMethod",
            render: (_text, row, _index) => row.appliedMethod.methodId,
          },
          {
            title: "Resource Tree",
            dataIndex: "resourceTree",
            key: "resourceTree",
            render: (_text, row, _index) => (
              <Descriptions column={1}>
                <Descriptions.Item label="Description">
                  {row.resourceTree.root.value.description}
                </Descriptions.Item>
                <Descriptions.Item label="Hash Value">
                  {row.resourceTree.root.value.hashValue}
                </Descriptions.Item>
                <Descriptions.Item label="Locator">
                  <Typography.Link href={row.resourceTree.root.value.locator}>
                    {row.resourceTree.root.value.locator}
                  </Typography.Link>
                </Descriptions.Item>
                <Descriptions.Item label="Format UUID">
                  <Link
                    href={paths.dataFormat(
                      row.resourceTree.root.value.formatId
                    )}
                  >
                    {row.resourceTree.root.value.formatId}
                  </Link>
                </Descriptions.Item>
              </Descriptions>
            ),
          },
        ]}
        dataSource={data}
      />
    </Layout>
  );
}

export default Index;
