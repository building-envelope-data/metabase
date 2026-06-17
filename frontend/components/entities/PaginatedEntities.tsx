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

const reduceWhere = <TFilterInput,>(where: { and: TFilterInput[] }) =>
  where.and.length == 0 ? null : where.and.length == 1 ? where.and[0] : where;

const reduceOrder = <TSortInput,>(
  order: TSortInput | TSortInput[] | null | undefined,
) => {
  if (!Array.isArray(order)) return order;
  return order.length == 0 ? null : order.length == 1 ? order[0] : order;
};

const reduceClauses = <TFilterInput, TSortInput>(clauses: {
  where: { and: TFilterInput[] };
  order?: TSortInput | TSortInput[] | null | undefined;
}) => {
  var where = reduceWhere(clauses.where);
  var order = reduceOrder(clauses.order);
  return {
    ...(where !== null && { where }),
    ...(order !== null && { order }),
  };
};

type FilterAndSortFormValues = {
  filters: ObjectFilterState[] | null | undefined;
  sorts: SortState[] | null | undefined;
};

type BaseProps<
  TNode,
  TFilterInput extends FilterInput,
  TSortInput extends SortInput,
> = {
  entitiesQuery: QueryDocument<TNode, TFilterInput, TSortInput>;
  extra?: React.ReactNode;
  list: (props: {
    loading: boolean;
    nodes: TNode[] | null;
    onReload: () => void;
  }) => React.ReactNode;
  filterDefinitions: readonly FilterDefinition<TFilterInput>[];
  sortDefinitions: readonly SortDefinition<TSortInput>[];
  baseWhere?: TFilterInput | null;
  defaultOrder?: TSortInput[] | TSortInput | null;
  loading?: boolean;
};

type Props<
  TNode,
  TFilterInput extends FilterInput,
  TSortInput extends SortInput,
> =
  | (BaseProps<TNode, TFilterInput, TSortInput> & {
      showJump: false;
    })
  | (BaseProps<TNode, TFilterInput, TSortInput> & {
      showJump: true;
      route: (id: Scalars["Uuid"]["output"]) => Route;
      namesQuery?: JumpToIdProps["query"];
    });

type FilterInput =
  | {
      and?: (FilterInput | undefined)[] | null;
      or?: (FilterInput | undefined)[] | null;
      [x: string]: { [x: string]: any | null | undefined } | null | undefined;
    }
  | null
  | undefined;

type SortInput =
  | { [x: string]: SortInput | SortEnumType | null | undefined }
  | { [x: string]: SortInput | SortEnumType | null | undefined }[]
  | null
  | undefined;

export default function PaginatedEntities<
  TNode,
  TFilterInput extends FilterInput,
  TSortInput extends SortInput,
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
            props.baseWhere,
            ...filters.map<TFilterInput>((filter) =>
              toWhereClause(filter, props.filterDefinitions),
            ),
          ].filter(notEmpty),
        },
        order:
          sorts.length == 0
            ? props.defaultOrder
            : sorts.map((sort) => toOrderClause(sort, props.sortDefinitions)),
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

  const jump = props.showJump && (
    <JumpToId
      query={props.namesQuery}
      route={props.route}
      style={{ width: props.extra ? "100%" : undefined }}
    />
  );

  return (
    <div>
      <Flex vertical gap="medium">
        {props.extra != null && jump}
        <Flex justify="space-between" align="baseline">
          {props.extra || jump ? (props.extra ?? jump) : <div />}
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
                      type="default"
                      onClick={clearFiltersAndSortsForm}
                    >
                      Clear
                    </DeleteButton>
                    <Button
                      type="default"
                      onClick={() => setIsFilterAndSortOpen(false)}
                    >
                      Close
                    </Button>
                    <Button
                      type="primary"
                      onClick={form.submit}
                      loading={loading}
                    >
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
        {props.list({
          loading: props.loading || loading,
          nodes,
          onReload: paginationProps.onReload,
        })}
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
