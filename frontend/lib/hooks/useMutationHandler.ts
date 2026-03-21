import { ApolloClient, ErrorLike } from "@apollo/client";
import { Form } from "antd";
import { useState } from "react";
import { handleFormErrors } from "../form";

type Errors = { code: any; message: string; path: string[] }[];

interface UseMutationHandlerProps<
	TMutation,
	TPayloadKey extends keyof TMutation,
> {
	payloadKey: TPayloadKey;
	getErrors: (payload: TMutation[TPayloadKey]) => Errors | null;
	onSuccess?: (model: TMutation[TPayloadKey] | null) => void | Promise<any>;
	onError?: (error: Error | null, userErrors: Errors | null) => void;
}

// type KeysWithErrors<T> = {
// 	[K in keyof T]: T[K] extends { errors: Errors } ? K : never;
// }[keyof T];

// export type KeysWithErrors<T> = {
// 	[K in keyof T]: NonNullable<T[K]> extends { errors: Errors } ? K : never;
// }[keyof T];

export function useMutationHandler<
	TMutation,
	TPayloadKey extends keyof TMutation,
	TFormValues,
>({
	payloadKey,
	getErrors,
	onSuccess,
	onError,
}: UseMutationHandlerProps<TMutation, TPayloadKey>) {
	const [globalErrorMessages, setGlobalErrorMessages] = useState<string[]>([]);
	const [form] = Form.useForm<TFormValues>();
	const [loading, setLoading] = useState(false);

	const handleMutationResult = async (
		error: ErrorLike | null,
		payload: TMutation[TPayloadKey] | null,
		userErrors: Errors | null,
	) => {
		if (!error && !userErrors?.length) {
			await onSuccess?.(payload);
		} else {
			handleFormErrors(
				error ?? undefined,
				userErrors?.map((e) => ({ ...e, code: String(e.code) })) ?? null,
				setGlobalErrorMessages,
				form,
			);
			onError?.(error, userErrors);
		}
	};

	const withMutationHandler = async (
		mutationFunction: () => Promise<ApolloClient.MutateResult<TMutation>>,
	) => {
		try {
			setLoading(true);
			const result = await mutationFunction();
			handleMutationResult(
				result.error ?? null,
				result.data ? result.data[payloadKey] : null,
				result.data ? getErrors(result.data[payloadKey]) : null,
			);
			return result;
		} catch (error) {
			setGlobalErrorMessages([
				error instanceof Error ? error.message : "An unexpected error occurred",
			]);
		} finally {
			setLoading(false);
		}
	};

	return {
		globalErrorMessages,
		setGlobalErrorMessages,
		form,
		loading,
		withMutationHandler,
	};
}
