import { useRouter } from "next/router";
import User from "../../components/users/User";
import Layout from "../../components/Layout";

function Page() {
  const router = useRouter();

  if (!router.isReady) {
    // Otherwise `uuid`, aka, `router.query`, is null on first render, see https://github.com/vercel/next.js/discussions/11484
    return null;
  }

  const { uuid } = router.query;

  return (
    <Layout>
      <User userId={uuid} />
    </Layout>
  );
}

export default Page;
