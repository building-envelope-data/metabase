import { List } from "antd";
import { SortOrder } from "antd/lib/table/interface";
import { getFreeTextFilterProps, highlight } from "./freeTextFilter";

const sortDirections: SortOrder[] = ["ascend", "descend"];

function compare<ValueType>(
  x: ValueType | null | undefined,
  y: ValueType | null | undefined,
  nonNullCompare: (a: ValueType, b: ValueType) => number
) {
  if (x != null && y != null) {
    return nonNullCompare(x, y);
  }
  if (x == null && y != null) {
    return -1;
  }
  if (x != null && y == null) {
    return 1;
  }
  return 0;
}

function compareLexicographically<ValueType>(
  xs: ValueType[],
  ys: ValueType[],
  elementCompare: (x: ValueType, y: ValueType) => number
) {
  let i = 0;
  while (i < xs.length && i < ys.length) {
    const comparison = elementCompare(xs[i], ys[i]);
    if (comparison !== 0) {
      return comparison;
    }
    i++;
  }
  return xs.length - ys.length;
}

export function getColumnProps<RecordType>(
  title: string,
  key: keyof RecordType
) {
  return {
    title: title,
    key: key,
    // dataIndex: key,
  };
}

export function getStringColumnProps<RecordType>(
  title: string,
  key: keyof RecordType,
  getValue: (record: RecordType) => string | null | undefined
) {
  return {
    ...getColumnProps(title, key),
    sorter: (a: RecordType, b: RecordType) =>
      compare(getValue(a), getValue(b), (x, y) => x.localeCompare(y, "en")),
    sortDirections: sortDirections,
  };
}

export function getFilterableStringColumnProps<RecordType>(
  title: string,
  key: keyof RecordType,
  getValue: (record: RecordType) => string | null | undefined,
  onFilterTextChange: (
    key: keyof RecordType
  ) => (newFilterText: string) => void,
  getFilterText: (key: keyof RecordType) => string | undefined,
  doRender: (
    record: RecordType,
    highlightedValue: JSX.Element,
    value: string | null | undefined
  ) => JSX.Element = (_record, highlightedValue, _value) => highlightedValue
) {
  return {
    ...getStringColumnProps(title, key, getValue),
    ...getFreeTextFilterProps<RecordType>(getValue, onFilterTextChange(key)),
    render: (_text: string, record: RecordType, _index: number) =>
      doRender(
        record,
        highlight(getValue(record), getFilterText(key)),
        getValue(record)
      ),
  };
}

// TODO Use `EnumType extends enum` once there is an `enum` constraint as asked for in https://github.com/microsoft/TypeScript/issues/30611
export function getEnumListColumnProps<RecordType, EnumType extends string>(
  title: string,
  key: keyof RecordType,
  getValues: (record: RecordType) => EnumType[] | null | undefined
) {
  return {
    ...getColumnProps(title, key),
    sorter: (a: RecordType, b: RecordType) =>
      compare(getValues(a), getValues(b), (xs, ys) =>
        compareLexicographically(xs, ys, (x, y) => x.localeCompare(y, "en"))
      ),
    sortDirections: sortDirections,
  };
}

export function getFilterableEnumListColumnProps<
  RecordType,
  EnumType extends string
>(
  title: string,
  key: keyof RecordType,
  entries: [string, EnumType][],
  getValues: (record: RecordType) => EnumType[] | null | undefined,
  // TODO Call `onFilterTextChange` when the filter text changes. Note though that it cannot be called inside `onFilter` because that function is being called on render and we may not change state on render!
  _onFilterTextChange: (
    key: keyof RecordType
  ) => (newFilterText: string) => void,
  getFilterText: (key: keyof RecordType) => string | undefined,
  doRender: (
    record: RecordType,
    highlightedValue: JSX.Element,
    value: EnumType
  ) => JSX.Element = (_record, highlightedValue, _value) => highlightedValue
) {
  return {
    ...getEnumListColumnProps(title, key, getValues),
    filters: entries.map(([key, value]) => ({
      text: key,
      value: value,
    })),
    onFilter: (value: string | number | boolean, record: RecordType) => {
      const values = getValues(record);
      if (
        typeof value === "string" &&
        entries
          .map(([_enumKey, enumValue]) => enumValue.toString())
          .includes(value)
      ) {
        return values?.includes(value as EnumType) || false;
      }
      return true;
    },
    render: (_value: string, record: RecordType, _index: number) => {
      const values = getValues(record);
      if (values == null) {
        return null;
      } else {
        return (
          <List>
            {values.map((value) => (
              <List.Item key={value}>
                {doRender(record, highlight(value, getFilterText(key)), value)}
              </List.Item>
            ))}
          </List>
        );
      }
    },
  };
}
