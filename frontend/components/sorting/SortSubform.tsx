import { Space, Form, Select } from "antd";
import { SortEnumType } from "../../__generated__/graphql";
import { formatSortDirection, SortDefinition } from "../../lib/sort";
import { getLabel } from "../../lib/string";

const sortDirectionOptions = Object.entries(SortEnumType).map(
  ([_key, value]) => ({
    value: value,
    label: formatSortDirection(value),
  }),
);

export default function SortSubform<TSortInput>({
  name,
  definitions,
}: {
  name: (string | number)[];
  definitions: readonly SortDefinition<TSortInput>[];
}) {
  return (
    <Space.Compact style={{ flex: 1 }}>
      <Form.Item noStyle name={[...name, "index"]} rules={[{ required: true }]}>
        <Select
          options={definitions.map((value, index) => ({
            value: index,
            label: getLabel(value, "all-upper"),
          }))}
        />
      </Form.Item>
      <Form.Item noStyle name={[...name, "direction"]}>
        <Select options={sortDirectionOptions} style={{ width: "100%" }} />
      </Form.Item>
    </Space.Compact>
  );
}
