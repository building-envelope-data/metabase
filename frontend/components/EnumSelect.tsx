import { Select, SelectProps } from "antd";
import { humanize } from "../lib/string";

export const allEnumSelectOptions = <T extends string | number>(
  enumObject: Record<string, T>,
  filter?: (value: T) => boolean,
) =>
  Object.entries(enumObject)
    // exclude reverse mappings
    .filter(([key]) => isNaN(Number(key)))
    .filter(([_, value]) => filter?.(value) ?? true)
    .map(([_, value]) => ({
      label: humanize(String(value), "all-upper"),
      value: value,
    }));

interface EnumSelectProps<T extends string | number> extends Omit<
  SelectProps<T>,
  "options"
> {
  enumObject: Record<string, T>;
  filter?: (value: T) => boolean;
}

export default function EnumSelect<T extends string | number>({
  enumObject,
  filter = () => true,
  ...rest
}: EnumSelectProps<T>) {
  return (
    <Select<T> {...rest} options={allEnumSelectOptions(enumObject, filter)} />
  );
}
