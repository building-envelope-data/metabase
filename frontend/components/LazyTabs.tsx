import React, { useState, useMemo } from "react";
import { GetProp, Tabs, TabsProps } from "antd";
import TabLabel from "./TabLabel";

type TabItem = GetProp<TabsProps, "items">[number];

type TabItemWithCount = TabItem & {
  count?: number;
};

export type LazyTabsProps = Omit<TabsProps, "items"> & {
  items?: TabItemWithCount[];
};

/**
 * Internal wrapper that tracks if a tab has ever been "active".
 * Once initialized, it stays mounted.
 */
const LazyWrapper: React.FC<{ active: boolean; children: React.ReactNode }> = ({
  active,
  children,
}) => {
  const [initialized, setInitialized] = useState(false);

  if (active && !initialized) {
    setInitialized(true);
  }
  if (!initialized) return null;
  return <>{children}</>;
};

/**
 * Reusable LazyTabs Component
 * Extends standard Ant Design TabsProps
 */
export default function LazyTabs({ items, onChange, ...props }: LazyTabsProps) {
  const [activeKey, setActiveKey] = useState<string>(() => {
    return (
      props.activeKey ||
      props.defaultActiveKey ||
      (items?.filter((item) => item.count && item.count > 0)?.[0]
        ?.key as string) ||
      (items?.[0].key as string)
    );
  });

  const handleTabChange = (key: string) => {
    setActiveKey(key);
    onChange?.(key);
  };

  const lazyItems = useMemo(() => {
    return items?.map((item) => ({
      ...item,
      label:
        item.count === undefined ? (
          item.label
        ) : (
          <TabLabel name={item.label} count={item.count} />
        ),
      children: (
        <LazyWrapper active={activeKey === item.key}>
          {item.children}
        </LazyWrapper>
      ),
    }));
  }, [items, activeKey]);

  return (
    <Tabs
      {...props}
      activeKey={activeKey}
      items={lazyItems}
      onChange={handleTabChange}
    />
  );
}
