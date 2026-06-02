import { ReactNode } from "react";
import Layout from "./Layout";
import { Button, Flex, Skeleton } from "antd";
import { useRouter } from "next/router";
import { isLocalUrl } from "../lib/url";
import paths from "../paths";
import { extractAntiforgeryTokenFromCookie } from "../lib/apollo";

interface SingleSignOnLayoutProps {
  children?: ReactNode;
}

export default function SingleSignOnLayout({
  children,
}: SingleSignOnLayoutProps) {
  const router = useRouter();
  const { returnTo } = router.query;
  // const clientIdMatch = returnTo?.toString()?.match(/[?&]client_id=([^&]+)/);
  // const clientId = clientIdMatch?.[1];

  if (!router.isReady) {
    return (
      <Layout>
        <Skeleton active avatar title />
      </Layout>
    );
  }

  const navItems =
    returnTo && !isLocalUrl(String(returnTo))
      ? [
          {
            key: "deny",
            label: (
              <form action={paths.openIdConnectAuthorize} method="post">
                <input
                  name="__RequestVerificationToken"
                  type="hidden"
                  value={
                    typeof window !== "undefined"
                      ? (extractAntiforgeryTokenFromCookie() ?? "")
                      : ""
                  }
                />
                <input name="submit.Deny" type="hidden" value="No" />
                <Button type="default" htmlType="submit">
                  Abort
                </Button>
              </form>
            ),
          },
        ]
      : undefined;

  return (
    <Layout items={navItems} pageTitles={["Single-Sign On"]}>
      <Flex justify="center">{children}</Flex>
    </Layout>
  );
}
