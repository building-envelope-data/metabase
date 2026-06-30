import { useRouter } from "next/router";
import Layout from "../../../components/Layout";
import OpenIdConnectApplication from "../../../components/openIdConnect/applications/OpenIdConnectApplication";
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
      <OpenIdConnectApplication applicationId={String(uuid)} />
    </Layout>
  );
}

export default Page;
