import { Space, Badge, Typography } from "antd";

const { Text } = Typography;

export default function TabLabel({
  name,
  count,
}: {
  name: String;
  count?: React.ReactNode;
}) {
  return count == 0 ? (
    <Space>
      <Text type="secondary">{name}</Text>
      <Badge showZero color="grey" count={count} />
    </Space>
  ) : (
    <Space>
      <Text>{name}</Text>
      {count != null && <Badge color="blue" count={count} />}
    </Space>
  );
}
