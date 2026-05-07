import { Tag, Tooltip, Typography } from "antd";
import { isTruthy } from "../../lib/array";
import paths from "../../paths";
import EntitySummary from "../entities/EntitySummary";
import { Reference } from "../Reference";
import UpdateDataFormat from "./UpdateDataFormat";
import {
  DataFormatsPartialFragment,
  DataFormatPartialFragment,
} from "../../queries/dataFormats.generated";
import Manager from "../Manager";

export default function DataFormatSummary({
  entity,
  hideExtra = false,
}: {
  entity: DataFormatsPartialFragment | DataFormatPartialFragment;
  hideExtra?: boolean;
}) {
  return (
    <EntitySummary
      entity={entity}
      route={paths.dataFormat}
      tags={[
        <Tag key="extension" style={{ fontWeight: "normal" }}>
          <Tooltip title="File Extension">*.{entity.extension}</Tooltip>
        </Tag>,
        <Tag key="mediaType" style={{ fontWeight: "normal" }}>
          <Tooltip title="Open Media-Type Specification">
            <Typography.Link
              target="_blank"
              rel="noopener noreferrer"
              href={`https://www.iana.org/assignments/media-types/${entity.mediaType}`}
            >
              {entity.mediaType}
            </Typography.Link>
          </Tooltip>
        </Tag>,
      ]}
      extra={
        !hideExtra &&
        [
          "isAuthorizedToUpdateNode" in entity &&
            entity.isAuthorizedToUpdateNode && (
              <UpdateDataFormat key="updateDataFormat" dataFormat={entity} />
            ),
        ].filter(isTruthy)
      }
    >
      {entity.schemaLocator && (
        <div>
          <Typography.Link href={entity.schemaLocator}>
            {entity.schemaLocator}
          </Typography.Link>
        </div>
      )}
      {entity.reference && <Reference data={entity.reference} />}
      {"manager" in entity && (
        <div>
          <Manager data={entity.manager.node} />
        </div>
      )}
    </EntitySummary>
  );
}
