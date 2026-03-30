import { useRouter } from "next/router";
import Layout from "../../../../../components/Layout";
import CalorimetricData from "../../../../../components/data/calorimetric/CalorimetricData";

function Page() {
  const router = useRouter();

  if (!router.isReady) {
    // Otherwise `uuid`, aka, `router.query`, is null on first render, see https://github.com/vercel/next.js/discussions/11484
    return null;
  }

  const { uuid, dataId } = router.query;

  return (
    <Layout>
      <CalorimetricData databaseId={uuid} id={dataId} />
    </Layout>
  );
}

export default Page;
