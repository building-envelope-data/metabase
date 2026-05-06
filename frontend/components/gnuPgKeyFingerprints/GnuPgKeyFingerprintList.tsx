import { GnuPgKeyFingerprintsPartialFragment } from "../../queries/gnuPgKeyFingerprints.generated";
import EntityList from "../entities/EntityList";
import GnuPgKeyFingerprintSummary from "./GnuPgKeyFingerprintSummary";
import EntityItem from "../entities/EntityItem";

export default function GnuPgKeyFingerprintList({
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
          <GnuPgKeyFingerprintSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
