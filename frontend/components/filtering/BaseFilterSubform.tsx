import { Form, Select, Space } from "antd";
import React from "react";
import {
  getFilterOperatorLabel,
  isFilterOperatorSingle,
  FilterOperator,
} from "../../lib/filter";

type NarrowedOperator<TFilterInput> = FilterOperator & keyof TFilterInput;

export default function BaseFilterSubform<TFilterInput>({
  name,
  ancestors,
  operators,
  renderSingleFormItem,
  renderMultipleFormItem,
  initialOperator,
}: {
  _type?: TFilterInput;
  name: readonly (string | number)[];
  ancestors: readonly (string | number)[];
  operators: readonly NarrowedOperator<TFilterInput>[];
  renderSingleFormItem: (props: {
    name: (string | number)[];
  }) => React.ReactNode;
  renderMultipleFormItem: (props: {
    name: (string | number)[];
  }) => React.ReactNode;
  initialOperator: number;
}) {
  const form = Form.useFormInstance();
  const currentOperator =
    (Form.useWatch(
      [...ancestors, ...name, "operator"],
      form,
    ) as NarrowedOperator<TFilterInput>) ?? operators[initialOperator];

  return (
    <Space.Compact style={{ flex: 1 }}>
      <Form.Item name={[...name, "operator"]} noStyle>
        <Select
          popupMatchSelectWidth={false}
          options={operators.map((key) => ({
            value: key,
            label: getFilterOperatorLabel(key),
          }))}
        />
      </Form.Item>
      <Form.Item noStyle style={{ flex: 1 }}>
        {isFilterOperatorSingle(currentOperator)
          ? renderSingleFormItem({ name: [...name, "value"] })
          : renderMultipleFormItem({ name: [...name, "value"] })}
      </Form.Item>
    </Space.Compact>
  );
}
