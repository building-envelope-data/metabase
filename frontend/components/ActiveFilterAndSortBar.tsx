import { Tag, Space, Typography, Flex } from "antd";
import {
  ObjectFilterState,
  getFilterOperatorLabel,
  createFilterStateReducer,
  FilterDefinition,
  FilterStateReducerContext,
} from "../lib/filter";
import { formatSortDirection, SortState, SortDefinition } from "../lib/sort";
import DeleteButton from "./DeleteButton";
import { Key } from "react";
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
): Key =>
  createFilterStateReducer<string>(
    (scalar) =>
      `${getFilterOperatorLabel(scalar.operator)}|${JSON.stringify(scalar.value)}`,
    (list, stringify) =>
      `${getFilterOperatorLabel(list.operator)}|${stringify(list.value)}`,
    (object, context, stringify) =>
      `${String(context[object.index].field)}|${stringify(object.value)}`,
  )(filter, definitions as FilterStateReducerContext);

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
    (list, render) => (
      <>
        {getFilterOperatorLabel(list.operator)} {render(list.value)}
      </>
    ),
    (object, context, render) => (
      <>
        {getLabel(context[object.index], "none-upper")} {render(object.value)}
      </>
    ),
  )(value, definitions as FilterStateReducerContext);

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
                  <DeleteButton type="icon" kind="remove" title="Remove" />
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
        <DeleteButton title="Remove All" type="text" onClick={onRemoveAll} />
      )}
    </Flex>
  );
}
