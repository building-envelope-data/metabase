import { Route } from "next";
import { NextRouter } from "next/router";
import paths from "../paths";

export function redirectToLoginPage(router: NextRouter, returnTo: Route): void {
  router.push({
    pathname: paths.openIdConnectClientLogin,
    query: { returnTo: returnTo },
  });
}
