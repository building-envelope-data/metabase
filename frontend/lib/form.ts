import { FormInstance } from "antd";
import { Dispatch, SetStateAction } from "react";
import { CombinedGraphQLErrors, ErrorLike } from "@apollo/client";

export function handleFormErrors(
	apolloError: ErrorLike | undefined,
	userErrors:
		| { code: string; message: string; path: string[] }[]
		| undefined
		| null,
	setGlobalErrorMessages: Dispatch<SetStateAction<string[]>>,
	form: FormInstance<any>,
) {
	const globalErrorMessages = new Array<string>();
	if (apolloError || userErrors) {
		globalErrorMessages.push("The form contains errors.");
	}
	if (apolloError && !CombinedGraphQLErrors.is(apolloError)) {
		// TODO Is this how we want to handle GraphQl errors?
		globalErrorMessages.push(apolloError.message);
	}
	const apolloErrors = CombinedGraphQLErrors.is(apolloError)
		? apolloError.errors.map((e) => ({
				code: "UNKNOWN",
				message: e.message ?? "",
				path: e.path?.map((x) => String(x)) ?? ["input"],
			}))
		: [];
	const errors = [...apolloErrors, ...(userErrors ?? [])];
	const errorPathToMessage = errors.reduce((a, x) => {
		// We use strings as keys instead of path arrays because the
		// latter are compared by reference.
		const pathAsString = x.path.join(".");
		if (!a.has(pathAsString)) {
			a.set(pathAsString, [x.path, []]);
		}
		a.get(pathAsString)?.[1]?.push(x.message);
		return a;
	}, new Map<string, [string[], string[]]>());
	for (let [, [path, messages]] of errorPathToMessage) {
		if (path.length === 1) {
			globalErrorMessages.push(...messages);
		} else {
			form.setFields([
				{
					name: path.slice(1),
					errors: messages,
				},
			]);
		}
	}
	setGlobalErrorMessages(globalErrorMessages);
}
