import { Select } from "antd";

export type SelectMultipleViaSearchProps<ValueType> = {
  options: { label: string; value: ValueType }[];
  value?: ValueType;
  onChange?: (value: ValueType) => void;
};

export function SelectMultipleViaSearch<ValueType extends string>({
  options,
  value,
  onChange,
}: SelectMultipleViaSearchProps<ValueType>) {
  return (
    <Select
      showSearch
      mode="multiple"
      placeholder="Please select"
      options={options}
      optionFilterProp="label"
      filterOption={(input, option) =>
        option?.label?.toLocaleString("en").includes(input) || false
      }
      filterSort={(optionA, optionB) =>
        optionA.label != null && optionB.label != null
          ? optionA.label
              .toLocaleString("en")
              .localeCompare(optionB.label.toLocaleString("en"), "en")
          : 0
      }
      value={value}
      onChange={onChange}
    />
  );
}
