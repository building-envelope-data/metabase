import { Button, Select, Tooltip, Spin, Flex } from "antd";
import {
  LeftOutlined,
  RightOutlined,
  LoadingOutlined,
} from "@ant-design/icons";

export const initialPageSize = 10;

export enum Fetching {
  INITIAL,
  NEXT,
  PREVIOUS,
}

const loadingIndicator = <Spin indicator={<LoadingOutlined spin />} />;

const pageSizeOptions = [3, initialPageSize, 20, 50, 100].map((size) => ({
  label: `${size} per page`,
  value: size,
}));

export interface PaginationProps {
  fetching: Fetching | null;
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
  fetching,
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
    <Flex justify="flex-end" align="center" gap="small">
      <Tooltip title="Previous">
        <Button
          onClick={onPrevious}
          disabled={fetching != null || !hasPrevious}
          type="text"
          icon={
            fetching == Fetching.PREVIOUS ? loadingIndicator : <LeftOutlined />
          }
        />
      </Tooltip>
      <span>
        Page {current} of{" "}
        {fetching == Fetching.INITIAL ? loadingIndicator : total}
      </span>
      <Tooltip title="Next">
        <Button
          onClick={onNext}
          disabled={fetching != null || !hasNext}
          type="text"
          icon={
            fetching == Fetching.NEXT ? loadingIndicator : <RightOutlined />
          }
        />
      </Tooltip>
      <Select
        options={pageSizeOptions}
        defaultValue={pageSize}
        onChange={onPageSizeChange}
        showSearch
        disabled={fetching != null}
        style={{ width: "max-content" }}
      />
    </Flex>
  );
}
