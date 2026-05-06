import { Space, Button, Select, Tooltip } from "antd";
import { LeftOutlined, RightOutlined } from "@ant-design/icons";

export const initialPageSize = 10;

const pageSizeOptions = [3, initialPageSize, 20, 50, 100].map((size) => ({
  label: `${size} per page`,
  value: size,
}));

export interface PaginationProps {
  current: number;
  total: number;
  pageSize: number;
  hasPrevious: boolean;
  hasNext: boolean;
  onPrevious: () => void;
  onNext: () => void;
  onPageSizeChange: (size: number) => void;
}

export default function Pagination({
  current,
  total,
  pageSize,
  onPrevious,
  hasPrevious,
  onNext,
  hasNext,
  onPageSizeChange,
}: PaginationProps) {
  return (
    <Space style={{ display: "flex", justifyContent: "flex-end" }}>
      <Tooltip title="Previous">
        <Button
          onClick={onPrevious}
          disabled={!hasPrevious}
          type="text"
          icon={<LeftOutlined />}
        />
      </Tooltip>
      <span>
        Page {current} of {total}
      </span>
      <Tooltip title="Next">
        <Button
          onClick={onNext}
          disabled={!hasNext}
          type="text"
          icon={<RightOutlined />}
        />
      </Tooltip>
      <Select
        options={pageSizeOptions}
        defaultValue={pageSize}
        onChange={onPageSizeChange}
        showSearch
        style={{ width: "max-content" }}
      />
    </Space>
  );
}
