import { UserNamesDocument } from "../../queries/users.generated";
import PaginatedIdSelect, { PaginatedSelectProps } from "../PaginatedIdSelect";

export default function UserIdSelect(
  props: Omit<PaginatedSelectProps, "query">,
) {
  return <PaginatedIdSelect {...props} query={UserNamesDocument} />;
}
