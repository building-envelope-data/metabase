import { Select, SelectProps } from "antd";
import { humanize } from "../lib/string";

export const allEnumSelectOptions = <T extends string | number>(
  enumObject: Record<string, T>,
) =>
  Object.entries(enumObject)
    // exclude reverse mappings
    .filter(([key]) => isNaN(Number(key)))
    .map(([_, value]) => ({
      label: humanize(String(value), "all-upper"),
      value: value,
    }));

interface EnumSelectProps<T extends string | number> extends Omit<
  SelectProps<T>,
  "options"
> {
  enumObject: Record<string, T>;
}

export default function EnumSelect<T extends string | number>({
  enumObject,
  ...rest
}: EnumSelectProps<T>) {
  return <Select<T> {...rest} options={allEnumSelectOptions(enumObject)} />;
}
