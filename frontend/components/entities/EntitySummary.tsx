import { Breadcrumb, Button, Flex, Space, Typography } from "antd";
import { ArrowLeftOutlined, PushpinOutlined } from "@ant-design/icons";
import { Scalars } from "../../__generated__/graphql";
import Copyable from "../Copyable";
import Id from "../Id";
import CopyButton from "../CopyButton";
import { Route } from "next";
import Link from "next/link";

interface Props {
  entity: {
    uuid: Scalars["Uuid"]["output"];
    name?: string | null;
    abbreviation?: string | null;
    description?: string | null;
  };
  route?: (id: Scalars["Uuid"]["output"]) => Route;
  tags?: React.ReactNode[];
  onBack?: () => void;
  extra?: React.ReactNode;
  breadcrumb?: { title: string; href?: string }[];
  children?: React.ReactNode;
}

export default function EntitySummary({
  entity,
  route,
  tags,
  onBack,
  extra,
  breadcrumb,
  children,
}: Props) {
  const absoluteRoute = route
    ? new URL(route(entity.uuid), new URL(window.location.href).origin)
    : null;

  return (
    <div>
      {breadcrumb && (
        <Breadcrumb
          items={breadcrumb.map((item) => ({
            title: item.title,
            href: item.href,
          }))}
          style={{ marginBottom: 12 }}
        />
      )}
      <Flex justify="space-between" align="flex-start">
        {onBack && (
          <Button
            type="text"
            icon={<ArrowLeftOutlined />}
            onClick={onBack}
            style={{ marginLeft: -12 }}
          />
        )}
        <div>
          {entity.uuid && (
            <div>
              <Copyable text={entity.uuid}>
                <Id value={entity.uuid} />
              </Copyable>
            </div>
          )}
          <Flex vertical gap={2}>
            <div style={{ position: "relative" }}>
              <div
                style={{
                  position: "absolute",
                  right: "100%",
                  marginRight: "3em",
                  top: "50%",
                  transform: "translateY(-50%)",
                }}
              >
                <CopyButton
                  getText={
                    () =>
                      absoluteRoute &&
                      absoluteRoute.pathname !==
                        new URL(window.location.href).pathname
                        ? absoluteRoute.toString() // use route if summary is not displayed on the entity page
                        : window.location.href // use URL (with URL parameters and all) if summary is displayed on the entity page
                  }
                  copyIcon={<PushpinOutlined />}
                >
                  Copy Permalink
                </CopyButton>
              </div>
              <Space wrap align="center">
                <Typography.Title level={4} style={{ margin: 0 }}>
                  {route ? (
                    <Link href={route(entity.uuid)}>
                      {entity.name ?? "Unnamed"}
                    </Link>
                  ) : (
                    (entity.name ?? "Unnamed")
                  )}
                  {entity.abbreviation && <span> ({entity.abbreviation})</span>}
                </Typography.Title>
                {tags && (
                  <div>
                    <Space>{tags}</Space>
                  </div>
                )}
              </Space>
            </div>
            {entity.description && (
              <Typography.Text type="secondary">
                {entity.description}
              </Typography.Text>
            )}
          </Flex>
        </div>
        <Space wrap>{extra}</Space>
      </Flex>
      {children && (
        <Flex vertical gap="small" style={{ marginTop: 16 }}>
          {children}
        </Flex>
      )}
    </div>
  );
}
