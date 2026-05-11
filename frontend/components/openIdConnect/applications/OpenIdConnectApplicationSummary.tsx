import {
  OpenIdConnectApplicationPartialFragment,
  OpenIdConnectApplicationsPartialFragment,
} from "../../../queries/openIdConnect.generated";
import paths from "../../../paths";
import { Tag, Typography } from "antd";
import { isTruthy } from "../../../lib/array";
import EntitySummary from "../../entities/EntitySummary";
import DeleteOpenIdConnectApplication from "./DeleteOpenIdConnectApplication";
import ResetOpenIdConnectApplicationClientSecret from "./ResetOpenIdConnectApplicationClientSecret";
import UpdateOpenIdConnectApplication from "./UpdateOpenIdConnectApplication";
import InlineList from "../../InlineList";
import EntityLink from "../../entities/EntityLink";
import { humanize } from "../../../lib/string";

export default function OpenIdConnectApplicationSummary({
  entity,
  hideInputControls = false,
}: {
  entity:
    | OpenIdConnectApplicationsPartialFragment
    | OpenIdConnectApplicationPartialFragment;
  hideInputControls?: boolean;
}) {
  return (
    <EntitySummary
      entity={entity}
      route={paths.openIdConnectApplication}
      tags={[
        <Tag key="consentType">
          Consent Type "{humanize(entity.consentType, "all-upper")}"
        </Tag>,
      ]}
      extra={
        !hideInputControls &&
        [
          "isAuthorizedToManageNode" in entity &&
            entity.isAuthorizedToManageNode && (
              <UpdateOpenIdConnectApplication
                key="updateApplication"
                application={entity}
              />
            ),
          "isAuthorizedToManageNode" in entity &&
            entity.isAuthorizedToManageNode && (
              <ResetOpenIdConnectApplicationClientSecret
                key="resetApplicationClientSecret"
                applicationId={entity.uuid}
              />
            ),
          "isAuthorizedToManageNode" in entity &&
            entity.isAuthorizedToManageNode && (
              <DeleteOpenIdConnectApplication
                key="deleteApplication"
                applicationId={entity.uuid}
                redirectTo={paths.institution(entity.owner.node.uuid)}
              />
            ),
        ].filter(isTruthy)
      }
    >
      {(entity.redirectUri || entity.postLogoutRedirectUri) && (
        <div>
          {entity.redirectUri && (
            <div>
              After login, redirect to{" "}
              <Typography.Link href={entity.redirectUri}>
                {entity.redirectUri}
              </Typography.Link>
            </div>
          )}
          {entity.postLogoutRedirectUri && (
            <div>
              After logout, redirect to{" "}
              <Typography.Link href={entity.postLogoutRedirectUri}>
                {entity.postLogoutRedirectUri}
              </Typography.Link>
            </div>
          )}
        </div>
      )}
      <div>
        <div>
          Endpoints{" "}
          <InlineList
            items={entity.endpoints}
            renderItem={(item) => <code key={item}>{item}</code>}
          />
        </div>
        <div>
          Grant Types{" "}
          <InlineList
            items={entity.grantTypes}
            renderItem={(item) => <code key={item}>{item}</code>}
          />
        </div>
        <div>
          Response Types{" "}
          <InlineList
            items={entity.responseTypes}
            renderItem={(item) => <code key={item}>{item}</code>}
          />
        </div>
        <div>
          Scopes{" "}
          <InlineList
            items={entity.scopes}
            renderItem={(item) => <code key={item}>{item}</code>}
          />
        </div>
        <div>
          Requirements{" "}
          <InlineList
            items={entity.requirements}
            renderItem={(item) => <code key={item}>{item}</code>}
          />
        </div>
      </div>
      <div>
        Owned by{" "}
        <EntityLink entity={entity.owner.node} route={paths.institution} />
      </div>
    </EntitySummary>
  );
}
