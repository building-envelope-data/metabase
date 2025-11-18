import { Typography } from "antd";
import paths from "../paths";

export type FooterProps = {};

export default function Footer() {
  return (
    <>
      <Typography.Link href={paths.legalNotice}>Legal Notice</Typography.Link>{" "}
      &middot;{" "}
      <Typography.Link href={paths.dataProtectionInformation}>
        Data Protection Information
      </Typography.Link>
    </>
  );
}
