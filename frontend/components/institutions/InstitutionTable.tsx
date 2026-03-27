import { Table } from "antd";
import { InstitutionsPartialFragment } from "../../queries/institutions.generated";
import {
  getNameColumnProps,
  getAbbreviationColumnProps,
  getDescriptionColumnProps,
  getUuidColumnProps,
} from "../../lib/table";
import paths from "../../paths";
import { useState } from "react";
import { setMapValue } from "../../lib/freeTextFilter";

export default function InstitutionTable({
  loading,
  institutions,
}: {
  loading: boolean;
  institutions: InstitutionsPartialFragment[];
}) {
  const [filterText, setFilterText] = useState(() => new Map<string, string>());
  const onFilterTextChange = setMapValue(filterText, setFilterText);

  return (
    <Table
      loading={loading}
      columns={[
        {
          ...getUuidColumnProps<(typeof nodes)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x),
            paths.institution,
          ),
        },
        {
          ...getNameColumnProps<(typeof nodes)[0]>(onFilterTextChange, (x) =>
            filterText.get(x),
          ),
        },
        {
          ...getAbbreviationColumnProps<(typeof nodes)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x),
          ),
        },
        {
          ...getDescriptionColumnProps<(typeof nodes)[0]>(
            onFilterTextChange,
            (x) => filterText.get(x),
          ),
        },
      ]}
      dataSource={institutions}
    />
  );
}
