import { useQuery } from "@apollo/client/react";
import { Skeleton, Result } from "antd";
import { SearchSelect } from "./SearchSelect";
import { notEmpty } from "../lib/array";
import { ComponentNamesDocument } from "../queries/components.generated";

export type SelectComponentIdProps = {
  mode?: "multiple" | "tags";
  value?: string;
  onChange?: (value: string) => void;
};

export function SelectComponentId({
  mode,
  value,
  onChange,
}: SelectComponentIdProps) {
  // TODO Use search instead of drop-down with all users/components preloaded. Be inspired by https://ant.design/components/select/#components-select-demo-select-users
  const { loading, data, error } = useQuery(ComponentNamesDocument);
  const components = data?.components?.edges
    ?.map((e) => e.node)
    .filter(notEmpty);

  if (loading) {
    return <Skeleton />;
  }

  if (error) {
    console.error(error);
  }

  if (!components) {
    return <Result status="error" title="Failed to load components." />;
  }

  return (
    <SearchSelect
      value={value}
      mode={mode}
      onChange={onChange}
      options={
        components?.map((component) => ({
          label: component.name,
          value: component.uuid,
        })) || []
      }
    />
  );
}
