import { InstitutionNamesDocument } from "../../queries/institutions.generated";
import PaginatedIdSelect, { PaginatedSelectProps } from "../PaginatedIdSelect";

export default function InstitutionIdSelect(
  props: Omit<PaginatedSelectProps, "query">,
) {
  return <PaginatedIdSelect {...props} query={InstitutionNamesDocument} />;
}
