import { Badge, Tooltip } from "antd";

export default function OpticalIdConnectTokenRibbon({
  type,
  children,
}: {
  type?: string | null;
  children: React.ReactNode;
}) {
  return type == null ? (
    children
  ) : (
    <Badge.Ribbon
      placement="end"
      text={<Tooltip title={type}>{type.split(":").pop()}</Tooltip>}
      style={{ textTransform: "lowercase", fontWeight: "normal" }}
    >
      {children}
    </Badge.Ribbon>
  );
}
