import { ApolloClient, ErrorLike } from "@apollo/client";
import { Form } from "antd";
import { useState } from "react";
import { handleFormErrors } from "../form";

interface UseMutationHandlerOptions<TModel> {
	onSuccess?: (model: TModel | null | undefined) => void | Promise<boolean>;
	onError?: (error: Error) => void;
}

type Errors = { code: any; message: string; path: string[] }[];

// type KeysWithErrors<T> = {
// 	[K in keyof T]: T[K] extends { errors: Errors } ? K : never;
// }[keyof T];

// export type KeysWithErrors<T> = {
// 	[K in keyof T]: NonNullable<T[K]> extends { errors: Errors } ? K : never;
// }[keyof T];

export function useMutationHandler<
	TMutation,
	TPayloadKey extends keyof TMutation,
	TModelKey extends keyof TMutation[TPayloadKey],
	TFormValues,
>(options?: UseMutationHandlerOptions<TMutation[TPayloadKey][TModelKey]>) {
	const [globalErrorMessages, setGlobalErrorMessages] = useState<string[]>([]);
	const [form] = Form.useForm<TFormValues>();
	const [loading, setLoading] = useState(false);

	const handleMutationResult = async (
		error: ErrorLike | undefined,
		model: TMutation[TPayloadKey][TModelKey] | null,
		userErrors: Errors | null,
	) => {
		handleFormErrors(
			error,
			userErrors?.map((e) => ({ ...e, code: String(e.code) })) ?? null,
			setGlobalErrorMessages,
			form,
		);
		if (!error && !userErrors?.length) {
			await options?.onSuccess?.(model);
		} else {
			error ??= new Error("Mutation failed");
			console.error(error, userErrors);
			setGlobalErrorMessages([error.message]);
			options?.onError?.(error);
		}
	};

	const withMutationHandler = async (
		mutationFunction: () => Promise<ApolloClient.MutateResult<TMutation>>,
		payloadKey: TPayloadKey,
		modelKey: TModelKey,
		getErrors: (payload: TMutation[TPayloadKey]) => Errors | null,
	) => {
		try {
			setLoading(true);
			const result = await mutationFunction();
			handleMutationResult(
				result.error,
				result.data ? result.data[payloadKey][modelKey] : null,
				result.data ? getErrors(result.data[payloadKey]) : null,
			);
			return result;
		} catch (error) {
			console.error("Mutation failed:", error);
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
