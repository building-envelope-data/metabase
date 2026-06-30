import { useRouter } from "next/router";
import Layout from "../../../../../components/Layout";
import GeometricData from "../../../../../components/data/geometric/GeometricData";
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
      <GeometricData databaseId={String(uuid)} id={String(dataId)} />
    </Layout>
  );
}

export default Page;
