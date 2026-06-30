import { Space } from "antd";

export const Iconize = ({
  icon,
  children,
}: {
  icon: React.ReactNode;
  children: React.ReactNode;
}) => {
  return (
    <Space size={4}>
      {icon}
      {children}
    </Space>
  );
};
