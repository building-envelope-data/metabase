import { InstitutionNamesDocument } from "../../queries/institutions.generated";
import PaginatedIdSelect, { PaginatedSelectProps } from "../PaginatedIdSelect";

export function InstitutionIdSelect({
  mode,
  value,
  onChange,
}: Omit<PaginatedSelectProps, "query">) {
  return (
    <PaginatedIdSelect
      query={InstitutionNamesDocument}
      value={value}
      mode={mode}
      onChange={onChange}
    />
  );
}
