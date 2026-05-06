import {
  AllPhotovoltaicDataDocument,
  AllPhotovoltaicDataQueryVariables,
} from "../../../queries/data.generated";
import PhotovoltaicDataList from "./PhotovoltaicDataList";
import PaginatedEntities from "../../entities/PaginatedEntities";

export default function PaginatedPhotovoltaicData({
  where,
}: {
  where?: AllPhotovoltaicDataQueryVariables["where"];
}) {
  return (
    <PaginatedEntities
      showJump={false}
      entitiesQuery={AllPhotovoltaicDataDocument}
      where={where}
      list={(props) => <PhotovoltaicDataList {...props} />}
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
      ]}
      sortDefinitions={[]}
    />
  );
}
