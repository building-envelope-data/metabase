import CopyableBlock from "./CopyableBlock";

export default function JsonViewer({ jsonData }: { jsonData: any }) {
  const jsonString = JSON.stringify(jsonData, null, 2);

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
