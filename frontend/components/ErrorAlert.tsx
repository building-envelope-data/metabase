import { Alert } from "antd";

export default function ErrorAlert({ messages }: { messages: string[] }) {
	return messages.length > 0 ? (
		<Alert type="error" message={messages.join(" ")} />
	) : null;
}
