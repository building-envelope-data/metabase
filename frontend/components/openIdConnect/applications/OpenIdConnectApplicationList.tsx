import { OpenIdConnectApplicationsPartialFragment } from "../../../queries/openIdConnect.generated";
import EntityList from "../../entities/EntityList";
import OpenIdConnectApplicationSummary from "./OpenIdConnectApplicationSummary";
import EntityItem from "../../entities/EntityItem";

export default function OpenIdConnectApplicationList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: OpenIdConnectApplicationsPartialFragment[] | null;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <OpenIdConnectApplicationSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
