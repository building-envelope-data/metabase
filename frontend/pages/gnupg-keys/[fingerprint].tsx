import { useRouter } from "next/router";
import GnuPgKey from "../../components/gnuPgKeys/GnuPgKey";
import Layout from "../../components/Layout";
import { Skeleton } from "antd";

function Page() {
  const router = useRouter();
  const { fingerprint } = router.query;

  if (!fingerprint) {
    return (
      <Layout>
        <Skeleton active avatar title />
      </Layout>
    );
  }

  return (
    <Layout>
      <GnuPgKey fingerprint={String(fingerprint)} />
    </Layout>
  );
}

export default Page;
