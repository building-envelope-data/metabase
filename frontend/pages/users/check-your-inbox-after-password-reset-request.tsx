import { useRouter } from "next/router";
import SingleSignOnLayout from "../../components/SingleSignOnLayout";
import { Card, Typography } from "antd";
import paths from "../../paths";
import Link from "next/link";

function CheckYourInboxAfterPasswordResetRequest() {
  const router = useRouter();
  const returnTo = router.query.returnTo;

  return (
    <SingleSignOnLayout>
      <Card title="Check your inbox!">
        <Typography.Paragraph>
          Confirm your password-reset request by following the confirmation link
          in your inbox and then{" "}
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
    </SingleSignOnLayout>
  );
}

export default CheckYourInboxAfterPasswordResetRequest;
