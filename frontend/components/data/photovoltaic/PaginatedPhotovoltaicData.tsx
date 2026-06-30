import {
  AllPhotovoltaicDataDocument,
  AllPhotovoltaicDataQueryVariables,
  PhotovoltaicDataPartialFragment,
} from "../../../queries/data.generated";
import PhotovoltaicDataList from "./PhotovoltaicDataList";
import PaginatedEntities from "../../entities/PaginatedEntities";
import { PhotovoltaicDataPropositionInput } from "../../../__generated__/graphql";

export default function PaginatedPhotovoltaicData({
  where,
}: {
  where?: AllPhotovoltaicDataQueryVariables["where"];
}) {
  return (
    <PaginatedEntities<
      PhotovoltaicDataPartialFragment,
      PhotovoltaicDataPropositionInput,
      any
    >
      showJump={false}
      entitiesQuery={AllPhotovoltaicDataDocument}
      baseWhere={where}
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
