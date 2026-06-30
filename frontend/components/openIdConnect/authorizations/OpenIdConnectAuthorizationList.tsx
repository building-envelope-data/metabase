import { OpenIdConnectAuthorizationsPartialFragment } from "../../../queries/openIdConnect.generated";
import EntityList from "../../entities/EntityList";
import OpenIdConnectAuthorizationSummary from "./OpenIdConnectAuthorizationSummary";
import EntityItem from "../../entities/EntityItem";

export default function OpenIdConnectAuthorizationList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: OpenIdConnectAuthorizationsPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <EntityItem>
          <OpenIdConnectAuthorizationSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
