import { Typography } from "antd";
import paths from "../../paths";
import {
  DatabasesPartialFragment,
  DatabasePartialFragment,
  PendingDatabasesPartialFragment,
} from "../../queries/databases.generated";
import EntityLink from "../entities/EntityLink";
import EntitySummary from "../entities/EntitySummary";
import { DatabaseVerificationState } from "../../__generated__/graphql";
import { isTruthy } from "../../lib/array";
import UpdateDatabase from "./UpdateDatabase";
import VerifyDatabase from "./VerifyDatabase";
import EnumTag from "../EnumTag";
import CodeView from "../CodeView";

export default function DatabaseSummary({
  entity,
  hideInputControls = false,
  showVerifyAnyway = false,
}: {
  entity:
    | DatabasesPartialFragment
    | PendingDatabasesPartialFragment
    | DatabasePartialFragment;
  hideInputControls?: boolean;
  showVerifyAnyway?: boolean;
}) {
  return (
    <EntitySummary
      entity={entity}
      route={paths.database}
      tags={[
        <EnumTag key="verificationState" color="magenta">
          {entity.verificationState}
        </EnumTag>,
      ]}
      extra={[
        (!hideInputControls || showVerifyAnyway) &&
          "isAuthorizedToVerifyNode" in entity &&
          entity.isAuthorizedToVerifyNode &&
          entity.verificationState == DatabaseVerificationState.Pending && (
            <VerifyDatabase key="verifyDatabase" databaseId={entity.uuid} />
          ),
        !hideInputControls &&
          "isAuthorizedToUpdateNode" in entity &&
          entity.isAuthorizedToUpdateNode && (
            <UpdateDatabase key="updateDatabase" database={entity} />
          ),
      ].filter(isTruthy)}
    >
      <div>
        Operated by <EntityLink entity={entity} route={paths.institution} />
      </div>
      <div>
        <Typography.Link href={entity.locator}>
          {entity.locator}
        </Typography.Link>
      </div>
      {"isAuthorizedToVerifyNode" in entity &&
        entity.isAuthorizedToVerifyNode &&
        entity.verificationState == DatabaseVerificationState.Pending && (
          <Typography.Paragraph style={{ maxWidth: "75ch" }}>
            Have your database&apos;s GraphQL endpoint return the verification
            code
            <CodeView code={entity.verificationCode} />
            when queried for the GraphQL query
            <CodeView code="query { verificationCode }" />
            Then, press the &ldquo;Verify&rdquo; button{" "}
            {!hideInputControls ? (
              "above"
            ) : (
              <>
                on <EntityLink entity={entity} route={paths.database} />
              </>
            )}{" "}
            to make the metabase assert that the verification codes match which
            proves that you control the GraphQL endpoint{" "}
            <Typography.Link href={entity.locator}>
              {entity.locator}
            </Typography.Link>
            . Verified databases are publicly listed and included in data
            searches.
          </Typography.Paragraph>
        )}
    </EntitySummary>
  );
}
