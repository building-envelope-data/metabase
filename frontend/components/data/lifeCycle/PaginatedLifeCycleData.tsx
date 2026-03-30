import {
  AllLifeCycleDataDocument,
  AllLifeCycleDataQueryVariables,
} from "../../../queries/data.generated";
import LifeCycleDataList from "./LifeCycleDataList";
import PaginatedEntities from "../../entities/PaginatedEntities";

export default function PaginatedLifeCycleData({
  where,
}: {
  where?: AllLifeCycleDataQueryVariables["where"];
}) {
  return (
    <PaginatedEntities
      showJump={false}
      entitiesQuery={AllLifeCycleDataDocument}
      where={where}
      list={(props) => <LifeCycleDataList {...props} />}
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
