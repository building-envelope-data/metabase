import { OpenIdConnectApplicationsPartialFragment } from "../../../queries/openIdConnect.generated";
import EntityList from "../../entities/EntityList";
import OpenIdConnectApplicationSummary from "./OpenIdConnectApplicationSummary";
import EntityItem from "../../entities/EntityItem";

export default function OpenIdConnectApplicationList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: OpenIdConnectApplicationsPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <OpenIdConnectApplicationSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
