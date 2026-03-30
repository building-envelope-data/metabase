import { Input, Form } from "antd";
import { uuidRegex } from "../lib/string";
import { CSSProperties } from "react";

export default function UuidFormItem({
  name,
  style,
}: {
  name: (string | number)[];
  style?: CSSProperties;
}) {
  return (
    <Form.Item
      name={name}
      noStyle
      // normalize makes the cursor jump to the end: normalize={(value) => value?.toLowerCase()}
      rules={[
        {
          required: true,
        },
        {
          whitespace: true,
        },
        {
          pattern: uuidRegex,
          message:
            "Invalid UUID format (e.g. 123e4567-e89b-12d3-a456-426614174000)",
        },
      ]}
    >
      <Input
        style={{ fontFamily: "monospace", ...style }}
        maxLength={36}
        placeholder="xxxxxxxx-xxxx-Mxxx-Nxxx-xxxxxxxxxxxx"
      />
    </Form.Item>
  );
}
