import { EditOutlined } from "@ant-design/icons";
import { Button, ButtonProps, Tooltip } from "antd";

interface EditButtonProps extends Omit<
  ButtonProps,
  "type" | "icon" | "shape" | "children"
> {
  type?: "text" | "default" | "icon";
}

export default function EditButton({
  type = "default",
  ...rest
}: EditButtonProps) {
  switch (type) {
    case "icon":
      return (
        <Tooltip title="Edit">
          <Button
            type="text"
            icon={<EditOutlined />}
            shape="circle"
            {...rest}
          />
        </Tooltip>
      );
    default:
      return (
        <Button type="default" {...rest}>
          Edit
        </Button>
      );
  }
}
