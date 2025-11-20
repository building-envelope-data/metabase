import { useQuery } from "@apollo/client/react";
import { Skeleton, Result } from "antd";
import { SearchSelect } from "./SearchSelect";
import { notEmpty } from "../lib/array";
import { InstitutionsDocument } from "../queries/institutions.generated";

export type SelectInstitutionIdProps = {
  mode?: "multiple" | "tags";
  value?: string;
  onChange?: (value: string) => void;
};

export function SelectInstitutionId({
  mode,
  value,
  onChange,
}: SelectInstitutionIdProps) {
  // TODO Only fetch `name` and `uuid` because nothing more is needed.
  // TODO Use search instead of drop-down with all users/institutions preloaded. Be inspired by https://ant.design/components/select/#components-select-demo-select-users
  const { loading, data, error } = useQuery(InstitutionsDocument);
  const institutions = data?.institutions?.edges
    ?.map((e) => e.node)
    .filter(notEmpty);

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
    <SearchSelect
      value={value}
      mode={mode}
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
