import {
  GnuPgKeyFingerprintsDocument,
  GnuPgKeyFingerprintsPartialFragment,
  GnuPgKeyFingerprintsQueryVariables,
} from "../../queries/gnuPgKeyFingerprints.generated";
import paths from "../../paths";
import GnuPgKeyList from "./GnuPgKeyList";
import PaginatedEntities from "../entities/PaginatedEntities";
import {
  GnuPgKeyFingerprintFilterInput,
  GnuPgKeyFingerprintSortInput,
} from "../../__generated__/graphql";

export default function PaginatedGnuPgKeys({
  where,
  order,
  showJump = false,
  extra,
}: {
  where?: GnuPgKeyFingerprintsQueryVariables["where"];
  order?: GnuPgKeyFingerprintsQueryVariables["order"];
  showJump?: boolean;
  extra?: React.ReactNode;
}) {
  return (
    <PaginatedEntities<
      GnuPgKeyFingerprintsPartialFragment,
      GnuPgKeyFingerprintFilterInput,
      GnuPgKeyFingerprintSortInput
    >
      entitiesQuery={GnuPgKeyFingerprintsDocument}
      // namesQuery={GnuPgKeyFingerprintNamesDocument}
      baseWhere={where}
      defaultOrder={order}
      showJump={showJump}
      route={paths.gnuPgKey}
      extra={extra}
      list={(props) => <GnuPgKeyList {...props} />}
      filterDefinitions={[
        {
          field: "fingerprint",
          type: "string",
        },
        {
          field: "institution",
          type: "object",
          items: [
            {
              field: "name",
              type: "string",
            },
            { field: "id", type: "uuid" },
          ],
        },
        {
          field: "user",
          type: "object",
          items: [
            {
              field: "name",
              type: "string",
            },
            { field: "id", type: "uuid" },
          ],
        },
        {
          field: "id",

          type: "uuid",
        },
      ]}
      sortDefinitions={[
        { field: "fingerprint" },
        { field: "createdAt" },
        { field: "allowedAt" },
        { field: "forbiddenAt" },
        { field: "updatedAt" },
        { field: "id" },
      ]}
    />
  );
}
