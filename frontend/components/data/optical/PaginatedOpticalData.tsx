import {
  AllOpticalDataDocument,
  AllOpticalDataQueryVariables,
  OpticalDataPartialFragment,
} from "../../../queries/data.generated";
import OpticalDataList from "./OpticalDataList";
import PaginatedEntities from "../../entities/PaginatedEntities";
import { OpticalDataPropositionInput } from "../../../__generated__/graphql";

export default function PaginatedOpticalData({
  where,
}: {
  where?: AllOpticalDataQueryVariables["where"];
}) {
  return (
    <PaginatedEntities<
      OpticalDataPartialFragment,
      OpticalDataPropositionInput,
      any
    >
      showJump={false}
      entitiesQuery={AllOpticalDataDocument}
      baseWhere={where}
      list={(props) => <OpticalDataList {...props} />}
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
        // {
        //   field: "type",
        //   type: "enum",
        //   enumObject: OpticalComponentType,
        // },
        // {
        //   field: "subtype",
        //   type: "enum",
        //   enumObject: OpticalComponentSubtype,
        // },
        // {
        //   field: "coatedSide",
        //   type: "enum",
        //   enumObject: CoatedSide,
        // },
        {
          field: "nearnormalHemisphericalSolarReflectances",
          type: "list",
          item: {
            type: "float",
          },
        },
        {
          field: "nearnormalHemisphericalSolarTransmittances",
          type: "list",
          item: {
            type: "float",
          },
        },
        {
          field: "nearnormalHemisphericalVisibleReflectances",
          type: "list",
          item: {
            type: "float",
          },
        },
        {
          field: "nearnormalHemisphericalVisibleTransmittances",
          type: "list",
          item: {
            type: "float",
          },
        },
        {
          field: "infraredEmittances",
          type: "list",
          item: {
            type: "float",
          },
        },
        {
          field: "cielabColors",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "lStar",
                type: "float",
              },
              {
                field: "aStar",
                type: "float",
              },
              {
                field: "bStar",
                type: "float",
              },
            ],
          },
        },
        {
          field: "colorRenderingIndices",
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
