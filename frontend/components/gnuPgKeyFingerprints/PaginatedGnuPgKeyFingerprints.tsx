import {
  GnuPgKeyFingerprintsDocument,
  GnuPgKeyFingerprintsQueryVariables,
} from "../../queries/gnuPgKeyFingerprints.generated";
import paths from "../../paths";
import GnuPgKeyFingerprintList from "./GnuPgKeyFingerprintList";
import PaginatedEntities from "../entities/PaginatedEntities";

export default function PaginatedGnuPgKeyFingerprints({
  where,
  showJump = false,
}: {
  where?: GnuPgKeyFingerprintsQueryVariables["where"];
  showJump?: boolean;
}) {
  return (
    <PaginatedEntities
      entitiesQuery={GnuPgKeyFingerprintsDocument}
      // namesQuery={GnuPgKeyFingerprintNamesDocument}
      where={where}
      showJump={showJump}
      route={paths.gnuPgKeyFingerprint}
      list={(props) => <GnuPgKeyFingerprintList {...props} />}
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
