import { Button, Space } from "antd";
import { CopyOutlined } from "@ant-design/icons";
import { useState } from "react";

export default function CopyableText({ text }: { text: string }) {
  const [copied, setCopied] = useState(false);

  return (
    <Space
      style={{
        fontFamily: "monospace",
      }}
    >
      <span>{text}</span>
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
