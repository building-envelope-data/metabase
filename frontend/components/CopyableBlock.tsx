import { Button } from "antd";
import { CheckOutlined, CopyOutlined } from "@ant-design/icons";
import { ReactNode, useState } from "react";

export default function CopyableBlock({
  text,
  children,
}: {
  text: string;
  children: ReactNode;
}) {
  const [copied, setCopied] = useState(false);

  return (
    <div
      style={{
        position: "relative",
        paddingBottom: "1em",
        borderBottom: "1px solid grey",
      }}
    >
      {children}
      <Button
        style={{ position: "absolute", right: 0, bottom: 0 }}
        type="text"
        icon={copied ? <CheckOutlined /> : <CopyOutlined />}
        onClick={() => {
          navigator.clipboard.writeText(text);
          setCopied(true);
          setTimeout(() => setCopied(false), 2000);
        }}
      >
        {copied ? "Done" : "Copy"}
      </Button>
    </div>
  );
}
