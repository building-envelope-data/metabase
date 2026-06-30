import { useRouter } from "next/router";
import Method from "../../components/methods/Method";
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
      <Method methodId={String(uuid)} />
    </Layout>
  );
}

export default Page;
