import { Popconfirm } from "antd";
import DeleteButton from "./DeleteButton";
import { capitalize } from "../lib/string";
import { CSSProperties } from "react";

export default function SafeDeleteButton({
  title,
  kind = "delete",
  type = "primary",
  deleting = false,
  style,
  onConfirm,
}: {
  title?: React.ReactNode;
  kind?: "delete" | "remove";
  type?: "primary" | "icon";
  deleting?: boolean;
  style?: CSSProperties;
  onConfirm: (e?: React.MouseEvent<HTMLElement>) => void;
}) {
  const theTitle = title ?? capitalize(kind);

  return (
    <Popconfirm
      title={theTitle}
      description="Are you sure?"
      okText="Yes"
      cancelText="No"
      okButtonProps={{ danger: true }}
      onConfirm={onConfirm}
    >
      <DeleteButton
        title={theTitle}
        kind={kind}
        type={type}
        deleting={deleting}
        style={style}
      />
    </Popconfirm>
  );
}
