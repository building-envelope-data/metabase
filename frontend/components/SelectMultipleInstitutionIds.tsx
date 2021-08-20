import { Skeleton, Result } from "antd";
import { SelectMultipleViaSearch } from "./SelectMultipleViaSearch";
import { notEmpty } from "../lib/array";
import { useInstitutionsQuery } from "../queries/institutions.graphql";

export type SelectMultipleInstitutionIdsProps<ValueType> = {
  value?: ValueType;
  onChange?: (value: ValueType) => void;
};

export function SelectMultipleInstitutionIds<ValueType extends string>({
  value,
  onChange,
}: SelectMultipleInstitutionIdsProps<ValueType>) {
  // TODO Only fetch `name` and `uuid` because nothing more is needed.
  // TODO Use search instead of drop-down with all users/institutions preloaded. Be inspired by https://ant.design/components/select/#components-select-demo-select-users
  const { loading, data, error } = useInstitutionsQuery();
  const institutions = data?.institutions?.nodes?.filter(notEmpty);

  if (loading) {
    return <Skeleton />;
  }

  if (error) {
    console.error(error);
  }

  if (!institutions) {
    return <Result status="error" title="Failed to load institutions." />;
  }

  return (
    <SelectMultipleViaSearch
      value={value}
      onChange={onChange}
      options={
        institutions?.map((institution) => ({
          label: institution.name,
          value: institution.uuid,
        })) || []
      }
    />
  );
}
