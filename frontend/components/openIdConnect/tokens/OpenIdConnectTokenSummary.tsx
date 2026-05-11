import { OpenIdConnectTokensPartialFragment } from "../../../queries/openIdConnect.generated";
import { Tag } from "antd";
import { isTruthy } from "../../../lib/array";
import EntitySummary from "../../entities/EntitySummary";
import RevokeOpenIdConnectToken from "./RevokeOpenIdConnectToken";
import { humanize } from "../../../lib/string";
import DateTimeX from "../../DateTimeX";
import paths from "../../../paths";
import EntityLink from "../../entities/EntityLink";

export default function OpenIdConnectTokenSummary({
  entity,
  hideInputControls = false,
}: {
  entity: OpenIdConnectTokensPartialFragment;
  hideInputControls?: boolean;
}) {
  const associates = [
    entity.subject && (
      <div>
        Subject{" "}
        <EntityLink
          entity={entity.subject}
          route={
            entity.subject.__typename == "User"
              ? paths.user
              : paths.openIdConnectApplication
          }
        />
      </div>
    ),
    entity.authorization && (
      <div>Authorization {entity.authorization.node.uuid}</div>
    ),
  ].filter(isTruthy);

  const dateTimes = [
    entity.createdAt && {
      key: "createdAt",
      value: entity.createdAt,
    },
    entity.expiredAt && {
      key: "expiredAt",
      value: entity.expiredAt,
    },
    entity.redeemedAt && {
      key: "redeemedAt",
      value: entity.redeemedAt,
    },
  ].filter(isTruthy);

  return (
    <EntitySummary
      entity={entity}
      tags={[
        <Tag key="status">{entity.status}</Tag>,
        <Tag key="type">{entity.type}</Tag>,
      ]}
      extra={
        !hideInputControls &&
        [
          entity.isAuthorizedToRevokeNode && (
            <RevokeOpenIdConnectToken tokenId={entity.uuid} />
          ),
        ].filter(isTruthy)
      }
    >
      {associates.length > 0 && <div>{associates}</div>}
      {dateTimes.length > 0 && (
        <div>
          {dateTimes.map((x) => (
            <div key={x.key}>
              {humanize(x.key, "all-upper")} <DateTimeX value={x.value} />
            </div>
          ))}
        </div>
      )}
    </EntitySummary>
  );
}
