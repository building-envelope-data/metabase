import { Popconfirm } from "antd";
import DeleteButton from "./DeleteButton";
import { capitalize } from "../lib/string";
import { CSSProperties } from "react";

export default function SafeDeleteButton({
  kind = "delete",
  type = "primary",
  deleting = false,
  style,
  onConfirm,
  children,
}: {
  kind?: "delete" | "remove";
  type?: "primary" | "default" | "icon";
  deleting?: boolean;
  style?: CSSProperties;
  onConfirm: (e?: React.MouseEvent<HTMLElement>) => void;
  children?: React.ReactNode;
}) {
  const label = children ?? capitalize(kind);

  return (
    <Popconfirm
      title={label}
      description="Are you sure?"
      okText="Yes"
      cancelText="No"
      okButtonProps={{ danger: true }}
      onConfirm={onConfirm}
    >
      <DeleteButton kind={kind} type={type} deleting={deleting} style={style}>
        {children}
      </DeleteButton>
    </Popconfirm>
  );
}
