import { useRouter } from "next/router";
import Layout from "../../components/Layout";
import { Card, Typography } from "antd";
import paths from "../../paths";
import Link from "next/link";

function CheckYourInboxAfterRegistration() {
  const router = useRouter();
  const returnTo = router.query.returnTo;

  return (
    <Layout>
      <Card title="Check your inbox!">
        <Typography.Paragraph>
          Confirm your email address by following the confirmation link in your
          inbox and then{" "}
          <Link
            href={{
              pathname: paths.userLogin,
              query: returnTo ? { returnTo: returnTo } : null,
            }}
          >
            Login
          </Link>
          .
        </Typography.Paragraph>
      </Card>
    </Layout>
  );
}

export default CheckYourInboxAfterRegistration;
