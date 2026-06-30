import { Tag, Space, Typography, Flex } from "antd";
import {
  ObjectFilterState,
  getFilterOperatorLabel,
  createFilterStateReducer,
  FilterDefinition,
  AnyFilterDefinition,
} from "../lib/filter";
import { formatSortDirection, SortState, SortDefinition } from "../lib/sort";
import DeleteButton from "./DeleteButton";
import { getLabel } from "../lib/string";

const stringifySort = <TSortInput,>(
  sort: SortState,
  definitions: readonly SortDefinition<TSortInput>[],
) => `${String(definitions[sort.index].field)}|${sort.direction}`;

const renderSort = <TSortInput,>(
  sort: SortState,
  definitions: readonly SortDefinition<TSortInput>[],
) => (
  <>
    {getLabel(definitions[sort.index], "none-upper")} (
    {formatSortDirection(sort.direction)})
  </>
);

const stringifyFilter = <TFilterInput,>(
  filter: ObjectFilterState,
  definitions: readonly FilterDefinition<TFilterInput>[],
): string =>
  createFilterStateReducer<string>(
    (scalar) =>
      `${getFilterOperatorLabel(scalar.operator)}|${JSON.stringify(scalar.value)}`,
    (list, _, stringifyValue) =>
      `${getFilterOperatorLabel(list.operator)}|${stringifyValue()}`,
    (object, context, stringifyValue) =>
      `${String(context.items[object.index].field)}|${stringifyValue()}`,
  )(filter, definitions as AnyFilterDefinition[]);

const renderFilter = <TFilterInput,>(
  value: ObjectFilterState,
  definitions: readonly FilterDefinition<TFilterInput>[],
) =>
  createFilterStateReducer<React.ReactNode>(
    (scalar) => (
      <>
        {getFilterOperatorLabel(scalar.operator)}{" "}
        {Array.isArray(scalar.value)
          ? `{${scalar.value.join(", ")}}`
          : scalar.value}
      </>
    ),
    (list, _, renderValue) => (
      <>
        {getFilterOperatorLabel(list.operator)} {renderValue()}
      </>
    ),
    (object, context, renderValue) => (
      <>
        {getLabel(context.items[object.index], "none-upper")} {renderValue()}
      </>
    ),
  )(value, definitions as AnyFilterDefinition[]);

export default function ActiveFilterAndSortBar<TFilterInput, TSortInput>({
  values,
  filterDefinitions,
  sortDefinitions,
  onRemoveFilter,
  onRemoveSort,
  onRemoveAll,
}: {
  values: {
    filters: readonly ObjectFilterState[];
    sorts: readonly SortState[];
  };
  filterDefinitions: readonly FilterDefinition<TFilterInput>[];
  sortDefinitions: readonly SortDefinition<TSortInput>[];
  onRemoveFilter: (index: number) => void;
  onRemoveSort: (index: number) => void;
  onRemoveAll: () => void;
}) {
  return (
    <Flex justify="space-between" align="baseline">
      <Space>
        {values.filters.length > 0 && (
          <>
            <Typography.Text type="secondary">Filtered by</Typography.Text>
            {values.filters.map((filter, index: number) => (
              <Tag
                color="green"
                closable
                closeIcon={
                  <DeleteButton type="icon" kind="remove">
                    Remove
                  </DeleteButton>
                }
                key={stringifyFilter(filter, filterDefinitions)}
                onClose={() => onRemoveFilter(index)}
              >
                {renderFilter(filter, filterDefinitions)}
              </Tag>
            ))}
          </>
        )}
        {values.sorts.length > 0 && (
          <>
            <Typography.Text type="secondary">Sorted by</Typography.Text>
            {values.sorts.map((sort, index: number) => (
              <Tag
                color="blue"
                closable
                closeIcon={<DeleteButton type="icon" kind="remove" />}
                key={stringifySort(sort, sortDefinitions)}
                onClose={() => onRemoveSort(index)}
              >
                {renderSort(sort, sortDefinitions)}
              </Tag>
            ))}
          </>
        )}
      </Space>
      {(values.filters.length > 0 || values.sorts.length > 0) && (
        <DeleteButton type="text" onClick={onRemoveAll}>
          Remove All
        </DeleteButton>
      )}
    </Flex>
  );
}
