import { Select, SelectProps } from "antd";

export interface SearchSelectProps<ValueType> extends Omit<
  SelectProps,
  "options" | "value"
> {
  options: { label: string; value: ValueType }[];
  mode?: "multiple" | "tags";
  value?: ValueType;
  onChange?: (value: ValueType) => void;
}

export default function SearchSelect<ValueType>({
  options,
  ...rest
}: SearchSelectProps<ValueType>) {
  return (
    <Select
      {...rest}
      placeholder="Search and select..."
      showSearch={{
        optionFilterProp: "label",
        filterOption: (input, option) =>
          option?.label?.toLocaleString().includes(input) || false,
        filterSort: (optionA, optionB) =>
          optionA.label != null && optionB.label != null
            ? optionA.label
                .toLocaleString()
                .localeCompare(optionB.label.toLocaleString(), "en")
            : 0,
      }}
    />
  );
}
