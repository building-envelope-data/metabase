import { OpenIdConnectAuthorizationsPartialFragment } from "../../../queries/openIdConnect.generated";
import { Tag } from "antd";
import { isTruthy } from "../../../lib/array";
import EntitySummary from "../../entities/EntitySummary";
import DeleteOpenIdConnectAuthorization from "./DeleteOpenIdConnectAuthorization";
import { humanize } from "../../../lib/string";
import DateTimeX from "../../DateTimeX";
import paths from "../../../paths";
import EntityLink from "../../entities/EntityLink";

export default function OpenIdConnectAuthorizationSummary({
  entity,
  hideExtra = false,
}: {
  entity: OpenIdConnectAuthorizationsPartialFragment;
  hideExtra?: boolean;
}) {
  const dateTimes = [
    entity.createdAt && {
      key: "createdAt",
      value: entity.createdAt,
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
        !hideExtra &&
        [
          entity.isAuthorizedToDeleteNode && (
            <DeleteOpenIdConnectAuthorization authorizationId={entity.uuid} />
          ),
        ].filter(isTruthy)
      }
    >
      {entity.subject && (
        <div>
          Subject <EntityLink entity={entity.subject} route={paths.user} />
        </div>
      )}
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
