import {
  AllLifeCycleDataDocument,
  AllLifeCycleDataQueryVariables,
  LifeCycleDataPartialFragment,
} from "../../../queries/data.generated";
import LifeCycleDataList from "./LifeCycleDataList";
import PaginatedEntities from "../../entities/PaginatedEntities";
import { LifeCycleDataPropositionInput } from "../../../__generated__/graphql";

export default function PaginatedLifeCycleData({
  where,
}: {
  where?: AllLifeCycleDataQueryVariables["where"];
}) {
  return (
    <PaginatedEntities<
      LifeCycleDataPartialFragment,
      LifeCycleDataPropositionInput,
      any
    >
      showJump={false}
      entitiesQuery={AllLifeCycleDataDocument}
      baseWhere={where}
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
