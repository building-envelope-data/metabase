import { Space } from "antd";
import { ReactNode } from "react";
import CopyButton from "./CopyButton";

export default function Copyable({
  text,
  onlyIcon,
  color,
  children,
}: {
  text: string;
  onlyIcon?: boolean;
  color?: "white";
  children?: ReactNode;
}) {
  return (
    <span>
      <Space>
        <span>{children == null ? text : children}</span>
        <CopyButton getText={() => text} onlyIcon={onlyIcon} color={color}>
          Copy
        </CopyButton>
      </Space>
    </span>
  );
}
