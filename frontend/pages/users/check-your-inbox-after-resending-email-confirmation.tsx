import { useRouter } from "next/router";
import { Card, Typography } from "antd";
import paths from "../../paths";
import Link from "next/link";
import SingleSignOnLayout from "../../components/SingleSignOnLayout";

export default function Page() {
  const router = useRouter();
  const { returnTo } = router.query;

  return (
    <SingleSignOnLayout>
      <Card title="Check your inbox!">
        <Typography.Paragraph style={{ maxWidth: "75ch" }}>
          Confirm your email address by following the confirmation link in your
          inbox and then{" "}
          <Link
            href={{
              pathname: paths.openIdConnectClientLogin,
              query: returnTo ? { returnTo: returnTo } : null,
            }}
          >
            Login
          </Link>
          .
        </Typography.Paragraph>
      </Card>
    </SingleSignOnLayout>
  );
}
