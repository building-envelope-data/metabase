import { useRouter } from "next/router";
import User from "../../components/users/User";
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
      <User userId={String(uuid)} />
    </Layout>
  );
}

export default Page;
