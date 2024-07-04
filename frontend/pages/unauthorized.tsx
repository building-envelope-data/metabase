import { Result } from "antd";
import Layout from "../components/Layout";

function Page() {
  return (
    <Layout>
      <Result
        status="403"
        title="403"
        subTitle="Sorry, you are not authorized to access this page."
      />
    </Layout>
  );
}

export default Page;
