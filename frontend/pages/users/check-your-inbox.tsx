import Layout from "../../components/Layout";
import { Card, Typography } from "antd";

function CheckYourInbox() {
  return (
    <Layout>
      <Card title="Check your inbox!">
        <Typography.Paragraph>
          Confirm your email address by following the confirmation link in your
          inbox.
        </Typography.Paragraph>
      </Card>
    </Layout>
  );
}

export default CheckYourInbox;
