import { useRouter } from "next/router";
import DataFormat from "../../components/dataFormats/DataFormat";
import Layout from "../../components/Layout";

function Show() {
  const router = useRouter();

  if (!router.isReady) {
    // Otherwise `uuid`, aka, `router.query`, is null on first render, see https://github.com/vercel/next.js/discussions/11484
    return null;
  }

  const { uuid } = router.query;

  return (
    <Layout>
      <DataFormat dataFormatId={uuid} />
    </Layout>
  );
}

export default Show;
