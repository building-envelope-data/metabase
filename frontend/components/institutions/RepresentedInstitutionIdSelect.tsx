import { useQuery } from "@apollo/client/react";
import { CurrentUserDocument } from "../../queries/currentUser.generated";
import { Select, SelectProps } from "antd";
import { createPaginatedIdSelectOption } from "../PaginatedIdSelect";

export default function RepresentedInstitutionIdSelect(
  props: Omit<SelectProps, "options">,
) {
  const currentUser = useQuery(CurrentUserDocument)?.data?.currentUser;

  return (
    <Select
      {...props}
      options={currentUser?.representedInstitutions.edges.map(({ node }) =>
        createPaginatedIdSelectOption(node),
      )}
    />
  );
}
