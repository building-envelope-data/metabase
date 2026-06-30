import {
  AllHygrothermalDataDocument,
  AllHygrothermalDataQueryVariables,
  HygrothermalDataPartialFragment,
} from "../../../queries/data.generated";
import HygrothermalDataList from "./HygrothermalDataList";
import PaginatedEntities from "../../entities/PaginatedEntities";
import { HygrothermalDataPropositionInput } from "../../../__generated__/graphql";

export default function PaginatedHygrothermalData({
  where,
}: {
  where?: AllHygrothermalDataQueryVariables["where"];
}) {
  return (
    <PaginatedEntities<
      HygrothermalDataPartialFragment,
      HygrothermalDataPropositionInput,
      any
    >
      showJump={false}
      entitiesQuery={AllHygrothermalDataDocument}
      baseWhere={where}
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
