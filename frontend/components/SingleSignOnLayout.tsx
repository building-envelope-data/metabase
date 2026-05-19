import { ReactNode } from "react";
import Layout from "./Layout";
import { Flex } from "antd";
import { useRouter } from "next/router";

interface SingleSignOnLayoutProps {
  children?: ReactNode;
}

export default function SingleSignOnLayout({
  children,
}: SingleSignOnLayoutProps) {
  const router = useRouter();
  const returnTo = router.query.returnTo?.toString();
  const clientIdMatch = returnTo?.match(/[?&]client_id=([^&]+)/);
  const clientId = clientIdMatch?.[1];

  return (
    <Layout
      onlyUserOrLoginItems={clientId != "metabase"}
      pageTitles={["Single-Sign On"]}
    >
      <Flex justify="center">{children}</Flex>
    </Layout>
  );
}
