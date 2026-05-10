import {
  DataFormatsDocument,
  DataFormatNamesDocument,
  DataFormatsQueryVariables,
  DataFormatsPartialFragment,
} from "../../queries/dataFormats.generated";
import paths from "../../paths";
import DataFormatList from "./DataFormatList";
import PaginatedEntities from "../entities/PaginatedEntities";
import {
  DataFormatFilterInput,
  DataFormatSortInput,
} from "../../__generated__/graphql";

export default function PaginatedDataFormats({
  where,
  order,
  showJump = false,
  extra,
}: {
  where?: DataFormatsQueryVariables["where"];
  order?: DataFormatsQueryVariables["order"];
  showJump?: boolean;
  extra?: React.ReactNode;
}) {
  return (
    <PaginatedEntities<
      DataFormatsPartialFragment,
      DataFormatFilterInput,
      DataFormatSortInput
    >
      entitiesQuery={DataFormatsDocument}
      namesQuery={DataFormatNamesDocument}
      baseWhere={where}
      defaultOrder={order}
      showJump={showJump}
      route={paths.dataFormat}
      extra={extra}
      list={(props) => <DataFormatList {...props} />}
      filterDefinitions={[
        {
          field: "name",
          type: "string",
        },
        {
          field: "description",
          type: "string",
        },
        {
          field: "extension",
          type: "string",
        },
        {
          field: "mediaType",
          type: "string",
        },
        {
          field: "manager",
          type: "object",
          items: [
            {
              field: "name",
              type: "string",
            },
            { field: "id", type: "uuid" },
          ],
        },
        {
          field: "id",
          type: "uuid",
        },
      ]}
      sortDefinitions={[
        { field: "name" },
        { field: "extension" },
        { field: "mediaType" },
        { field: "createdAt" },
        { field: "updatedAt" },
        { field: "id" },
      ]}
    />
  );
}
