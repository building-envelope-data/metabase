import { useRouter } from "next/router";
import User from "../../components/users/User";
import Layout from "../../components/Layout";
import { Skeleton } from "antd";

function Page() {
  const router = useRouter();

  if (!router.isReady) {
    // Otherwise `uuid`, aka, `router.query`, is null on first render, see https://github.com/vercel/next.js/discussions/11484
    return null;
  }

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
