import { Card } from "antd";
import React from "react";

export default function EntityItem({
  children,
}: {
  children?: React.ReactNode;
}) {
  return <Card variant="borderless">{children}</Card>;
}
