import { Input, Space } from "antd";

// or `UrlFormItem`? With `<FormItem normalize={normalize}`

const normalize = (value: string) => {
  if (!value) return value;
  // If it doesn't start with http or https, prepend https://
  if (!/^https?:\/\//i.test(value) && value.length > 3) {
    return `https://${value}`;
  }
  return value.toLowerCase().trim();
};

export default function UrlInput({
  value,
  onChange,
}: {
  value?: string;
  onChange?: (v: string) => void;
}) {
  return (
    <Space.Compact>
      <Input
        value={value}
        onChange={(e) => onChange?.(normalize(e.target.value))}
        placeholder="https://..."
      />
      <a href={value} target="_blank" rel="noreferrer">
        Test Link
      </a>
    </Space.Compact>
  );
}
