import { ComponentNamesDocument } from "../../queries/components.generated";
import PaginatedIdSelect, { PaginatedSelectProps } from "../PaginatedIdSelect";

export function ComponentIdSelect({
  mode,
  value,
  onChange,
}: Omit<PaginatedSelectProps, "query">) {
  return (
    <PaginatedIdSelect
      query={ComponentNamesDocument}
      value={value}
      mode={mode}
      onChange={onChange}
    />
  );
}
