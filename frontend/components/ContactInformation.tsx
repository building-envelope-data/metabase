import { Space, Typography } from "antd";
import {
  MailOutlined,
  PhoneOutlined,
  GlobalOutlined,
  EnvironmentOutlined,
} from "@ant-design/icons";
import { ContactInformationPartialFragment } from "../queries/common.generated";

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
