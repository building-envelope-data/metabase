import { Space, Typography } from "antd";
import {
  MailOutlined,
  PhoneOutlined,
  GlobalOutlined,
  EnvironmentOutlined,
} from "@ant-design/icons";
import { ContactInformationPartialFragment } from "../queries/common.generated";

export const phoneNumberFormInput = {
  placeholder: "[+][Country Code][Local Area Code][Local Phone Number]",
  extra: (
    <span>
      in the format{" "}
      <code>[+][Country Code][Local Area Code][Local Phone Number]</code> as
      standardized in{" "}
      <Typography.Link
        href="https://www.itu.int/rec/t-rec-e.164/en"
        target="_blank"
      >
        E.164
      </Typography.Link>
      . For example, <code>(510) 486-4000</code> in the standard local format of
      the United States becomes <code>+15104864000</code> in E.164 format,{" "}
      <code>02 313 99 44</code> in Belgium becomes <code>+3223139944</code>, and{" "}
      <code>0761 4588-0</code> in Germany becomes <code>+4976145880</code>. See
      also{" "}
      <Typography.Link href="https://countrycode.org" target="_blank">
        Country Codes
      </Typography.Link>{" "}
      and{" "}
      <Typography.Link
        href="https://help.twilio.com/articles/223183008-Formatting-International-Phone-Numbers"
        target="_blank"
      >
        Formatting International Phone Numbers
      </Typography.Link>
      .
    </span>
  ),
};

export default function ContactInformation({
  contact,
}: {
  contact: ContactInformationPartialFragment | null;
}) {
  const hasContact =
    contact?.emailAddress ||
    contact?.phoneNumber ||
    contact?.websiteLocator ||
    contact?.postalAddress;

  if (!hasContact) {
    return <></>;
    // return <Empty description="No contact information available" />;
  }

  return (
    <Space>
      {contact?.emailAddress && (
        <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
          <MailOutlined style={{ fontSize: 16, color: "#1890ff" }} />
          <Typography.Link href={`mailto:${contact.emailAddress}`}>
            {contact.emailAddress}
          </Typography.Link>
        </div>
      )}
      {contact?.phoneNumber && (
        <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
          <PhoneOutlined style={{ fontSize: 16, color: "#1890ff" }} />
          <Typography.Link href={`tel:${contact.phoneNumber}`}>
            {contact.phoneNumber}
          </Typography.Link>
        </div>
      )}
      {contact?.websiteLocator && (
        <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
          <GlobalOutlined style={{ fontSize: 16, color: "#1890ff" }} />
          <Typography.Link href={contact.websiteLocator} target="_blank">
            {contact.websiteLocator}
          </Typography.Link>
        </div>
      )}
      {contact?.postalAddress && (
        <div style={{ display: "flex", alignItems: "start", gap: 8 }}>
          <EnvironmentOutlined
            style={{ fontSize: 16, color: "#1890ff", marginTop: 2 }}
          />
          <span>{contact.postalAddress}</span>
        </div>
      )}
    </Space>
  );
}
