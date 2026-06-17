import { OpenIdConnectTokensPartialFragment } from "../../../queries/openIdConnect.generated";
import EntityList from "../../entities/EntityList";
import OpenIdConnectTokenSummary from "./OpenIdConnectTokenSummary";
import EntityItem from "../../entities/EntityItem";
import OpenIdConnectTokenRibbon from "./OpenIdConnectTokenRibbon";

export default function OpenIdConnectTokenList({
  loading,
  nodes,
  onReload,
}: {
  loading: boolean;
  nodes: OpenIdConnectTokensPartialFragment[] | null;
  onReload: () => void;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      onReload={onReload}
      renderItem={(node) => (
        <OpenIdConnectTokenRibbon {...node}>
          <EntityItem>
            <OpenIdConnectTokenSummary entity={node} />
          </EntityItem>
        </OpenIdConnectTokenRibbon>
      )}
    />
  );
}
