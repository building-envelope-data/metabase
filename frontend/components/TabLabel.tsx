import { Space, Badge, Typography } from "antd";

export default function TabLabel({
  name,
  count,
}: {
  name: React.ReactNode;
  count?: React.ReactNode;
}) {
  return count == 0 ? (
    <Space>
      <Typography.Text type="secondary">{name}</Typography.Text>
      <Badge showZero color="grey" count={count} />
    </Space>
  ) : (
    <Space>
      <Typography.Text>{name}</Typography.Text>
      {count != null && <Badge color="blue" count={count} />}
    </Space>
  );
}
