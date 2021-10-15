import Layout from "../components/Layout";
import {
  Table,
  message,
  Form,
  Button,
  Alert,
  Typography,
  Descriptions,
} from "antd";
import { useAllOpticalDataQuery } from "../queries/data.graphql";
import {
  OpticalData,
  Scalars,
  OpticalDataPropositionInput,
} from "../__generated__/__types__";
import { useState } from "react";
import Link from "next/link";
import paths from "../paths";
import { setMapValue } from "../lib/freeTextFilter";
import {
  getDescriptionColumnProps,
  getFilterableDescriptionListColumnProps,
  getInternallyLinkedFilterableStringColumnProps,
  getNameColumnProps,
  getTimestampColumnProps,
  getUuidColumnProps,
} from "../lib/table";
import {
  FloatPropositionComparator,
  FloatPropositionFormList,
} from "../components/FloatPropositionFormList";
import {
  UuidPropositionComparator,
  UuidPropositionFormList,
} from "../components/UuidPropositionFormList";

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

function Page() {
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
    infraredEmittances,
    nearnormalHemisphericalSolarReflectances,
    nearnormalHemisphericalSolarTransmittances,
    nearnormalHemisphericalVisibleReflectances,
    nearnormalHemisphericalVisibleTransmittances,
  }: {
    componentIds:
      | {
          negator: Negator;
          comparator: UuidPropositionComparator;
          value: Scalars["UUID"] | undefined;
        }[]
      | undefined;
    infraredEmittances:
      | {
          negator: Negator;
          comparator: FloatPropositionComparator;
          value: number | undefined;
        }[]
      | undefined;
    nearnormalHemisphericalSolarReflectances:
      | {
          negator: Negator;
          comparator: FloatPropositionComparator;
          value: number | undefined;
        }[]
      | undefined;
    nearnormalHemisphericalSolarTransmittances:
      | {
          negator: Negator;
          comparator: FloatPropositionComparator;
          value: number | undefined;
        }[]
      | undefined;
    nearnormalHemisphericalVisibleReflectances:
      | {
          negator: Negator;
          comparator: FloatPropositionComparator;
          value: number | undefined;
        }[]
      | undefined;
    nearnormalHemisphericalVisibleTransmittances:
      | {
          negator: Negator;
          comparator: FloatPropositionComparator;
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
          for (let { negator, comparator, value } of componentIds) {
            propositions.push(
              negateIfNecessary(negator, {
                componentId: { [comparator]: value },
              })
            );
          }
        }
        // Note that `0` evaluates to `false`, so below we cannot use
        // `if (nearnormalHemisphericalVisibleTransmittance)`.
        if (infraredEmittances) {
          for (let { negator, comparator, value } of infraredEmittances) {
            if (value !== undefined && value !== null) {
              propositions.push(
                negateIfNecessary(negator, {
                  infraredEmittance: {
                    [comparator]: value,
                  },
                })
              );
            }
          }
        }
        if (nearnormalHemisphericalSolarReflectances) {
          for (let {
            negator,
            comparator,
            value,
          } of nearnormalHemisphericalSolarReflectances) {
            if (value !== undefined && value !== null) {
              propositions.push(
                negateIfNecessary(negator, {
                  nearnormalHemisphericalSolarReflectance: {
                    [comparator]: value,
                  },
                })
              );
            }
          }
        }
        if (nearnormalHemisphericalSolarTransmittances) {
          for (let {
            negator,
            comparator,
            value,
          } of nearnormalHemisphericalSolarTransmittances) {
            if (value !== undefined && value !== null) {
              propositions.push(
                negateIfNecessary(negator, {
                  nearnormalHemisphericalSolarTransmittance: {
                    [comparator]: value,
                  },
                })
              );
            }
          }
        }
        if (nearnormalHemisphericalVisibleReflectances) {
          for (let {
            negator,
            comparator,
            value,
          } of nearnormalHemisphericalVisibleReflectances) {
            if (value !== undefined && value !== null) {
              propositions.push(
                negateIfNecessary(negator, {
                  nearnormalHemisphericalVisibleReflectance: {
                    [comparator]: value,
                  },
                })
              );
            }
          }
        }
        if (nearnormalHemisphericalVisibleTransmittances) {
          for (let {
            negator,
            comparator,
            value,
          } of nearnormalHemisphericalVisibleTransmittances) {
            if (value !== undefined && value !== null) {
              propositions.push(
                negateIfNecessary(negator, {
                  nearnormalHemisphericalVisibleTransmittance: {
                    [comparator]: value,
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
        <UuidPropositionFormList name="componentIds" label="Component Id" />
        <FloatPropositionFormList
          name="infraredEmittances"
          label="Infrared emittance"
        />
        <FloatPropositionFormList
          name="nearnormalHemisphericalSolarReflectances"
          label="Nearnormal hemispherical solar reflectance"
        />
        <FloatPropositionFormList
          name="nearnormalHemisphericalSolarTransmittances"
          label="Nearnormal hemispherical solar transmittance"
        />
        <FloatPropositionFormList
          name="nearnormalHemisphericalVisibleReflectances"
          label="Nearnormal hemispherical visible reflectance"
        />
        <FloatPropositionFormList
          name="nearnormalHemisphericalVisibleTransmittances"
          label="Nearnormal hemispherical visible transmittance"
        />

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
            ...getNameColumnProps<typeof data[0]>(onFilterTextChange, (x) =>
              filterText.get(x)
            ),
          },
          {
            ...getDescriptionColumnProps<typeof data[0]>(
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getTimestampColumnProps<typeof data[0]>(),
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
                  key: "appliedMethodId",
                  title: "UUID",
                  value: x.appliedMethod.methodId,
                  render: (_record, _highlightedValue, value) => (
                    // TODO Why does this not work with `_highlightedValue`? An error is raised saying "Function components cannot be given refs. Attempts to access this ref will fail. Did you mean to use React.forwardRef()?": https://nextjs.org/docs/api-reference/next/link#if-the-child-is-a-function-component or https://reactjs.org/docs/forwarding-refs.html or https://deepscan.io/docs/rules/react-func-component-invalid-ref-prop or https://www.carlrippon.com/react-forwardref-typescript/
                    // TODO Actually, `value` is neither `null` nor `undefined` but the type system does not know about it. How can we make it know about it so we don't need `|| ""` here?
                    <Link href={paths.method(value || "")}>{value}</Link>
                  ),
                },
                // {
                //   key: "appliedMethodName",
                //   title: "Name",
                //   value: x.appliedMethod.method?.name,
                // },
                // {
                //   key: "appliedMethodDescription",
                //   title: "Description",
                //   value: x.appliedMethod.method?.description,
                // },
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
            title: "Emittances, Reflectances, and Transmittances",
            key: "nearnormalHemisphericalX",
            render: (_text, record, _index) => (
              <Descriptions column={1}>
                <Descriptions.Item
                  key="infraredEmittances"
                  label="Infrared Emittances"
                >
                  {record.infraredEmittances
                    .map((x) => x.toLocaleString("en"))
                    .join(", ")}
                </Descriptions.Item>
                <Descriptions.Item
                  key="nearnormalHemisphericalSolarReflectances"
                  label="Nearnormal Hemispherical Solar Reflectances"
                >
                  {record.nearnormalHemisphericalSolarReflectances
                    .map((x) => x.toLocaleString("en"))
                    .join(", ")}
                </Descriptions.Item>
                <Descriptions.Item
                  key="nearnormalHemisphericalSolarTransmittances"
                  label="Nearnormal Hemispherical Solar Transmittances"
                >
                  {record.nearnormalHemisphericalSolarTransmittances
                    .map((x) => x.toLocaleString("en"))
                    .join(", ")}
                </Descriptions.Item>
                <Descriptions.Item
                  key="nearnormalHemisphericalVisibleReflectances"
                  label="Nearnormal Hemispherical Visible Reflectances"
                >
                  {record.nearnormalHemisphericalVisibleReflectances
                    .map((x) => x.toLocaleString("en"))
                    .join(", ")}
                </Descriptions.Item>
                <Descriptions.Item
                  key="nearnormalHemisphericalVisibleTransmittances"
                  label="Nearnormal Hemispherical Visible Transmittances"
                >
                  {record.nearnormalHemisphericalVisibleTransmittances
                    .map((x) => x.toLocaleString("en"))
                    .join(", ")}
                </Descriptions.Item>
              </Descriptions>
            ),
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

export default Page;
