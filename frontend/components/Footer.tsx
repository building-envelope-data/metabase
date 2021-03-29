import { Typography } from "antd";
import * as React from "react";
import paths from "../paths";

export type FooterProps = {};

export const Footer: React.FunctionComponent<FooterProps> = () => (
  <>
    <Typography.Link href={paths.legalNotice}>Legal Notice</Typography.Link>
    <Typography.Link href={paths.dataProtectionInformation}>
      Data Protection Information
    </Typography.Link>
  </>
);

export default Footer;
