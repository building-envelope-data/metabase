import Layout from "../../components/Layout";
import { Table, message, Form, Button, Alert, Typography } from "antd";
import { useAllPhotovoltaicDataQuery } from "../../queries/data.graphql";
import {
  PhotovoltaicData,
  Scalars,
  PhotovoltaicDataPropositionInput,
} from "../../__generated__/__types__";
import { useState } from "react";
import { setMapValue } from "../../lib/freeTextFilter";
import {
  getAppliedMethodColumnProps,
  getComponentUuidColumnProps,
  getDescriptionColumnProps,
  getNameColumnProps,
  getResourceTreeColumnProps,
  getTimestampColumnProps,
  getUuidColumnProps,
} from "../../lib/table";
import {
  UuidPropositionComparator,
  UuidPropositionFormList,
} from "../../components/UuidPropositionFormList";

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
  proposition: PhotovoltaicDataPropositionInput
): PhotovoltaicDataPropositionInput => {
  switch (negator) {
    case Negator.Is:
      return proposition;
    case Negator.IsNot:
      // return { not: proposition };
      return proposition; // TODO Support `not` in filter!
  }
};

const conjunct = (
  propositions: PhotovoltaicDataPropositionInput[]
): PhotovoltaicDataPropositionInput => {
  if (propositions.length == 0) {
    return {};
  }
  if (propositions.length == 1) {
    return propositions[0];
  }
  return { and: propositions };
};

// const disjunct = (
//   propositions: PhotovoltaicDataPropositionInput[]
// ): PhotovoltaicDataPropositionInput => {
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
  const [data, setData] = useState<PhotovoltaicData[]>([]);
  // Using `skip` is inspired by https://github.com/apollographql/apollo-client/issues/5268#issuecomment-749501801
  // An alternative would be `useLazy...` as told in https://github.com/apollographql/apollo-client/issues/5268#issuecomment-527727653
  // `useLazy...` does not return a `Promise` though as `use...Query.refetch` does which is used below.
  // For error policies see https://www.apollographql.com/docs/react/v2/data/error-handling/#error-policies
  const allPhotovoltaicDataQuery = useAllPhotovoltaicDataQuery({
    skip: true,
    errorPolicy: "all",
  });

  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

  const onFinish = ({
    componentIds,
  }: {
    componentIds:
      | {
          negator: Negator;
          comparator: UuidPropositionComparator;
          value: Scalars["Uuid"] | undefined;
        }[]
      | undefined;
  }) => {
    const filter = async () => {
      try {
        setFiltering(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const propositions: PhotovoltaicDataPropositionInput[] = [];
        if (componentIds) {
          for (let { negator, comparator, value } of componentIds) {
            propositions.push(
              negateIfNecessary(negator, {
                componentId: { [comparator]: value },
              })
            );
          }
        }
        const { error, data } = await allPhotovoltaicDataQuery.refetch(
          propositions.length == 0
            ? {}
            : {
                where: conjunct(propositions),
              }
        );
        if (error) {
          // TODO Handle properly.
          console.log(error);
          message.error(
            error.graphQLErrors.map((error) => error.message).join(" ")
          );
        }
        // TODO Casting to `PhotovoltaicData` is wrong and error prone!
        const nestedData =
          data?.databases?.edges?.map(
            (edge) => edge?.node?.allPhotovoltaicData?.nodes || []
          ) || [];
        const flatData = ([] as PhotovoltaicData[]).concat(...nestedData);
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
      <Typography.Title>Photovoltaic Data</Typography.Title>
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
            ...getComponentUuidColumnProps<typeof data[0]>(
              onFilterTextChange,
              (x) => filterText.get(x)
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
            ...getAppliedMethodColumnProps<typeof data[0]>(
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
          {
            ...getResourceTreeColumnProps<typeof data[0]>(
              onFilterTextChange,
              (x) => filterText.get(x)
            ),
          },
        ]}
        dataSource={data}
      />
    </Layout>
  );
}

export default Page;
