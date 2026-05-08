import { useState, useMemo, useEffect } from "react";
import { TypedDocumentNode } from "@apollo/client";
import { useQuery } from "@apollo/client/react";
import { Connection } from "../connection";
import { initialPageSize, PaginationProps } from "../../components/Pagination";

export type QueryData<TNode> = {
  connection: Connection<TNode> | null;
};

type FilterInput<TFilterInput> = TFilterInput | { and?: TFilterInput[] };

type SortInput<TSortInput> = TSortInput | TSortInput[];

export type QueryVariables<TFilterInput, TSortInput> = {
  first: number;
  after?: string | null;
  where?: FilterInput<TFilterInput>;
  order?: SortInput<TSortInput>;
};

export type QueryDocument<TNode, TFilterInput, TSortInput> = TypedDocumentNode<
  QueryData<TNode>,
  QueryVariables<TFilterInput, TSortInput>
>;

// ensure `relayStylePagination(["first", "after", ...])` is set in `InMemoryCache` for the field `fieldName`
export function usePaginatedQuery<TNode, TFilterInput, TSortInput>(
  queryDocument: QueryDocument<TNode, TFilterInput, TSortInput>,
  {
    where,
    order,
  }: {
    where?: FilterInput<TFilterInput>;
    order?: SortInput<TSortInput>;
  },
  onQueryVariablesChange: (
    variables: QueryVariables<TFilterInput, TSortInput>,
  ) => void,
): {
  loading: boolean;
  nodes: TNode[];
  paginationProps: PaginationProps;
} {
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(initialPageSize);
  const [afterCursors, setAfterCursors] = useState<(string | null)[]>([null]);

  const variables = {
    first: pageSize,
    after: null,
    where: where,
    order: order,
  };
  const { loading, error, data, fetchMore, refetch } = useQuery(queryDocument, {
    variables,
    notifyOnNetworkStatusChange: true,
    errorPolicy: "ignore",
  });
  if (error) {
    console.error("Paginated query failed", error);
  }

  // Reset: If where OR order change, jump back to page 1
  useEffect(() => {
    setCurrentPage(1);
    setAfterCursors([null]);
    onQueryVariablesChange(variables);
  }, [JSON.stringify(where), JSON.stringify(order)]);

  const connection = data?.connection;
  const edges = connection?.edges || [];

  // Window the current page from the cache
  const currentPageNodes = useMemo(() => {
    const start = (currentPage - 1) * pageSize;
    return edges.slice(start, start + pageSize).map((e) => e.node);
  }, [edges, currentPage, pageSize]);

  const handlePrevious = () => {
    if (loading) return;
    if (currentPage <= 1) return;
    const previousPage = currentPage - 1;
    setCurrentPage(previousPage);
    onQueryVariablesChange({
      ...variables,
      after: afterCursors[previousPage - 1],
    });
  };

  const handleNext = () => {
    if (loading) return;
    const isLastPageInMemory = currentPage * pageSize >= edges.length;
    if (isLastPageInMemory && connection?.pageInfo.hasNextPage) {
      const endCursor = connection?.pageInfo.endCursor ?? null;
      const variables = {
        first: pageSize,
        after: endCursor,
        where: where,
        order: order,
      };
      fetchMore({ variables, errorPolicy: "ignore" })
        .then(() => {
          setCurrentPage((previous) => previous + 1);
          setAfterCursors((previous) => [...previous, endCursor]);
          onQueryVariablesChange(variables);
        })
        .catch((error) => {
          console.error("Fetching next page failed", error);
        });
    } else if (!isLastPageInMemory) {
      setCurrentPage(currentPage + 1);
      onQueryVariablesChange({
        ...variables,
        after: afterCursors[currentPage],
      });
    }
  };

  const handlePageSizeChange = (pageSize: number) => {
    setPageSize(pageSize);
    const variables = {
      first: pageSize,
      after: null,
      where: where,
      order: order,
    };
    refetch(variables)
      .then(() => {
        setCurrentPage(1);
        setAfterCursors([null]);
        onQueryVariablesChange(variables);
      })
      .catch((error) => {
        console.error("Changing page size failed", error);
      });
  };

  return {
    loading,
    nodes: currentPageNodes,
    paginationProps: {
      current: currentPage,
      total: Math.ceil((connection?.totalCount ?? 0) / pageSize),
      pageSize: pageSize,
      hasNext:
        !!connection?.pageInfo.hasNextPage ||
        currentPage * pageSize < edges.length,
      hasPrevious: currentPage > 1,
      onNext: handleNext,
      onPrevious: handlePrevious,
      onPageSizeChange: handlePageSizeChange,
    },
  };
}
