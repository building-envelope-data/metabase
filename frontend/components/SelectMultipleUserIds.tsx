import { Skeleton, Result } from "antd";
import { SelectMultipleViaSearch } from "./SelectMultipleViaSearch";
import { notEmpty } from "../lib/array";
import { useUsersQuery } from "../queries/users.graphql";

export type SelectMultipleUserIdsProps<ValueType> = {
  value?: ValueType;
  onChange?: (value: ValueType) => void;
};

export function SelectMultipleUserIds<ValueType extends string>({
  value,
  onChange,
}: SelectMultipleUserIdsProps<ValueType>) {
  // TODO Only fetch `name` and `uuid` because nothing more is needed.
  // TODO Use search instead of drop-down with all users/users preloaded. Be inspired by https://ant.design/components/select/#components-select-demo-select-users
  const { loading, data, error } = useUsersQuery();
  const users = data?.users?.nodes?.filter(notEmpty);

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
    <SelectMultipleViaSearch
      value={value}
      onChange={onChange}
      options={
        users?.map((user) => ({
          label: user.name,
          value: user.uuid,
        })) || []
      }
    />
  );
}
