import { OpenIdConnectTokensPartialFragment } from "../../../queries/openIdConnect.generated";
import { Tag } from "antd";
import { isTruthy } from "../../../lib/array";
import EntitySummary from "../../entities/EntitySummary";
import RevokeOpenIdConnectToken from "./RevokeOpenIdConnectToken";
import { humanize } from "../../../lib/string";

export default function OpenIdConnectTokenSummary({
  entity,
  hideExtra = false,
}: {
  entity: OpenIdConnectTokensPartialFragment;
  hideExtra?: boolean;
}) {
  const dateTimes = [
    {
      key: "createdAt",
      value: entity.createdAt,
    },
    {
      key: "expiredAt",
      value: entity.expiredAt,
    },
    {
      key: "redeemedAt",
      value: entity.redeemedAt,
    },
  ].filter((x) => x.value != null);

  return (
    <EntitySummary
      entity={entity}
      tags={[
        <Tag key="status">{entity.status}</Tag>,
        <Tag key="type">{entity.type}</Tag>,
      ]}
      extra={
        !hideExtra &&
        [
          entity.isAuthorizedToRevokeNode && (
            <RevokeOpenIdConnectToken
              tokenId={entity.uuid}
              applicationId={entity.application.node.uuid}
            />
          ),
        ].filter(isTruthy)
      }
    >
      <div>Subject {entity.subject}</div>
      {dateTimes.length > 0 && (
        <div>
          {dateTimes.map((x) => (
            <div key={x.key}>
              {humanize(x.key, "all-upper")} {x.value}
            </div>
          ))}
        </div>
      )}
    </EntitySummary>
  );
}
