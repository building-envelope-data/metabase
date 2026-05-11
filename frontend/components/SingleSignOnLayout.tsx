import { ReactNode } from "react";
import Layout from "./Layout";
import { Flex } from "antd";

interface SingleSignOnLayoutProps {
  children?: ReactNode;
}

export default function SingleSignOnLayout({
  children,
}: SingleSignOnLayoutProps) {
  return (
    <Layout hideNav pageTitles={["Single-Sign On"]}>
      <Flex justify="center">{children}</Flex>
    </Layout>
  );
}
