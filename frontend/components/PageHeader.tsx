import { Breadcrumb, Button, Space, Typography } from "antd";
import { ArrowLeftOutlined } from "@ant-design/icons";
import { Scalars } from "../__generated__/graphql";
import Copyable from "./Copyable";
import Id from "./Id";

const { Title, Text } = Typography;

interface Props {
  id?: Scalars["Uuid"]["output"];
  title: string;
  subTitle?: string;
  tags?: React.ReactNode[];
  onBack?: () => void;
  extra?: React.ReactNode;
  breadcrumb?: { title: string; href?: string }[];
  children?: React.ReactNode;
}

export default function PageHeader({
  id,
  title,
  subTitle,
  tags,
  onBack,
  extra,
  breadcrumb,
  children,
}: Props) {
  return (
    <>
      {breadcrumb && (
        <Breadcrumb
          items={breadcrumb.map((item) => ({
            title: item.title,
            href: item.href,
          }))}
          style={{ marginBottom: 12 }}
        />
      )}

      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "flex-start",
        }}
      >
        {onBack && (
          <Button
            type="text"
            icon={<ArrowLeftOutlined />}
            onClick={onBack}
            style={{ marginLeft: -12 }}
          />
        )}
        <div>
          {id && (
            <div>
              <Copyable text={id}>
                <Id value={id} />
              </Copyable>
            </div>
          )}
          <Space>
            <Title level={4} style={{ margin: 0 }}>
              {title}
            </Title>
            {tags && <Space>{tags}</Space>}
          </Space>
          {subTitle && (
            <Text type="secondary" style={{ display: "block", marginTop: 4 }}>
              {subTitle}
            </Text>
          )}
        </div>
        <Space wrap>{extra}</Space>
      </div>

      {children && <div style={{ marginTop: 16 }}>{children}</div>}
    </>
  );
}
