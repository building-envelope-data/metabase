import { FilterOutlined, PlusOutlined } from "@ant-design/icons";
import { Button, Card, Flex, Form, Space } from "antd";
import {
  QueryDocument,
  QueryVariables,
  usePaginatedQuery,
} from "../../lib/hooks/usePaginatedQuery";
import Pagination from "../Pagination";
import { useMemo, useRef, useState } from "react";
import ActiveFilterAndSortBar from "../ActiveFilterAndSortBar";
import {
  ObjectFilterState,
  FilterDefinition,
  toWhereClause,
  getInitialFilterSubformValues,
  ObjectFilterDefinition,
} from "../../lib/filter";
import {
  SortState,
  SortDefinition,
  toOrderClause,
  initialSortSubformValues,
} from "../../lib/sort";
import JumpToId, { JumpToIdProps } from "../JumpToId";
import { Scalars, SortEnumType } from "../../__generated__/graphql";
import { Route } from "next";
import DeleteButton from "../DeleteButton";
import ObjectFilterSubform from "../filtering/ObjectFilterSubform";
import SortSubform from "../sorting/SortSubform";
import QueryToolbar from "../QueryToolbar";
import SlideDown from "../SlideDown";
import { notEmpty } from "../../lib/array";

const reduceClauses = <TFilterInput, TSortInput>(clauses: {
  where: { and: TFilterInput[] };
  order: TSortInput[];
}) => {
  if (clauses.where.and.length == 0 && clauses.order.length == 0) {
    return {};
  }
  if (clauses.where.and.length == 0) {
    return { order: clauses.order };
  }
  if (clauses.order.length == 0) {
    return { where: clauses.where };
  }
  return clauses;
};

type FilterAndSortFormValues = {
  filters: ObjectFilterState[] | null | undefined;
  sorts: SortState[] | null | undefined;
};

type BaseProps<TNode, TFilterInput, TSortInput> = {
  entitiesQuery: QueryDocument<TNode, TFilterInput, TSortInput>;
  list: (props: { loading: boolean; nodes: TNode[] }) => React.ReactNode;
  filterDefinitions: readonly FilterDefinition<TFilterInput>[];
  sortDefinitions: readonly SortDefinition<TSortInput>[];
  where?: TFilterInput | null;
  order?: TSortInput | null;
  loading?: boolean;
};

type Props<TNode, TFilterInput, TSortInput> =
  | (BaseProps<TNode, TFilterInput, TSortInput> & {
      showJump: false;
    })
  | (BaseProps<TNode, TFilterInput, TSortInput> & {
      showJump: true;
      route: (id: Scalars["Uuid"]["output"]) => Route;
      namesQuery?: JumpToIdProps["query"];
    });

export default function PaginatedEntities<
  TNode,
  TFilterInput extends {
    [x: string]: { [x: string]: any | null } | null;
  },
  TSortInput extends { [x: string]: SortEnumType | null },
>(props: Props<TNode, TFilterInput, TSortInput>) {
  // const router = useRouter();
  const [form] = Form.useForm<FilterAndSortFormValues>();
  const [filters, setFilters] = useState<readonly ObjectFilterState[]>([]);
  const [sorts, setSorts] = useState<readonly SortState[]>([]);
  const [isFilterAndSortOpen, setIsFilterAndSortOpen] = useState(false);
  const [paginatedQueryVariables, setPaginatedQueryVariables] =
    useState<QueryVariables<TFilterInput, TSortInput> | null>(null);
  const filterFormListRemoveRef =
    useRef<(index: number | number[]) => void | null>(null);
  const sortFormListRemoveRef =
    useRef<(index: number | number[]) => void | null>(null);

  const applyFiltersAndSortsFormValues = (values: FilterAndSortFormValues) => {
    setFilters(values.filters ?? []);
    setSorts(values.sorts ?? []);
    // setIsFilterAndSortOpen(false);
  };
  const clearFiltersAndSortsForm = () => {
    // form.resetFields();
    form.setFieldsValue({ filters: [], sorts: [] });
    // setIsFilterAndSortOpen(false);
  };

  const removeFilter = (index: number) => {
    filterFormListRemoveRef.current?.(index);
    const newFilters = filters.filter((_, i) => i !== index);
    setFilters(newFilters);
    setIsFilterAndSortOpen(
      (current) => current && (newFilters.length > 0 || sorts.length > 0),
    );
  };
  const removeSort = (index: number) => {
    sortFormListRemoveRef.current?.(index);
    const newSorts = sorts.filter((_, i) => i !== index);
    setSorts(newSorts);
    setIsFilterAndSortOpen(
      (current) => current && (filters.length > 0 || newSorts.length > 0),
    );
  };
  const removeAllFiltersAndSorts = () => {
    // form.resetFields();
    form.setFieldsValue({ filters: [], sorts: [] });
    setFilters([]);
    setSorts([]);
    setIsFilterAndSortOpen(false);
  };

  // useEffect(() => {
  //   if (!router.isReady) return;
  //   const { filters, sorts } = router.query;
  //   form.setFieldsValue({
  //     filters: filters ? JSON.parse(filters as string) : [],
  //     sorts: sorts ? JSON.parse(sorts as string) : [],
  //   });
  // }, [router.isReady, router.query, form]);

  // const handleValuesChange = (_: any, values: FilterAndSortFormValues) => {
  //   const { filters, sorts } = values;
  //   router.push(
  //     {
  //       pathname: router.pathname,
  //       query: {
  //         ...router.query,
  //         filters: JSON.stringify(filters),
  //         sorts: JSON.stringify(sorts),
  //       },
  //     },
  //     undefined,
  //     { shallow: true }, // Prevents calling getServerSideProps again
  //   );
  // };

  const queryVariables = useMemo(
    () =>
      reduceClauses({
        where: {
          and: [
            props.where,
            ...filters.map<TFilterInput>((filter) =>
              toWhereClause(filter, props.filterDefinitions),
            ),
          ].filter(notEmpty),
        },
        order: [
          props.order,
          ...sorts.map((sort) => toOrderClause(sort, props.sortDefinitions)),
        ].filter(notEmpty),
      }),
    [filters, sorts],
  );
  const { loading, nodes, paginationProps } = usePaginatedQuery(
    props.entitiesQuery,
    queryVariables,
    setPaginatedQueryVariables,
  );

  const objectFilterDefinition: ObjectFilterDefinition<any, any> = {
    type: "object",
    field: null,
    items: props.filterDefinitions as readonly FilterDefinition<any>[],
  };

  return (
    <div>
      <Flex vertical gap="medium">
        <Flex justify="space-between" align="baseline">
          {props.showJump ? (
            <JumpToId query={props.namesQuery} route={props.route} />
          ) : (
            <div />
          )}
          <Button
            type={isFilterAndSortOpen ? "text" : "default"}
            icon={<FilterOutlined />}
            onClick={() => setIsFilterAndSortOpen((x) => !x)}
          >
            Filter & Sort
          </Button>
        </Flex>
        <SlideDown open={isFilterAndSortOpen}>
          <Card variant="borderless">
            <Form
              form={form}
              layout="vertical"
              onFinish={applyFiltersAndSortsFormValues}
              initialValues={{ filters, sorts }}
            >
              <Flex vertical gap="medium">
                {props.filterDefinitions.length > 0 && (
                  <div>
                    <h3>Filter by</h3>
                    <Form.List name="filters">
                      {(fields, { add, remove }, { errors }) => {
                        filterFormListRemoveRef.current = remove;
                        return (
                          <Flex vertical gap="small">
                            <Form.ErrorList errors={errors} />
                            {fields.map(({ key, name }) => {
                              return (
                                <Flex key={key} gap="small" align="baseline">
                                  <ObjectFilterSubform
                                    name={[name]}
                                    ancestors={["filters"]}
                                    definition={objectFilterDefinition}
                                  />
                                  <DeleteButton
                                    kind="remove"
                                    type="icon"
                                    onClick={() => remove(name)}
                                  />
                                </Flex>
                              );
                            })}
                            <Button
                              block
                              type="dashed"
                              onClick={() =>
                                add(
                                  getInitialFilterSubformValues(
                                    objectFilterDefinition,
                                  ),
                                )
                              }
                              icon={<PlusOutlined />}
                            >
                              Add Filter
                            </Button>
                          </Flex>
                        );
                      }}
                    </Form.List>
                  </div>
                )}
                {props.sortDefinitions.length > 0 && (
                  <div>
                    <h3>Sort by</h3>
                    <Form.List name="sorts">
                      {(fields, { add, remove }, { errors }) => {
                        sortFormListRemoveRef.current = remove;
                        return (
                          <Flex vertical gap="small">
                            <Form.ErrorList errors={errors} />
                            {fields.map(({ key, name }) => {
                              return (
                                <Flex key={key} gap="small" align="baseline">
                                  <SortSubform
                                    name={[name]}
                                    definitions={props.sortDefinitions}
                                  />
                                  <DeleteButton
                                    kind="remove"
                                    type="icon"
                                    onClick={() => remove(name)}
                                  />
                                </Flex>
                              );
                            })}
                            <Button
                              block
                              type="dashed"
                              onClick={() => add(initialSortSubformValues)}
                              icon={<PlusOutlined />}
                            >
                              Add Sort
                            </Button>
                          </Flex>
                        );
                      }}
                    </Form.List>
                  </div>
                )}
                <div
                  style={{
                    display: "flex",
                    justifyContent: "flex-end",
                  }}
                >
                  <Space>
                    <DeleteButton
                      title="Clear"
                      type="default"
                      onClick={clearFiltersAndSortsForm}
                    />
                    <Button
                      type="default"
                      onClick={() => setIsFilterAndSortOpen(false)}
                    >
                      Close
                    </Button>
                    <Button type="primary" onClick={form.submit}>
                      Apply
                    </Button>
                  </Space>
                </div>
              </Flex>
            </Form>
          </Card>
        </SlideDown>
        {(filters.length > 0 || sorts.length > 0) && (
          <ActiveFilterAndSortBar
            values={{ filters, sorts }}
            filterDefinitions={props.filterDefinitions}
            sortDefinitions={props.sortDefinitions}
            onRemoveFilter={removeFilter}
            onRemoveSort={removeSort}
            onRemoveAll={removeAllFiltersAndSorts}
          />
        )}
        {props.list({ loading: props.loading || loading, nodes })}
        <Flex justify="space-between" align="baseline">
          <QueryToolbar
            query={props.entitiesQuery}
            variables={paginatedQueryVariables}
          />
          <Pagination {...paginationProps} />
        </Flex>
      </Flex>
    </div>
  );
}
