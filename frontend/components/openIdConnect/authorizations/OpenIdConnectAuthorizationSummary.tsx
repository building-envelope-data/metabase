import { OpenIdConnectAuthorizationsPartialFragment } from "../../../queries/openIdConnect.generated";
import { Tag } from "antd";
import { isTruthy } from "../../../lib/array";
import EntitySummary from "../../entities/EntitySummary";
import DeleteOpenIdConnectAuthorization from "./DeleteOpenIdConnectAuthorization";

export default function OpenIdConnectAuthorizationSummary({
  entity,
  hideExtra = false,
}: {
  entity: OpenIdConnectAuthorizationsPartialFragment;
  hideExtra?: boolean;
}) {
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
            <DeleteOpenIdConnectAuthorization
              authorizationId={entity.uuid}
              applicationId={entity.application.node.uuid}
            />
          ),
        ].filter(isTruthy)
      }
    >
      <div>Subject {entity.subject}</div>
      <div>Created At {entity.createdAt}</div>
    </EntitySummary>
  );
}
