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
  Typography,
} from "antd";
import { MinusCircleOutlined, PlusOutlined } from "@ant-design/icons";
import { useAllOpticalDataQuery } from "../../queries/data.graphql";
import {
  OpticalData,
  Scalars,
  OpticalDataPropositionInput,
} from "../../__generated__/__types__";
import { useState } from "react";
import Link from "next/link";
import paths from "../../paths";
import { setMapValue } from "../../lib/freeTextFilter";
import {
  getFilterableDescriptionListColumnProps,
  getInternallyLinkedFilterableStringColumnProps,
  getUuidColumnProps,
} from "../../lib/table";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

enum Negator {
  Is = "is",
  IsNot = "isNot",
}

enum ComponentIdComperator {
  EqualTo = "equalTo",
}

enum NearnormalHemisphericalVisibleTransmittanceComperator {
  EqualTo = "equalTo",
  LessThanOrEqualTo = "lessThanOrEqualTo",
  GreaterThanOrEqualTo = "greaterThanOrEqualTo",
  // InClosedInterval = "inClosedInterval"
}

const negateIfNecessary = (
  negator: Negator,
  proposition: OpticalDataPropositionInput
): OpticalDataPropositionInput => {
  switch (negator) {
    case Negator.Is:
      return proposition;
    case Negator.IsNot:
      return { not: proposition };
  }
};

const conjunct = (
  propositions: OpticalDataPropositionInput[]
): OpticalDataPropositionInput => {
  if (propositions.length == 0) {
    return {};
  }
  if (propositions.length == 1) {
    return propositions[0];
  }
  return { and: propositions };
};

// const disjunct = (
//   propositions: OpticalDataPropositionInput[]
// ): OpticalDataPropositionInput => {
//   if (propositions.length == 0) {
//     return {};
//   }
//   if (propositions.length == 1) {
//     return propositions[0];
//   }
//   return { or: propositions };
// };

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

  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

  const onFinish = ({
    componentIds,
    nearnormalHemisphericalVisibleTransmittances,
  }: {
    componentIds:
      | {
          negator: Negator;
          comperator: ComponentIdComperator;
          value: Scalars["Uuid"] | undefined;
        }[]
      | undefined;
    nearnormalHemisphericalVisibleTransmittances:
      | {
          negator: Negator;
          comperator: NearnormalHemisphericalVisibleTransmittanceComperator;
          value: number | undefined;
        }[]
      | undefined;
  }) => {
    const filter = async () => {
      try {
        setFiltering(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const propositions: OpticalDataPropositionInput[] = [];
        if (componentIds) {
          for (let { negator, comperator, value } of componentIds) {
            propositions.push(
              negateIfNecessary(negator, {
                componentId: { [comperator]: value },
              })
            );
          }
        }
        // Note that `0` evaluates to `false`, so below we cannot use
        // `if (nearnormalHemisphericalVisibleTransmittance)`.
        if (nearnormalHemisphericalVisibleTransmittances) {
          for (let {
            negator,
            comperator,
            value,
          } of nearnormalHemisphericalVisibleTransmittances) {
            if (value !== undefined && value !== null) {
              propositions.push(
                negateIfNecessary(negator, {
                  nearnormalHemisphericalVisibleTransmittance: {
                    [comperator]: value,
                  },
                })
              );
            }
          }
        }
        const { error, data } = await allOpticalDataQuery.refetch({
          where: conjunct(propositions),
        });
        if (error) {
          // TODO Handle properly.
          console.log(error);
          message.error(
            error.graphQLErrors.map((error) => error.message).join(" ")
          );
        }
        // TODO Add `edge.node.databaseId to nodes?
        const nestedData =
          data?.databases?.edges?.map(
            (edge) => edge?.node?.allOpticalData?.nodes || []
          ) || [];
        const flatData = ([] as OpticalData[]).concat(...nestedData);
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
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        The network of <Link href={paths.databases}>databases</Link> can be
        queried here for data on building envelope{" "}
        <Link href={paths.components}>components</Link>.
      </Typography.Paragraph>
      <Typography.Title>Optical Data</Typography.Title>
      {/* TODO Display error messages in a list? */}
      {globalErrorMessages.length > 0 && (
        <Alert type="error" message={globalErrorMessages.join(" ")} />
      )}
      <Form
        {...layout}
        form={form}
        name="filterData"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.List name="componentIds">
          {(fields, { add, remove }, { errors }) => (
            <>
              {fields.map(({ key, name, fieldKey, ...restField }, index) => (
                <Form.Item key={key} label={index === 0 ? "Component Id" : " "}>
                  <Input.Group>
                    <Form.Item
                      {...restField}
                      key={`negator${key}`}
                      name={[name, "negator"]}
                      fieldKey={[fieldKey, "negator"]}
                      noStyle
                      initialValue={Negator.Is}
                    >
                      <Select style={{ width: "10%" }}>
                        <Select.Option value={Negator.Is}>Is</Select.Option>
                        <Select.Option value={Negator.IsNot}>
                          Is not
                        </Select.Option>
                      </Select>
                    </Form.Item>
                    <Form.Item
                      {...restField}
                      key={`comperator${key}`}
                      name={[name, "comperator"]}
                      fieldKey={[fieldKey, "comperator"]}
                      noStyle
                      initialValue={ComponentIdComperator.EqualTo}
                    >
                      <Select style={{ width: "20%" }}>
                        <Select.Option value={ComponentIdComperator.EqualTo}>
                          equal to
                        </Select.Option>
                      </Select>
                    </Form.Item>
                    <Form.Item
                      {...restField}
                      key={`value${key}`}
                      name={[name, "value"]}
                      fieldKey={[fieldKey, "value"]}
                      noStyle
                    >
                      <Input style={{ float: "none", width: "60%" }} />
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
                  style={{ width: "100%" }}
                  icon={<PlusOutlined />}
                >
                  Add component UUID proposition
                </Button>
                <Form.ErrorList errors={errors} />
              </Form.Item>
            </>
          )}
        </Form.List>

        <Form.List name="nearnormalHemisphericalVisibleTransmittances">
          {(fields, { add, remove }, { errors }) => (
            <>
              {fields.map(({ key, name, fieldKey, ...restField }, index) => (
                <Form.Item
                  key={key}
                  label={
                    index === 0
                      ? "Nearnormal hemispherical visible transmittance"
                      : " "
                  }
                >
                  <Input.Group>
                    <Form.Item
                      {...restField}
                      key={`negator${key}`}
                      name={[name, "negator"]}
                      fieldKey={[fieldKey, "negator"]}
                      noStyle
                      initialValue={Negator.Is}
                    >
                      <Select style={{ width: "10%" }}>
                        <Select.Option value={Negator.Is}>Is</Select.Option>
                        <Select.Option value={Negator.IsNot}>
                          Is not
                        </Select.Option>
                      </Select>
                    </Form.Item>
                    <Form.Item
                      {...restField}
                      key={`comperator${key}`}
                      name={[name, "comperator"]}
                      fieldKey={[fieldKey, "comperator"]}
                      noStyle
                      initialValue={
                        NearnormalHemisphericalVisibleTransmittanceComperator.EqualTo
                      }
                    >
                      <Select style={{ width: "20%" }}>
                        <Select.Option
                          value={
                            NearnormalHemisphericalVisibleTransmittanceComperator.EqualTo
                          }
                        >
                          equal to
                        </Select.Option>
                        <Select.Option
                          value={
                            NearnormalHemisphericalVisibleTransmittanceComperator.GreaterThanOrEqualTo
                          }
                        >
                          greater than or equal to
                        </Select.Option>
                        <Select.Option
                          value={
                            NearnormalHemisphericalVisibleTransmittanceComperator.LessThanOrEqualTo
                          }
                        >
                          less than or equal to
                        </Select.Option>
                        {/* TODO `inClosedInverval` */}
                      </Select>
                    </Form.Item>
                    <Form.Item
                      {...restField}
                      key={`value${key}`}
                      name={[name, "value"]}
                      fieldKey={[fieldKey, "value"]}
                      noStyle
                    >
                      <InputNumber
                        min={0}
                        max={1}
                        step="0.01"
                        style={{ width: "60%" }}
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
                  style={{ width: "100%" }}
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
            ...getUuidColumnProps<typeof data[0]>(
              onFilterTextChange,
              (x) => filterText.get(x),
              (_uuid) => "/" // TODO Link somewhere useful!
            ),
          },
          {
            title: "Timestamp",
            key: "timestamp",
            sorter: (a, b) => Date.parse(b.timestamp) - Date.parse(a.timestamp),
            sortDirections: ["ascend", "descend"],
          },
          {
            ...getInternallyLinkedFilterableStringColumnProps<typeof data[0]>(
              "Component UUID",
              "componentId",
              (x) => x.componentId,
              onFilterTextChange,
              (x) => filterText.get(x),
              (x) => paths.component(x.componentId)
            ),
          },
          // {
          //   ...getInternallyLinkedFilterableStringColumnProps<typeof data[0]>(
          //     "Database UUID",
          //     "databaseId",
          //     (x) => x.databaseId,
          //     onFilterTextChange,
          //     (x) => filterText.get(x),
          //     (x) => paths.database(x.databaseId)
          //   ),
          // },
          {
            ...getFilterableDescriptionListColumnProps<typeof data[0]>(
              "Applied Method",
              "appliedMethod",
              (x) => [
                {
                  key: "methodId",
                  title: "Method UUID",
                  value: x.appliedMethod.methodId,
                  render: (_record, _highlightedValue, value) => (
                    // TODO Why does this not work with `_highlightedValue`? An error is raised saying "Function components cannot be given refs. Attempts to access this ref will fail. Did you mean to use React.forwardRef()?": https://nextjs.org/docs/api-reference/next/link#if-the-child-is-a-function-component or https://reactjs.org/docs/forwarding-refs.html or https://deepscan.io/docs/rules/react-func-component-invalid-ref-prop or https://www.carlrippon.com/react-forwardref-typescript/
                    // TODO Actually, `value` is neither `null` nor `undefined` but the type system does not know about it. How can we make it know about it so we don't need `|| ""` here?
                    <Link href={paths.method(value || "")}>{value}</Link>
                  ),
                },
              ],
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getFilterableDescriptionListColumnProps<typeof data[0]>(
              "Resource Tree Root",
              "resourceTree",
              (x) => [
                {
                  key: "description",
                  title: "Description",
                  value: x.resourceTree.root.value.description,
                },
                {
                  key: "hashValue",
                  title: "Hash Value",
                  value: x.resourceTree.root.value.hashValue,
                },
                {
                  key: "locator",
                  title: "Locator",
                  value: x.resourceTree.root.value.locator,
                  render: (_record, hightlightedValue, value) => (
                    // TODO Actually, `value` is neither `null` nor `undefined` but the type system does not know about it. How can we make it know about it so we don't need `|| ""` here?
                    <Typography.Link href={value || ""}>
                      {hightlightedValue}
                    </Typography.Link>
                  ),
                },
                {
                  key: "formatId",
                  title: "Format UUID",
                  value: x.resourceTree.root.value.formatId,
                  render: (_record, _highlightedValue, value) => (
                    // TODO Why does this not work with `_highlightedValue`? An error is raised saying "Function components cannot be given refs. Attempts to access this ref will fail. Did you mean to use React.forwardRef()?": https://nextjs.org/docs/api-reference/next/link#if-the-child-is-a-function-component or https://reactjs.org/docs/forwarding-refs.html or https://deepscan.io/docs/rules/react-func-component-invalid-ref-prop or https://www.carlrippon.com/react-forwardref-typescript/
                    // TODO Actually, `value` is neither `null` nor `undefined` but the type system does not know about it. How can we make it know about it so we don't need `|| ""` here?
                    <Link href={paths.dataFormat(value || "")}>{value}</Link>
                  ),
                },
              ],
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            title: "Nearnormal Hemispherical Visible Transmittances",
            key: "nearnormalHemisphericalVisibleTransmittances",
            render: (_text, record, _index) =>
              record.nearnormalHemisphericalVisibleTransmittances
                .map((x) => x.toLocaleString("en"))
                .join(", "),
          },
        ]}
        dataSource={data}
      />
      <Typography.Paragraph style={{ maxWidth: 768 }}>
        The{" "}
        <Typography.Link
          href={`${process.env.NEXT_PUBLIC_METABASE_URL}/graphql/`}
        >
          GraphQL endpoint
        </Typography.Link>{" "}
        is the most powerful way of querying the databases.
      </Typography.Paragraph>
    </Layout>
  );
}

export default Index;
