import {
  AllCalorimetricDataDocument,
  AllCalorimetricDataQueryVariables,
} from "../../../queries/data.generated";
import CalorimetricDataList from "./CalorimetricDataList";
import PaginatedEntities from "../../entities/PaginatedEntities";

export default function PaginatedCalorimetricData({
  where,
}: {
  where?: AllCalorimetricDataQueryVariables["where"];
}) {
  return (
    <PaginatedEntities
      showJump={false}
      entitiesQuery={AllCalorimetricDataDocument}
      where={where}
      list={(props) => <CalorimetricDataList {...props} />}
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
          field: "gValues",
          type: "list",
          item: {
            type: "float",
          },
        },
        {
          field: "uValues",
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
