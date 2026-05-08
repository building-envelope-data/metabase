import { ComponentNamesDocument } from "../../queries/components.generated";
import PaginatedIdSelect, { PaginatedSelectProps } from "../PaginatedIdSelect";

export default function ComponentIdSelect(
  props: Omit<PaginatedSelectProps, "query">,
) {
  return <PaginatedIdSelect {...props} query={ComponentNamesDocument} />;
}
