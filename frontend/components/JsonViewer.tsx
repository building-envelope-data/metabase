import { Tooltip } from "antd";
import Copyable from "./Copyable";
import CopyableBlock from "./CopyableBlock";

export default function JsonViewer({
  data,
  inline = false,
}: {
  data: object;
  inline?: boolean;
}) {
  if (inline) {
    const jsonString = JSON.stringify(data);
    return (
      <Copyable onlyIcon text={jsonString}>
        <Tooltip title={<JsonViewer data={data} />}>
          <code>{jsonString}</code>
        </Tooltip>
      </Copyable>
    );
  } else {
    const jsonString = JSON.stringify(data, null, 2);
    return (
      <CopyableBlock text={jsonString}>
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
