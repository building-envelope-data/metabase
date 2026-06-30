import { useRouter } from "next/router";
import Layout from "../../../../../components/Layout";
import PhotovoltaicData from "../../../../../components/data/photovoltaic/PhotovoltaicData";
import { Skeleton } from "antd";

function Page() {
  const router = useRouter();
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
