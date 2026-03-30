import {
  AllHygrothermalDataDocument,
  AllHygrothermalDataQueryVariables,
} from "../../../queries/data.generated";
import HygrothermalDataList from "./HygrothermalDataList";
import PaginatedEntities from "../../entities/PaginatedEntities";

export default function PaginatedHygrothermalData({
  where,
}: {
  where?: AllHygrothermalDataQueryVariables["where"];
}) {
  return (
    <PaginatedEntities
      showJump={false}
      entitiesQuery={AllHygrothermalDataDocument}
      where={where}
      list={(props) => <HygrothermalDataList {...props} />}
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
