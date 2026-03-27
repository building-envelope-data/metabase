export default function JsonViewer({ jsonData }: { jsonData: any }) {
  return (
    <pre
      style={{
        overflow: "auto",
      }}
    >
      <code style={{ whiteSpace: "pre-wrap", wordWrap: "break-word" }}>
        {JSON.stringify(jsonData, null, 2)}
      </code>
    </pre>
  );
}
