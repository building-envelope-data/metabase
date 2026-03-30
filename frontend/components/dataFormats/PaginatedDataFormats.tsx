import {
  DataFormatsDocument,
  DataFormatNamesDocument,
  DataFormatsQueryVariables,
} from "../../queries/dataFormats.generated";
import paths from "../../paths";
import DataFormatList from "./DataFormatList";
import PaginatedEntities from "../entities/PaginatedEntities";

export default function PaginatedDataFormats({
  where,
  showJump = false,
}: {
  where?: DataFormatsQueryVariables["where"];
  showJump?: boolean;
}) {
  return (
    <PaginatedEntities
      entitiesQuery={DataFormatsDocument}
      namesQuery={DataFormatNamesDocument}
      where={where}
      showJump={showJump}
      route={paths.dataFormat}
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
