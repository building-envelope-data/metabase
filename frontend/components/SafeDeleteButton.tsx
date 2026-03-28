import { Button, Popconfirm, Tooltip } from "antd";
import { DeleteOutlined, SyncOutlined } from "@ant-design/icons";

const capitalize = <T extends string>(s: T) =>
  (s[0].toUpperCase() + s.slice(1)) as Capitalize<T>;

export default function SafeDeleteButton({
  kind,
  type = "text",
  deleting,
  onConfirm,
}: {
  kind: "delete" | "remove";
  type: "icon" | "text";
  deleting: boolean;
  onConfirm: (e?: React.MouseEvent<HTMLElement>) => void;
}) {
  const title = capitalize(kind);

  const button = (() => {
    switch (type) {
      case "icon":
        return (
          <Tooltip title={title}>
            <Button
              danger
              type="text"
              icon={deleting ? <SyncOutlined spin /> : <DeleteOutlined />}
              loading={deleting}
              shape="circle"
            />
          </Tooltip>
        );
      case "text":
        return (
          <Button
            danger
            type="primary"
            icon={<DeleteOutlined />}
            loading={deleting}
          >
            {title}
          </Button>
        );
    }
  })();

  return (
    <Popconfirm
      title={title}
      description="Are you sure?"
      okText="Yes"
      cancelText="No"
      okButtonProps={{ danger: true }}
      onConfirm={onConfirm}
    >
      {button}
    </Popconfirm>
  );
}
