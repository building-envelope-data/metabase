import { Button, Space } from "antd";
import { RocketOutlined } from "@ant-design/icons";
import { print } from "graphql";
import { TypedDocumentNode } from "@apollo/client";
import CopyButton from "./CopyButton";

export default function QueryToolbar<TVariables>({
  query,
  variables,
}: {
  query: TypedDocumentNode<any, TVariables>;
  variables?: TVariables | null;
}) {
  const openInNitro = () => {
    const nitroUrl = new URL("/graphql/", window.location.origin);
    // const minifiedQuery = print(query)
    //   .replace(/#.*$/gm, "")
    //   .replace(/\s+/g, " ")
    //   .trim();
    // nitroUrl.searchParams.set("query", minifiedQuery);
    // if (variables != null) {
    //   nitroUrl.searchParams.set("variables", JSON.stringify(variables));
    // }
    window.open(nitroUrl.toString(), "_blank");
  };

  return (
    <Space>
      GraphQL
      <CopyButton type="default" size="medium" getText={() => print(query)}>
        Copy Query
      </CopyButton>
      {variables && (
        <CopyButton
          type="default"
          size="medium"
          getText={() => JSON.stringify(variables)}
        >
          Copy Variables
        </CopyButton>
      )}
      <Button type="link" icon={<RocketOutlined />} onClick={openInNitro}>
        Open Explorer
      </Button>
    </Space>
  );
}
