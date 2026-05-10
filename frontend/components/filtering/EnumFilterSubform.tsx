import { Form } from "antd";
import BaseFilterSubform from "./BaseFilterSubform";
import { EnumFilterInput } from "../../lib/filter";
import { FilterTypeMap } from "../../lib/filter";
import EnumSelect from "../EnumSelect";

export default function EnumFilterSubform<
  TFilterInput extends EnumFilterInput<TEnum>,
  TEnum extends Record<string, TEnumValue>,
  TEnumValue extends string | number,
>({
  name,
  ancestors,
  enumObject,
}: {
  name: readonly (string | number)[];
  ancestors: readonly (string | number)[];
  enumObject: TEnum;
}) {
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
          <EnumSelect enumObject={enumObject} style={{ width: "100%" }} />
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
          <EnumSelect
            enumObject={enumObject}
            mode="multiple"
            style={{ width: "100%" }}
          />
        </Form.Item>
      )}
    />
  );
}
