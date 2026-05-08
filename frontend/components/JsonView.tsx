import { Tooltip } from "antd";
import CopyableBlock from "./CopyableBlock";

export default function JsonView({
  data,
  inline = false,
  color,
  autoSize = true,
}: {
  data: object;
  inline?: boolean;
  color?: "white";
  autoSize?: boolean;
}) {
  if (inline) {
    const jsonString = JSON.stringify(data);
    return (
      <Tooltip title={<JsonView autoSize={false} data={data} color="white" />}>
        <code>{jsonString}</code>
      </Tooltip>
    );
  } else {
    const jsonString = JSON.stringify(data, null, 2);
    return (
      <CopyableBlock autoSize={autoSize} text={jsonString} color={color}>
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
