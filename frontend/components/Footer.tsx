import { Space, Typography } from "antd";
import paths from "../paths";

export default function Footer() {
  return (
    <Space size="large">
      <Typography.Link href={paths.legalNotice}>Legal Notice</Typography.Link>
      <Typography.Link href={paths.dataProtectionInformation}>
        Data Protection Information
      </Typography.Link>
    </Space>
  );
}
