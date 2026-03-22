import { useQuery } from "@apollo/client/react";
import { Skeleton, Result } from "antd";
import { SearchSelect } from "./SearchSelect";
import { notEmpty } from "../lib/array";
import { UserNamesDocument } from "../queries/users.generated";

export type SelectUserIdProps = {
  mode?: "multiple" | "tags";
  value?: string;
  onChange?: (value: string) => void;
};

export function SelectUserId({ mode, value, onChange }: SelectUserIdProps) {
  // TODO Use search instead of drop-down with all users/users preloaded. Be inspired by https://ant.design/components/select/#components-select-demo-select-users
  const { loading, data, error } = useQuery(UserNamesDocument);
  const users = data?.users?.edges?.map((e) => e.node).filter(notEmpty);

  if (loading) {
    return <Skeleton />;
  }

  if (error) {
    console.error(error);
  }

  if (!users) {
    return <Result status="error" title="Failed to load users." />;
  }

  return (
    <SearchSelect
      value={value}
      mode={mode}
      onChange={onChange}
      options={
        users?.map((user) => ({
          label: `${user.name} (${user.uuid})`,
          value: user.uuid,
        })) || []
      }
    />
  );
}
