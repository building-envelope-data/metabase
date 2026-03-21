import { useEffect, useRef } from "react";
import { stringifyApolloError } from "../apollo";
import { App } from "antd";
import { ErrorLike } from "@apollo/client";

interface UseQueryHandlerProps {
	error?: ErrorLike | null;
}

export function useQueryHandler({ error }: UseQueryHandlerProps) {
	const { message } = App.useApp();
	const lastErrorRef = useRef<ErrorLike | null>(null);

	useEffect(() => {
		if (error && error !== lastErrorRef.current) {
			lastErrorRef.current = error;
			message.error(stringifyApolloError(error));
		}
	}, [error, message]);
}
