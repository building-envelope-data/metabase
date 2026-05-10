import { FormOutlined } from "@ant-design/icons";
import { Button, ButtonProps, Tooltip } from "antd";

interface NewButtonProps extends Omit<ButtonProps, "type" | "icon" | "shape"> {
  type?: "text" | "default" | "icon";
}

export default function NewButton({
  type = "default",
  onClick,
  children,
  ...rest
}: NewButtonProps) {
  switch (type) {
    case "icon":
      return (
        <Tooltip title="New">
          <Button
            type="text"
            icon={<FormOutlined />}
            shape="circle"
            {...rest}
          />
        </Tooltip>
      );
    default:
      return (
        <Button type="default" {...rest}>
          New {children}
        </Button>
      );
  }
}
