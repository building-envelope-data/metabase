import { useRouter } from "next/router";
import { useEffect } from "react";
import { useQuery } from "@apollo/client/react";
import { CurrentUserDocument } from "../../queries/currentUser.generated";
import { App } from "antd";
import { stringifyApolloError } from "../apollo";
import { redirectToLoginPage } from "../redirect";
import { Route } from "next";

interface UseRequireAuthProps {
	returnTo: Route;
}

export function useRequireAuth({ returnTo }: UseRequireAuthProps) {
	const router = useRouter();

	const { loading, data, error } = useQuery(CurrentUserDocument);
	const currentUser = data?.currentUser;
	const shouldRedirect = !(loading || error || currentUser);
	const { message } = App.useApp();

	useEffect(() => {
		if (error) {
			message.error(stringifyApolloError(error));
		}
	}, [error, message]);

	useEffect(() => {
		if (router.isReady && shouldRedirect) {
			redirectToLoginPage(router, returnTo);
		}
	}, [router, shouldRedirect, returnTo]);

	return { currentUser };
}
