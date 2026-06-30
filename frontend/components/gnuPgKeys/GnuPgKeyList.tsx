import { GnuPgKeyFingerprintsPartialFragment } from "../../queries/gnuPgKeyFingerprints.generated";
import EntityList from "../entities/EntityList";
import GnuPgKeySummary from "./GnuPgKeySummary";
import EntityItem from "../entities/EntityItem";

export default function GnuPgKeyList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: GnuPgKeyFingerprintsPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <GnuPgKeySummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
