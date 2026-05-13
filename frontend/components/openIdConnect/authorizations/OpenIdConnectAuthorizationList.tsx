import { OpenIdConnectAuthorizationsPartialFragment } from "../../../queries/openIdConnect.generated";
import EntityList from "../../entities/EntityList";
import OpenIdConnectAuthorizationSummary from "./OpenIdConnectAuthorizationSummary";
import EntityItem from "../../entities/EntityItem";

export default function OpenIdConnectAuthorizationList({
  loading,
  nodes,
}: {
  loading: boolean;
  nodes: OpenIdConnectAuthorizationsPartialFragment[] | null;
}) {
  return (
    <EntityList
      loading={loading}
      dataSource={nodes}
      renderItem={(node) => (
        <EntityItem>
          <OpenIdConnectAuthorizationSummary entity={node} />
        </EntityItem>
      )}
    />
  );
}
