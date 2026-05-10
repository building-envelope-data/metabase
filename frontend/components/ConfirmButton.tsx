import { CheckOutlined } from "@ant-design/icons";
import { Button, ButtonProps, Tooltip } from "antd";

interface ConfirmButtonProps extends Omit<
  ButtonProps,
  "type" | "icon" | "shape" | "children"
> {
  type?: "primary" | "default" | "icon";
}

export default function ConfirmButton({
  type = "default",
  ...rest
}: ConfirmButtonProps) {
  switch (type) {
    case "icon":
      return (
        <Tooltip title="Confirm">
          <Button
            type="text"
            icon={<CheckOutlined />}
            shape="circle"
            {...rest}
          />
        </Tooltip>
      );
    default:
      return (
        <Button type="primary" {...rest}>
          Confirm
        </Button>
      );
  }
}
