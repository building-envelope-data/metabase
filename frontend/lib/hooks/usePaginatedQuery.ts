import { useState, useMemo, useEffect } from "react";
import { TypedDocumentNode } from "@apollo/client";
import { useQuery } from "@apollo/client/react";
import { Connection } from "../connection";
import {
  Fetching,
  initialPageSize,
  PaginationProps,
} from "../../components/Pagination";

type QueryData<TNode> = {
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
  nodes: TNode[] | null;
  paginationProps: PaginationProps;
} {
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(initialPageSize);
  const [afterCursors, setAfterCursors] = useState<(string | null)[]>([null]);
  const [fetching, setFetching] = useState<Fetching | null>(Fetching.INITIAL);
  const [hasLoadedOnce, setHasLoadedOnce] = useState(false);

  const variables = {
    first: pageSize,
    after: afterCursors[currentPage - 1],
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

  if (!hasLoadedOnce && !loading) {
    setFetching(null);
    setHasLoadedOnce(true);
  }

  // Reset: If where OR order change, jump back to page 1
  useEffect(() => {
    setCurrentPage(1);
    setAfterCursors([null]);
    onQueryVariablesChange(variables);
  }, [JSON.stringify(where), JSON.stringify(order)]);

  const connection = data?.connection;
  const edges = connection?.edges;

  // window the current page from the cache
  const currentPageNodes = useMemo(() => {
    const start = (currentPage - 1) * pageSize;
    return edges?.slice(start, start + pageSize).map((e) => e.node);
  }, [edges, currentPage, pageSize]);

  const handleReload = () => {
    if (loading) return;
    const variables = {
      first: pageSize,
      after: null,
      where: where,
      order: order,
    };
    setFetching(Fetching.INITIAL);
    refetch(variables)
      .then(() => {
        // note that `currentPage` or `afterCursors` may have changed when this
        // callback fires
        setCurrentPage(1);
        setAfterCursors([null]);
        onQueryVariablesChange(variables);
      })
      .catch((error) => {
        console.error("Reloading current page failed", error);
      })
      .finally(() => setFetching(null));
  };

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
    if (loading || edges == null) return;
    const isLastPageInMemory = currentPage * pageSize >= edges.length;
    if (isLastPageInMemory && connection?.pageInfo.hasNextPage) {
      const endCursor = connection?.pageInfo.endCursor ?? null;
      const variables = {
        first: pageSize,
        after: endCursor,
        where: where,
        order: order,
      };
      setFetching(Fetching.NEXT);
      fetchMore({ variables, errorPolicy: "ignore" })
        .then(() => {
          // do not use `currentPage` or `afterCursors` to determine the new
          // values because they may have changed when this callback fires
          setCurrentPage((previous) => previous + 1);
          setAfterCursors((previous) => [...previous, endCursor]);
          onQueryVariablesChange(variables);
        })
        .catch((error) => {
          console.error("Fetching next page failed", error);
        })
        .finally(() => setFetching(null));
    } else if (!isLastPageInMemory) {
      setCurrentPage(currentPage + 1);
      onQueryVariablesChange({
        ...variables,
        after: afterCursors[currentPage],
      });
    }
  };

  const handlePageSizeChange = (pageSize: number) => {
    if (loading) return;
    setPageSize(pageSize);
    const variables = {
      first: pageSize,
      after: null,
      where: where,
      order: order,
    };
    setFetching(Fetching.INITIAL);
    refetch(variables)
      .then(() => {
        // note that `currentPage` or `afterCursors` may have changed when this
        // callback fires
        setCurrentPage(1);
        setAfterCursors([null]);
        onQueryVariablesChange(variables);
      })
      .catch((error) => {
        console.error("Changing page size failed", error);
      })
      .finally(() => setFetching(null));
  };

  return {
    loading,
    nodes: currentPageNodes ?? null,
    paginationProps: {
      fetching,
      current: currentPage,
      total: Math.ceil((connection?.totalCount ?? 0) / pageSize),
      pageSize: pageSize,
      hasNext:
        !!connection?.pageInfo.hasNextPage ||
        currentPage * pageSize < (edges?.length ?? 0),
      hasPrevious: currentPage > 1,
      onReload: handleReload,
      onNext: handleNext,
      onPrevious: handlePrevious,
      onPageSizeChange: handlePageSizeChange,
    },
  };
}
