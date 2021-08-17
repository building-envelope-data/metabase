import { Input, Space, Button } from "antd";
import { FilterFilled } from "@ant-design/icons";
import Highlighter from "react-highlight-words";
import {
  FilterConfirmProps,
  FilterDropdownProps,
} from "antd/lib/table/interface";
import { Dispatch, forwardRef, Key, SetStateAction } from "react";

export function setMapValue(
  map: Map<string, string>,
  setMap: Dispatch<SetStateAction<Map<string, string>>>
) {
  return (key: string) => {
    return (newValue: string) => {
      if (map.get(key) !== newValue) {
        const copy = new Map(map);
        copy.set(key, newValue);
        setMap(copy);
      }
    };
  };
}

export function doesFieldIncludeFilterValue(
  field: string,
  value: string | number | boolean
) {
  return field.toLowerCase().includes(value.toString().toLowerCase());
}

export function getFreeTextFilterProps<RecordType>(
  getField: (record: RecordType) => string | null | undefined,
  onFilterTextChange: (newFilterText: string) => void
) {
  const filter = (
    selectedKeys: Key[],
    confirm: (param?: FilterConfirmProps | undefined) => void
  ) => {
    confirm();
    onFilterTextChange(
      selectedKeys.length === 0 ? "" : selectedKeys[0].toString()
    );
  };

  const reset = (clearFilters: (() => void) | undefined) => {
    if (clearFilters !== undefined) {
      clearFilters();
      onFilterTextChange("");
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
          placeholder={"Filter"}
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
    onFilter: (value: string | number | boolean, record: RecordType) => {
      const field = getField(record);
      return field ? doesFieldIncludeFilterValue(field, value) : false;
    },
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
  return (
    <Highlight textToHightlight={textToHightlight} filterText={filterText} />
  );
  // return textToHightlight !== null &&
  //   textToHightlight !== undefined &&
  //   textToHightlight !== "" &&
  //   filterText !== null &&
  //   filterText !== undefined &&
  //   filterText !== "" ? (
  //   <Highlighter
  //     highlightStyle={{ backgroundColor: "#ffc069", padding: 0 }}
  //     searchWords={[filterText]}
  //     autoEscape
  //     textToHighlight={textToHightlight}
  //   />
  // ) : (
  //   <>{textToHightlight}</>
  // );
}

type HighlightProps = {
  textToHightlight: string | null | undefined;
  filterText: string | null | undefined;
};

export const Highlight = forwardRef<Highlighter, HighlightProps>(
  ({ textToHightlight, filterText }, ref) => {
    return textToHightlight !== null &&
      textToHightlight !== undefined &&
      textToHightlight !== "" &&
      filterText !== null &&
      filterText !== undefined &&
      filterText !== "" ? (
      <Highlighter
        ref={ref}
        highlightStyle={{ backgroundColor: "#ffc069", padding: 0 }}
        searchWords={[filterText]}
        autoEscape
        textToHighlight={textToHightlight}
      />
    ) : (
      <>{textToHightlight}</>
    );
  }
);
