import { Button, Space } from "antd";
import { CopyOutlined } from "@ant-design/icons";
import { ReactNode, useState } from "react";

export default function CopyableText({
  text,
  children,
}: {
  text: string;
  children?: ReactNode;
}) {
  const [copied, setCopied] = useState(false);

  return (
    <Space
      style={{
        fontFamily: "monospace",
      }}
    >
      <span>{children ? children : text}</span>
      <Button
        type="text"
        size="small"
        icon={<CopyOutlined />}
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
