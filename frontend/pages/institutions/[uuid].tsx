import { useRouter } from "next/router";
import Institution from "../../components/institutions/Institution";
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
      <Institution institutionId={String(uuid)} />
    </Layout>
  );
}

export default Page;
