import {
  AllGeometricDataDocument,
  AllGeometricDataQueryVariables,
  GeometricDataPartialFragment,
} from "../../../queries/data.generated";
import GeometricDataList from "./GeometricDataList";
import PaginatedEntities from "../../entities/PaginatedEntities";
import { GeometricDataPropositionInput } from "../../../__generated__/graphql";

export default function PaginatedGeometricData({
  where,
}: {
  where?: AllGeometricDataQueryVariables["where"];
}) {
  return (
    <PaginatedEntities<
      GeometricDataPartialFragment,
      GeometricDataPropositionInput,
      any
    >
      showJump={false}
      entitiesQuery={AllGeometricDataDocument}
      baseWhere={where}
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
