import { Select, SelectProps } from "antd";
import { humanize } from "../lib/string";

export const allEnumValues = <T extends string | number>(
  enumObject: Record<string, T>,
) =>
  Object.entries(enumObject)
    .filter(([key]) => isNaN(Number(key)))
    .map(([_, value]) => value);

export const allEnumSelectOptions = <T extends string | number>(
  values: T[],
  filter?: (value: T) => boolean,
) =>
  values
    // exclude reverse mappings
    .filter((value) => filter?.(value) ?? true)
    .map((value) => ({
      label: humanize(String(value), "all-upper"),
      value: value,
    }));

interface BaseProps<T extends string | number> extends Omit<
  SelectProps<T>,
  "options"
> {}

type EnumSelectProps<T extends string | number> =
  | (BaseProps<T> & {
      enumObject: Record<string, T>;
      filter?: (value: T) => boolean;
    })
  | (BaseProps<T> & {
      values: T[];
    });

export default function EnumSelect<T extends string | number>(
  props: EnumSelectProps<T>,
) {
  if ("enumObject" in props) {
    const { enumObject, filter, ...rest } = props;
    return (
      <Select<T>
        {...rest}
        options={allEnumSelectOptions(allEnumValues(enumObject))}
      />
    );
  } else {
    const { values, ...rest } = props;
    return <Select<T> {...rest} options={allEnumSelectOptions(values)} />;
  }
}
