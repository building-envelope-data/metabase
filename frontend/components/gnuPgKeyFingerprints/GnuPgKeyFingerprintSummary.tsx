import { GnuPgKeyFingerprintsPartialFragment } from "../../queries/gnuPgKeyFingerprints.generated";
import paths from "../../paths";
import { Tag } from "antd";
import AllowGnuPgKeyFingerprint from "./AllowGnuPgKeyFingerprint";
import ForbidGnuPgKeyFingerprint from "./ForbidGnuPgKeyFingerprint";
import EntityLink from "../entities/EntityLink";
import EntitySummary from "../entities/EntitySummary";

type Status = "pending" | "allowed" | "forbidden";

const color = (status: Status) => {
  switch (status) {
    case "pending":
      return "processing"; // "warning"
    case "allowed":
      return "success";
    case "forbidden":
      return "error";
  }
};

export default function GnuPgKeyFingerprintSummary({
  entity,
}: {
  entity: GnuPgKeyFingerprintsPartialFragment;
}) {
  const status: Status = entity.allowedAt
    ? entity.forbiddenAt
      ? "forbidden"
      : "allowed"
    : "pending";

  return (
    <EntitySummary
      entity={entity}
      route={(_) => paths.gnuPgKeyFingerprint(entity.fingerprint)}
      tags={[
        <Tag
          key="status"
          color={color(status)}
          style={{ fontWeight: "normal" }}
        >
          {status}
        </Tag>,
      ]}
      extra={[
        entity.isAuthorizedToAllowNode && entity.allowedAt == undefined && (
          <AllowGnuPgKeyFingerprint
            fingerprint={entity.fingerprint}
            institutionId={entity.institution.node.uuid}
          />
        ),
        entity.isAuthorizedToForbidNode && entity.forbiddenAt == undefined && (
          <ForbidGnuPgKeyFingerprint
            fingerprint={entity.fingerprint}
            institutionId={entity.institution.node.uuid}
          />
        ),
      ]}
    >
      {entity.allowedAt && <div>Allowed at {entity.allowedAt}</div>}
      {entity.forbiddenAt && <div>Forbidden at {entity.forbiddenAt}</div>}
      <div>
        Owned by <EntityLink entity={entity.user.node} route={paths.user} />
      </div>
    </EntitySummary>
  );
}
