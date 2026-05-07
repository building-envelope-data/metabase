import { GnuPgKeyFingerprintsPartialFragment } from "../../queries/gnuPgKeyFingerprints.generated";
import EntityList from "../entities/EntityList";
import GnuPgKeySummary from "./GnuPgKeySummary";
import EntityItem from "../entities/EntityItem";

export default function GnuPgKeyList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: GnuPgKeyFingerprintsPartialFragment[];
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <GnuPgKeySummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
