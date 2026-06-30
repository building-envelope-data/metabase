import { CSSProperties } from "react";
import Copyable from "./Copyable";
import CopyableBlock from "./CopyableBlock";

export default function CodeView({
  code,
  inline = false,
  style,
}: {
  code: string;
  inline?: boolean;
  style?: CSSProperties;
}) {
  if (inline) {
    return (
      <Copyable onlyIcon text={code} style={style}>
        <code>{code}</code>
      </Copyable>
    );
  } else {
    return (
      <CopyableBlock text={code} style={style}>
        <pre
          style={{
            overflow: "auto",
          }}
        >
          <code style={{ whiteSpace: "pre-wrap", wordWrap: "break-word" }}>
            {code}
          </code>
        </pre>
      </CopyableBlock>
    );
  }
}
