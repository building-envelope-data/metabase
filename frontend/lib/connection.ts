interface PageInfo {
  hasNextPage: boolean;
  hasPreviousPage?: boolean;
  startCursor?: string | null;
  endCursor: string | null;
}

export interface Connection<TNode> {
  edges?: Array<{ cursor?: string; node: TNode }> | null;
  pageInfo: PageInfo;
  totalCount: number;
}
