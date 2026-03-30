import { TypedDocumentNode } from "@apollo/client";
import { useQuery } from "@apollo/client/react";
import { Connection } from "../connection";

// ensure `relayStylePagination(["first", "after", ...])` is set in `InMemoryCache` for the field `fieldName`
export function useRelayInfiniteScroll<
  TNode,
  TData extends Record<TFieldName, Connection<TNode>>,
  TVariables extends {
    first?: number | null;
    after?: string | null;
    where?: any;
    order?: any;
  },
  TFieldName extends keyof TData,
>(
  queryDocument: TypedDocumentNode<TData, TVariables>,
  {
    pageSize,
    fieldName,
    where,
    order,
  }: {
    pageSize: number;
    fieldName: TFieldName;
    where: TVariables["where"];
    order: TVariables["order"];
  },
) {
  const { data, loading, fetchMore } = useQuery<TData, TVariables>(
    queryDocument,
    {
      variables: {
        first: pageSize,
        after: null,
        where: where,
        order: order, // HotChocolate naming convention
      } as TVariables,
      notifyOnNetworkStatusChange: true,
    },
  );

  const connection = data?.[fieldName];
  const hasMore = !!connection?.pageInfo.hasNextPage;

  const loadMore = () => {
    if (hasMore && !loading) {
      fetchMore({
        variables: { after: connection.pageInfo.endCursor } as TVariables,
      });
    }
  };

  return {
    edges: connection?.edges || [],
    nodes: connection?.edges?.map((e: any) => e.node) || [],
    loading,
    hasMore,
    loadMore,
    loadMoreButtonProps: {
      loading,
      onClick: loadMore,
      disabled: !hasMore,
    },
  };
}
