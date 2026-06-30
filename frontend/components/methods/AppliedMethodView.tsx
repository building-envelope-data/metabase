import { Scalars } from "../../__generated__/graphql";
import paths from "../../paths";
import EntityLink from "../entities/EntityLink";
import InlineList from "../InlineList";
import JsonView from "../JsonView";
import {
  AppliedMethodPartialFragment,
  ToTreeVertexAppliedConversionMethodPartialFragment,
} from "../../queries/data.generated";

const nameFallback = (id: Scalars["Uuid"]["output"]) => ({
  uuid: id,
  name: id,
});

export default function AppliedMethodView({
  value,
}: {
  value:
    | AppliedMethodPartialFragment
    | ToTreeVertexAppliedConversionMethodPartialFragment;
}) {
  return (
    <span>
      <EntityLink
        entity={value.method ?? nameFallback(value.methodId)}
        route={paths.method}
      />
      {value.arguments.length > 0 && (
        <>
          {" "}
          with the arguments{" "}
          <InlineList
            items={value.arguments}
            renderItem={(item) => (
              <span>
                <code>{item.name}</code>=&ldquo;
                <JsonView inline data={item.value} />
                &rdquo;
              </span>
            )}
          />
        </>
      )}
      {value.__typename === "AppliedMethod" && value.sources.length > 0 && (
        <>
          {" "}
          on the sources{" "}
          <InlineList
            items={value.sources}
            renderItem={(item) => (
              <span>
                <code>{item.name}</code>=&ldquo;
                <EntityLink
                  entity={{
                    uuid: item.value.dataId,
                    name: `data ${item.value.dataId} in database ${item.value.database?.name ?? item.value.databaseId} at timestamp ${item.value.dataTimestamp}`,
                  }}
                  route={(id) =>
                    paths.data(item.value.databaseId, item.value.dataKind, id)
                  }
                />
                &rdquo;
              </span>
            )}
          />
        </>
      )}
      {value.__typename === "ToTreeVertexAppliedConversionMethod" && (
        <>
          {" "}
          with its parent as the source <code>{value.sourceName}</code>.
        </>
      )}
    </span>
  );
}
