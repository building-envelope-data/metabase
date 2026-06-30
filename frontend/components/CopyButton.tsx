import { Button, Tooltip } from "antd";
import { CopyOutlined, CheckOutlined } from "@ant-design/icons";
import { ReactNode, useState } from "react";

export default function CopyButton({
  getText,
  type = "text",
  size = "small",
  copyIcon = <CopyOutlined />,
  onlyIcon = false,
  color = undefined,
  children,
}: {
  getText: () => string;
  type?: "text" | "default";
  size?: "small" | "medium" | "large";
  copyIcon?: ReactNode;
  onlyIcon?: boolean;
  color?: "white";
  children?: ReactNode;
}) {
  const [copied, setCopied] = useState(false);

  return onlyIcon ? (
    <Tooltip title="Copy">
      <Button
        type={type}
        size={size}
        icon={
          copied ? (
            <CheckOutlined style={{ color: color }} />
          ) : (
            <span style={{ color: color }}>{copyIcon}</span>
          )
        }
        onClick={() => {
          navigator.clipboard.writeText(getText());
          setCopied(true);
          setTimeout(() => setCopied(false), 2000);
        }}
      />
    </Tooltip>
  ) : (
    <Button
      type={type}
      size={size}
      icon={
        copied ? (
          <CheckOutlined style={{ color: color }} />
        ) : (
          <span style={{ color: color }}>{copyIcon}</span>
        )
      }
      color="danger"
      onClick={() => {
        navigator.clipboard.writeText(getText());
        setCopied(true);
        setTimeout(() => setCopied(false), 2000);
      }}
    >
      <div style={{ display: "inline-grid", gridTemplateAreas: '"stack"' }}>
        <span
          style={{
            gridArea: "stack",
            visibility: copied ? "hidden" : "visible",
            whiteSpace: "nowrap",
            color: color,
          }}
        >
          {children}
        </span>
        <span
          style={{
            gridArea: "stack",
            visibility: copied ? "visible" : "hidden",
            whiteSpace: "nowrap",
            color: color,
          }}
        >
          Done
        </span>
      </div>
    </Button>
  );
}
