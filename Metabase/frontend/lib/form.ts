import { GraphQLError } from 'graphql'
import { FormInstance } from 'antd'
import { Dispatch, SetStateAction } from 'react'

export function handleFormErrors(
    graphQlErrors: readonly GraphQLError[] | undefined,
    userErrors: { code: string, message: string, path: string[] }[] | undefined,
    setGlobalErrorMessages: Dispatch<SetStateAction<string[]>>,
    form: FormInstance<any>
) {
    const globalErrorMessages = new Array<string>()
    if (graphQlErrors) {
        // TODO Is this how we want to handle GraphQl errors?
        globalErrorMessages.push(
            ...graphQlErrors.map(e => e.message)
        )
    }
    if (userErrors) {
        const errorPathToMessage =
            userErrors.reduce(
                (a, x) => {
                    if (!a.has(x.path)) {
                        a.set(x.path, [])
                    }
                    a.get(x.path)?.push(x.message)
                    return a
                },
                new Map<string[], string[]>()
            )
        for (let [path, messages] of errorPathToMessage) {
            if (path.length === 1) {
                globalErrorMessages.push(...messages)
            }
            else {
                form.setFields([
                    {
                        name: path.slice(1),
                        errors: messages,
                    }
                ])
            }
        }
    }
    setGlobalErrorMessages(globalErrorMessages)
}
