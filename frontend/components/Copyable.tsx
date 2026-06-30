import { Space } from "antd";
import { CSSProperties, ReactNode } from "react";
import CopyButton from "./CopyButton";

export default function Copyable({
  text,
  onlyIcon,
  color,
  style,
  children,
}: {
  text: string;
  onlyIcon?: boolean;
  color?: "white";
  style?: CSSProperties;
  children?: ReactNode;
}) {
  const content = (
    <>
      <span>{children == null ? text : children}</span>
      <CopyButton getText={() => text} onlyIcon={onlyIcon} color={color}>
        Copy
      </CopyButton>
    </>
  );

  return (
    <span style={style}>{onlyIcon ? content : <Space>{content}</Space>}</span>
  );
}
