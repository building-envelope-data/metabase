import Copyable from "./Copyable";
import CopyableBlock from "./CopyableBlock";

export default function CodeView({
  code,
  inline = false,
}: {
  code: string;
  inline?: boolean;
}) {
  if (inline) {
    return (
      <Copyable onlyIcon text={code}>
        <code>{code}</code>
      </Copyable>
    );
  } else {
    return (
      <CopyableBlock text={code}>
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
