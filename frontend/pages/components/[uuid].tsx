import { useRouter } from "next/router";
import Component from "../../components/components/Component";
import Layout from "../../components/Layout";
import { Skeleton } from "antd";

function Page() {
  const router = useRouter();
  const { uuid } = router.query;

  if (!uuid) {
    return (
      <Layout>
        <Skeleton active avatar title />
      </Layout>
    );
  }

  return (
    <Layout>
      <Component componentId={String(uuid)} />
    </Layout>
  );
}

export default Page;
