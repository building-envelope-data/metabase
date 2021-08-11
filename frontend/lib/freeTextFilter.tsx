import { Input, Space, Button, Typography } from "antd";
import { FilterFilled } from "@ant-design/icons";
import Highlighter from "react-highlight-words";
import {
  FilterConfirmProps,
  FilterDropdownProps,
} from "antd/lib/table/interface";
import { Key } from "react";

export function resolvePath(path: string | string[], object: any) {
  const properties = Array.isArray(path) ? path : [path];
  return properties.reduce(
    (previous, current) => previous && previous[current],
    object
  );
}

export function getFreeTextFilterProps<RecordType extends object = any>(
  dataIndex: string | string[],
  onFilterTextChange: (
    dataIndex: string | string[],
    newFilterText: string
  ) => void
) {
  const filter = (
    selectedKeys: Key[],
    confirm: (param?: FilterConfirmProps | undefined) => void
  ) => {
    confirm();
    onFilterTextChange(
      dataIndex,
      selectedKeys.length === 0 ? "" : selectedKeys[0].toString()
    );
  };

  const reset = (clearFilters: (() => void) | undefined) => {
    if (clearFilters !== undefined) {
      clearFilters();
      onFilterTextChange(dataIndex, "");
    }
  };

  let searchInput: Input | null = null;

  return {
    filterDropdown: ({
      setSelectedKeys,
      selectedKeys,
      confirm,
      clearFilters,
    }: FilterDropdownProps) => (
      <div style={{ padding: 8 }}>
        <Input
          ref={(node) => {
            searchInput = node;
          }}
          placeholder={`Filter ${dataIndex}`}
          value={selectedKeys[0]}
          onChange={(e) =>
            setSelectedKeys(e.target.value ? [e.target.value] : [])
          }
          onPressEnter={() => filter(selectedKeys, confirm)}
          style={{ marginBottom: 8, display: "block" }}
        />
        <Space>
          <Button
            type="primary"
            onClick={() => filter(selectedKeys, confirm)}
            icon={<FilterFilled />}
            size="small"
            style={{ width: 90 }}
          >
            Filter
          </Button>
          <Button
            onClick={() => reset(clearFilters)}
            size="small"
            style={{ width: 90 }}
          >
            Reset
          </Button>
        </Space>
      </div>
    ),
    filterIcon: (filtered: boolean) => (
      <FilterFilled style={{ color: filtered ? "#1890ff" : undefined }} />
    ),
    onFilter: (value: string | number | boolean, record: RecordType) =>
      resolvePath(dataIndex, record)
        ? resolvePath(dataIndex, record)
            .toString()
            .toLowerCase()
            .includes(value.toString().toLowerCase())
        : "",
    onFilterDropdownVisibleChange: (visible: boolean) => {
      if (visible) {
        setTimeout(() => searchInput?.select(), 100);
      }
    },
  };
}

export function highlight(
  textToHightlight: string | null | undefined,
  filterText: string | null | undefined
) {
  return textToHightlight !== null &&
    textToHightlight !== undefined &&
    textToHightlight !== "" &&
    filterText !== null &&
    filterText !== undefined &&
    filterText !== "" ? (
    <Highlighter
      highlightStyle={{ backgroundColor: "#ffc069", padding: 0 }}
      searchWords={[filterText]}
      autoEscape
      textToHighlight={textToHightlight}
    />
  ) : (
    <Typography.Text>{textToHightlight}</Typography.Text>
  );
}
