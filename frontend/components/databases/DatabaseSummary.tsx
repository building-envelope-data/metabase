import { Tag, Typography } from "antd";
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

export default function DatabaseSummary({
  entity,
}: {
  entity:
    | DatabasesPartialFragment
    | PendingDatabasesPartialFragment
    | DatabasePartialFragment;
}) {
  return (
    <EntitySummary
      entity={entity}
      route={paths.database}
      tags={[
        <Tag key="verificationState" color="magenta">
          {entity.verificationState}
        </Tag>,
      ]}
      extra={[
        "isAuthorizedToVerifyNode" in entity &&
          entity.isAuthorizedToVerifyNode &&
          entity.verificationState == DatabaseVerificationState.Pending && (
            <VerifyDatabase key="verifyDatabase" databaseId={entity.uuid} />
          ),
        "isAuthorizedToUpdateNode" in entity &&
          entity.isAuthorizedToUpdateNode && (
            <UpdateDatabase key="updateDatabase" database={entity} />
          ),
      ].filter(isTruthy)}
    >
      <div>
        Operated by <EntityLink entity={entity} route={paths.database} />
      </div>
      <div>
        <Typography.Link href={entity.locator}>
          {entity.locator}
        </Typography.Link>
      </div>
    </EntitySummary>
  );
}
