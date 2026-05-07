import { OpenIdConnectTokensPartialFragment } from "../../../queries/openIdConnect.generated";
import EntityList from "../../entities/EntityList";
import OpenIdConnectTokenSummary from "./OpenIdConnectTokenSummary";
import EntityItem from "../../entities/EntityItem";

export default function OpenIdConnectTokenList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: OpenIdConnectTokensPartialFragment[];
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <OpenIdConnectTokenSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
