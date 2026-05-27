import { useRouter } from "next/router";
import Layout from "../../../../../components/Layout";
import PhotovoltaicData from "../../../../../components/data/photovoltaic/PhotovoltaicData";
import { Skeleton } from "antd";

function Page() {
  const router = useRouter();

  if (!router.isReady) {
    // Otherwise `uuid`, aka, `router.query`, is null on first render, see https://github.com/vercel/next.js/discussions/11484
    return null;
  }

  const { uuid, dataId } = router.query;

  if (!uuid || !dataId) {
    return (
      <Layout>
        <Skeleton active avatar title />
      </Layout>
    );
  }

  return (
    <Layout>
      <PhotovoltaicData databaseId={String(uuid)} id={String(dataId)} />
    </Layout>
  );
}

export default Page;
