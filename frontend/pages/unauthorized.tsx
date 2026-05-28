import { Result } from "antd";
import Layout from "../components/Layout";
import Link from "next/link";
import paths from "../paths";

export default function Page() {
  return (
    <Layout>
      <Result
        status="403"
        title="403"
        subTitle="Sorry, you are not authorized to access this page."
        extra={
          <Link
            href={{
              pathname: paths.openIdConnectClientLogin,
              query: { returnTo: window.location.pathname },
            }}
          >
            Login
          </Link>
        }
      />
    </Layout>
  );
}
