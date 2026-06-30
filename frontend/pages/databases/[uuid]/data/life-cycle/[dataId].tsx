import { useRouter } from "next/router";
import Layout from "../../../../../components/Layout";
import LifeCycleData from "../../../../../components/data/lifeCycle/LifeCycleData";
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
      <LifeCycleData databaseId={String(uuid)} id={String(dataId)} />
    </Layout>
  );
}

export default Page;
