import {
  AllGeometricDataDocument,
  AllGeometricDataQueryVariables,
} from "../../../queries/data.generated";
import GeometricDataList from "./GeometricDataList";
import PaginatedEntities from "../../entities/PaginatedEntities";

export default function PaginatedGeometricData({
  where,
}: {
  where?: AllGeometricDataQueryVariables["where"];
}) {
  return (
    <PaginatedEntities
      showJump={false}
      entitiesQuery={AllGeometricDataDocument}
      where={where}
      list={(props) => <GeometricDataList {...props} />}
      filterDefinitions={[
        {
          field: "componentId",
          type: "uuid",
        },
        {
          field: "resources",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "dataFormatId",
                type: "uuid",
              },
              {
                field: "archivedFilesMetaInformation",
                type: "list",
                item: {
                  type: "object",
                  items: [
                    {
                      field: "dataFormatId",
                      type: "uuid",
                    },
                  ],
                },
              },
            ],
          },
        },
        {
          field: "thicknesses",
          type: "list",
          item: {
            type: "float",
          },
        },
      ]}
      sortDefinitions={[]}
    />
  );
}
