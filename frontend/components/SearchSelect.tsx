import { Select } from "antd";

export type SearchSelectProps<ValueType> = {
  options: { label: string; value: ValueType }[];
  mode?: "multiple" | "tags";
  value?: ValueType;
  onChange?: (value: ValueType) => void;
};

export function SearchSelect<ValueType extends string>({
  options,
  mode,
  value,
  onChange,
}: SearchSelectProps<ValueType>) {
  return (
    <Select
      showSearch
      mode={mode}
      placeholder="Please select"
      options={options}
      optionFilterProp="label"
      filterOption={(input, option) =>
        option?.label?.toLocaleString().includes(input) || false
      }
      filterSort={(optionA, optionB) =>
        optionA.label != null && optionB.label != null
          ? optionA.label
              .toLocaleString()
              .localeCompare(optionB.label.toLocaleString(), "en")
          : 0
      }
      value={value}
      onChange={onChange}
    />
  );
}
