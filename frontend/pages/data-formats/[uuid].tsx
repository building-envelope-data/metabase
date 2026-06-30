import { useRouter } from "next/router";
import DataFormat from "../../components/dataFormats/DataFormat";
import Layout from "../../components/Layout";
import { Skeleton } from "antd";

function Page() {
  const router = useRouter();
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
      <DataFormat dataFormatId={String(uuid)} />
    </Layout>
  );
}

export default Page;
