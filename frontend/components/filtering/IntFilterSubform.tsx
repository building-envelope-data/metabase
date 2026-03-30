import { Form, InputNumber, Select } from "antd";
import BaseFilterSubform from "./BaseFilterSubform";
import { IntFilterInput } from "../../__generated__/graphql";
import { FilterTypeMap } from "../../lib/filter";

export default function IntFilterSubform<TFilterInput extends IntFilterInput>({
  name,
  ancestors,
}: {
  name: readonly (string | number)[];
  ancestors: readonly (string | number)[];
}) {
  return (
    <BaseFilterSubform<TFilterInput>
      name={name}
      ancestors={ancestors}
      operators={FilterTypeMap["int"].operators}
      initialOperator={FilterTypeMap["int"].initialOperator}
      renderSingleFormItem={(props) => (
        <Form.Item
          name={props.name}
          noStyle
          rules={[
            {
              required: true,
            },
            {
              whitespace: true,
            },
          ]}
        >
          <InputNumber style={{ width: "100%" }} />
        </Form.Item>
      )}
      renderMultipleFormItem={(props) => (
        <Form.Item
          name={props.name}
          noStyle
          normalize={(value: any[] | null) =>
            value ? value.map((v) => parseInt(v)).filter((v) => !isNaN(v)) : []
          }
          rules={[
            {
              required: true,
            },
            {
              validator: (_, value) => {
                if (value?.some((v: any) => isNaN(parseInt(v)))) {
                  return Promise.reject(
                    new Error("All entries must be valid integers"),
                  );
                }
                return Promise.resolve();
              },
            },
          ]}
        >
          <Select
            mode="tags"
            tokenSeparators={[",", " "]}
            onInputKeyDown={(e) => {
              if (
                !/[0-9-]/.test(e.key) &&
                e.key !== "Enter" &&
                e.key !== "Tab" &&
                e.key !== "Backspace"
              ) {
                e.preventDefault();
              }
            }}
            style={{ width: "100%" }}
          />
        </Form.Item>
      )}
    />
  );
}
