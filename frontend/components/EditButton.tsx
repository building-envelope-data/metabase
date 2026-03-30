import { EditOutlined } from "@ant-design/icons";
import { Button, Tooltip } from "antd";

export default function EditButton({
  type = "default",
  onClick,
}: {
  type?: "text" | "default" | "icon";
  onClick?: (e?: React.MouseEvent<HTMLElement>) => void;
}) {
  switch (type) {
    case "icon":
      return (
        <Tooltip title="Edit">
          <Button
            type="text"
            icon={<EditOutlined />}
            shape="circle"
            onClick={onClick}
          />
        </Tooltip>
      );
    default:
      return (
        <Button type="default" onClick={onClick}>
          Edit
        </Button>
      );
  }
}
