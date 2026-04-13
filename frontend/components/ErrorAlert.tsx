import { Alert } from "antd";

export default function ErrorAlert({ messages }: { messages: string[] }) {
  return messages.length > 0 ? (
    <Alert
      type="error"
      title={messages.length == 1 ? "Error" : "Errors"}
      description={
        messages.length == 1 ? (
          messages[0]
        ) : (
          <ul style={{ margin: 0, paddingLeft: "18px" }}>
            {messages.map((message, index) => (
              <li key={index}>{message}</li>
            ))}
          </ul>
        )
      }
    />
  ) : null;
}
