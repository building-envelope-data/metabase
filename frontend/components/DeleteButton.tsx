import { Button, Tooltip } from "antd";
import { DeleteOutlined, SyncOutlined } from "@ant-design/icons";
import { capitalize } from "../lib/string";
import { CSSProperties, forwardRef } from "react";

interface DeleteButtonProps {
  kind?: "delete" | "remove";
  type?: "primary" | "text" | "default" | "icon";
  deleting?: boolean;
  style?: CSSProperties;
  onClick?: (e?: React.MouseEvent<HTMLElement>) => void;
  children?: React.ReactNode;
  // This allows Popconfirm to inject its internal event handlers
  [key: string]: any;
}

const DeleteButton = forwardRef<HTMLButtonElement, DeleteButtonProps>(
  (
    {
      kind = "delete",
      type = "primary",
      deleting = false,
      style,
      onClick,
      children,
      ...rest
    },
    ref,
  ) => {
    const label = children ?? capitalize(kind);

    const commonProps = {
      ...rest, // contains Popconfirm's events
      ref, // allows Popconfirm to measure position
      danger: true,
      loading: deleting,
      style,
      onClick,
    };

    switch (type) {
      case "icon":
        return (
          <Tooltip title={label}>
            <Button
              {...commonProps}
              type="text"
              icon={deleting ? <SyncOutlined spin /> : <DeleteOutlined />}
              shape="circle"
            />
          </Tooltip>
        );
      default:
        return (
          <Button {...commonProps} type={type} icon={<DeleteOutlined />}>
            {label}
          </Button>
        );
    }
  },
);

export default DeleteButton;
