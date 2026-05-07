import { FormOutlined } from "@ant-design/icons";
import { Button, Tooltip } from "antd";

export default function NewButton({
  type = "default",
  onClick,
  children,
}: {
  type?: "text" | "default" | "icon";
  onClick?: (e?: React.MouseEvent<HTMLElement>) => void;
  children?: React.ReactNode;
}) {
  switch (type) {
    case "icon":
      return (
        <Tooltip title="New">
          <Button
            type="text"
            icon={<FormOutlined />}
            shape="circle"
            onClick={onClick}
          />
        </Tooltip>
      );
    default:
      return (
        <Button type="default" onClick={onClick}>
          New {children}
        </Button>
      );
  }
}
