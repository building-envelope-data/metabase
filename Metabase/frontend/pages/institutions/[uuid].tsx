import { useRouter } from "next/router";
import Institution from "../../components/institutions/Institution";
import Layout from "../../components/Layout";

function Show() {
  const router = useRouter();
  const { uuid } = router.query;

  if (!router.isReady) {
    // Otherwise `uuid`, aka, `router.query`, is null on first render, see https://github.com/vercel/next.js/discussions/11484
    return null;
  }

  return (
    <Layout>
      <Institution institutionId={uuid} />
    </Layout>
  );
}

export default Show;
