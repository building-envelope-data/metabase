import { Scalars } from "../__generated__/__types__";
import { Select } from "antd";

export type SelectMultipleViaSearchProps = {
  options: { label: string; value: Scalars["Uuid"] }[];
};

export function SelectMultipleViaSearch({
  options,
}: SelectMultipleViaSearchProps) {
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
    />
  );
}
