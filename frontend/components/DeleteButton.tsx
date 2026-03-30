import { Button, Tooltip } from "antd";
import { DeleteOutlined, SyncOutlined } from "@ant-design/icons";
import { capitalize } from "../lib/string";
import { CSSProperties, forwardRef } from "react";

interface DeleteButtonProps {
  title?: React.ReactNode;
  kind?: "delete" | "remove";
  type?: "primary" | "text" | "default" | "icon";
  deleting?: boolean;
  style?: CSSProperties;
  onClick?: (e?: React.MouseEvent<HTMLElement>) => void;
  // This allows Popconfirm to inject its internal event handlers
  [key: string]: any;
}

const DeleteButton = forwardRef<HTMLButtonElement, DeleteButtonProps>(
  (
    {
      title,
      kind = "delete",
      type = "primary",
      deleting = false,
      style,
      onClick,
      ...rest
    },
    ref,
  ) => {
    const theTitle = title ?? capitalize(kind);

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
          <Tooltip title={theTitle}>
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
            {theTitle}
          </Button>
        );
    }
  },
);

export default DeleteButton;
