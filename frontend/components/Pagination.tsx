import { Button, Select, Tooltip, Spin, Flex } from "antd";
import {
  LeftOutlined,
  RightOutlined,
  LoadingOutlined,
  ReloadOutlined,
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
  onReload: () => void;
  onPrevious: () => void;
  onNext: () => void;
  onPageSizeChange: (size: number) => void;
}

export default function Pagination({
  fetching,
  current,
  total,
  pageSize,
  hasPrevious,
  hasNext,
  onReload,
  onPrevious,
  onNext,
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
            fetching === Fetching.PREVIOUS ? loadingIndicator : <LeftOutlined />
          }
        />
      </Tooltip>
      <span>
        Page {current} of {fetching === Fetching.INITIAL ? "∞" : total}
      </span>
      <Tooltip title="Next">
        <Button
          onClick={onNext}
          disabled={fetching != null || !hasNext}
          type="text"
          icon={
            fetching === Fetching.NEXT ? loadingIndicator : <RightOutlined />
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
      <Tooltip title="Reload">
        <Button
          onClick={onReload}
          disabled={fetching != null}
          type="text"
          icon={
            fetching === Fetching.INITIAL ? (
              loadingIndicator
            ) : (
              <ReloadOutlined />
            )
          }
        />
      </Tooltip>
    </Flex>
  );
}
