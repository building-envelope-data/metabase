import { FormInstance } from "antd";
import { UserError } from "../__generated__/graphql";
import { GraphQLFormattedError } from "graphql";

export function handleFormErrors(
	graphQlErrors: readonly GraphQLFormattedError[] | undefined | null,
	userErrors: UserError[] | undefined | null,
	form: FormInstance<any>,
) {
	const globalErrorMessages = new Array<string>();
	const transformedGraphQlErrors = graphQlErrors?.map((error) => ({
		message: error.message ?? "",
		// TODO Do not hardcode the fallback path `["input"]`
		path: error.path ?? ["input"],
	}));
	const errors = [...(transformedGraphQlErrors ?? []), ...(userErrors ?? [])];
	if (errors?.length) {
		globalErrorMessages.push("The form contains errors.");
	}
	const errorPathToMessage = errors.reduce((accumulator, error) => {
		// We use strings as keys instead of path arrays because the
		// latter are compared by reference.
		const pathAsString = error.path.join(".");
		if (!accumulator.has(pathAsString)) {
			accumulator.set(pathAsString, [error.path, []]);
		}
		accumulator.get(pathAsString)?.[1]?.push(error.message);
		return accumulator;
	}, new Map<string, [readonly (string | number)[], string[]]>());
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
	return globalErrorMessages;
}
