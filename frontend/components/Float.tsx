import Copyable from "./Copyable";
import { Scalars } from "../__generated__/graphql";
import { Tooltip } from "antd";

export default function Float({
  value,
}: {
  value: Scalars["Float"]["output"];
}) {
  return (
    <Tooltip
      title={
        <Copyable text={value.toString()} color="white">
          <code>{value}</code>
        </Copyable>
      }
      styles={{
        container: {
          whiteSpace: "nowrap",
          minWidth: "max-content",
          maxWidth: "none",
        },
      }}
    >
      <code>{value}</code>
    </Tooltip>
  );
}
