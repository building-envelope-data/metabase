import { Select, Form } from "antd";
import BaseFilterSubform from "./BaseFilterSubform";
import { EnumFilterInput } from "../../lib/filter";
import { FilterTypeMap } from "../../lib/filter";

export default function EnumFilterSubform<
  TFilterInput extends EnumFilterInput<TEnum>,
  TEnum extends object,
>({
  name,
  ancestors,
  enumObject,
}: {
  name: readonly (string | number)[];
  ancestors: readonly (string | number)[];
  enumObject: TEnum;
}) {
  const selectOptions = Object.entries(enumObject).map(([_key, value]) => ({
    label: value,
    value: value,
  }));

  return (
    <BaseFilterSubform<TFilterInput>
      name={name}
      ancestors={ancestors}
      operators={FilterTypeMap["enum"].operators}
      initialOperator={FilterTypeMap["enum"].initialOperator}
      renderSingleFormItem={(props) => (
        <Form.Item
          name={props.name}
          noStyle
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Select options={selectOptions} style={{ width: "100%" }} />
        </Form.Item>
      )}
      renderMultipleFormItem={(props) => (
        <Form.Item
          name={props.name}
          noStyle
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Select
            mode="multiple"
            options={selectOptions}
            style={{ width: "100%" }}
          />
        </Form.Item>
      )}
    />
  );
}
