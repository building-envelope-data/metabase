import { Button, Space } from "antd";
import { CopyOutlined, CheckOutlined } from "@ant-design/icons";
import { ReactNode, useState } from "react";

export default function Copyable({
  text,
  children,
}: {
  text: string;
  children?: ReactNode;
}) {
  const [copied, setCopied] = useState(false);

  return (
    <Space>
      {children == null ? <span>{text}</span> : children}
      <Button
        type="text"
        size="small"
        icon={copied ? <CheckOutlined /> : <CopyOutlined />}
        onClick={() => {
          navigator.clipboard.writeText(text);
          setCopied(true);
          setTimeout(() => setCopied(false), 2000);
        }}
      >
        {copied ? "Copied!" : "Copy"}
      </Button>
    </Space>
  );
}
