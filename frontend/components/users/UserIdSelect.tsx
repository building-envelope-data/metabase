import { UserNamesDocument } from "../../queries/users.generated";
import PaginatedIdSelect, { PaginatedSelectProps } from "../PaginatedIdSelect";

export function UserIdSelect({
  mode,
  value,
  onChange,
}: Omit<PaginatedSelectProps, "query">) {
  return (
    <PaginatedIdSelect
      query={UserNamesDocument}
      value={value}
      mode={mode}
      onChange={onChange}
    />
  );
}
