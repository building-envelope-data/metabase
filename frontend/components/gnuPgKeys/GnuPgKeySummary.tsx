import { GnuPgKeyFingerprintsPartialFragment } from "../../queries/gnuPgKeyFingerprints.generated";
import paths from "../../paths";
import { Tag } from "antd";
import AllowGnuPgKeyFingerprint from "./AllowGnuPgKeyFingerprint";
import ForbidGnuPgKeyFingerprint from "./ForbidGnuPgKeyFingerprint";
import EntityLink from "../entities/EntityLink";
import EntitySummary from "../entities/EntitySummary";
import { isTruthy } from "../../lib/array";
import Link from "next/link";

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

export default function GnuPgKeySummary({
  entity,
  hideInputControls = false,
}: {
  entity: GnuPgKeyFingerprintsPartialFragment;
  hideInputControls?: boolean;
}) {
  const status: Status = entity.allowedAt
    ? entity.forbiddenAt
      ? "forbidden"
      : "allowed"
    : "pending";

  const dateTimes = [
    entity.allowedAt && <div>Allowed at {entity.allowedAt}</div>,
    entity.forbiddenAt && <div>Forbidden at {entity.forbiddenAt}</div>,
  ].filter(isTruthy);

  return (
    <EntitySummary
      entity={entity}
      route={(_) => paths.gnuPgKey(entity.fingerprint)}
      tags={[
        <Tag
          key="status"
          color={color(status)}
          style={{ fontWeight: "normal" }}
        >
          {status}
        </Tag>,
      ]}
      extra={
        !hideInputControls && [
          entity.isAuthorizedToAllowNode && entity.allowedAt == undefined && (
            <AllowGnuPgKeyFingerprint fingerprint={entity.fingerprint} />
          ),
          entity.isAuthorizedToForbidNode &&
            entity.forbiddenAt == undefined && (
              <ForbidGnuPgKeyFingerprint fingerprint={entity.fingerprint} />
            ),
        ]
      }
    >
      {dateTimes.length > 0 && <div>{dateTimes}</div>}
      <div>
        <div>
          Associated with{" "}
          <EntityLink
            entity={entity.institution.node}
            route={paths.institution}
          />
        </div>
        <div>
          Published on the{" "}
          <Link href="https://keys.openpgp.org">OpenPGP Key Server</Link> at{" "}
          <Link
            href={`https://keys.openpgp.org/vks/v1/by-fingerprint/${entity.fingerprint}`}
          ></Link>
          .
        </div>
        <div>
          Owned by <EntityLink entity={entity.user.node} route={paths.user} />
        </div>
      </div>
    </EntitySummary>
  );
}
