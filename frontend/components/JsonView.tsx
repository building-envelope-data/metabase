import { Tooltip } from "antd";
import CopyableBlock from "./CopyableBlock";
import { CSSProperties } from "react";

export default function JsonView({
  data,
  inline = false,
  color,
  style,
}: {
  data: object;
  inline?: boolean;
  color?: "white";
  style?: CSSProperties;
}) {
  if (inline) {
    const jsonString = JSON.stringify(data);
    return (
      <Tooltip title={<JsonView data={data} color="white" />}>
        <code style={style}>{jsonString}</code>
      </Tooltip>
    );
  } else {
    const jsonString = JSON.stringify(data, null, 2);
    return (
      <CopyableBlock text={jsonString} color={color} style={style}>
        <pre
          style={{
            overflow: "auto",
          }}
        >
          <code style={{ whiteSpace: "pre-wrap", wordWrap: "break-word" }}>
            {jsonString}
          </code>
        </pre>
      </CopyableBlock>
    );
  }
}
