import { Button } from "antd";
import { CheckOutlined, CopyOutlined } from "@ant-design/icons";
import { ReactNode, useState } from "react";

export default function CopyableBlock({
  text,
  color,
  children,
}: {
  text: string;
  color?: "white";
  children: ReactNode;
}) {
  const [copied, setCopied] = useState(false);

  return (
    <div
      style={{
        position: "relative",
        minWidth: "16ch",
      }}
    >
      {children}
      <Button
        style={{
          position: "absolute",
          right: 0,
          bottom: 0,
          color: color,
          backgroundColor:
            color == "white"
              ? "rgba(0, 0, 0, 0.7)"
              : "rgba(255, 255, 255, 0.7)",
        }}
        type="text"
        icon={
          copied ? (
            <CheckOutlined style={{ color: color }} />
          ) : (
            <CopyOutlined style={{ color: color }} />
          )
        }
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
