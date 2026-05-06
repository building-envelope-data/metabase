import { Badge } from "antd";
import {
  OpticalComponentSubtype,
  OpticalComponentType,
} from "../../../__generated__/graphql";

export default function OpticalDataRibbon({
  type,
  subtype,
  children,
}: {
  type?: OpticalComponentType | null;
  subtype?: OpticalComponentSubtype | null;
  children: React.ReactNode;
}) {
  return type == null && subtype == null ? (
    children
  ) : (
    <Badge.Ribbon
      text={`${type ?? "unknown"}${subtype && ` (${subtype})`}`}
      style={{ textTransform: "lowercase", fontWeight: "normal" }}
    >
      {children}
    </Badge.Ribbon>
  );
}
