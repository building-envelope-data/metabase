import { SortEnumType } from "../__generated__/graphql";

export type SortState = {
  index: number;
  direction: SortEnumType;
};

export type SortDefinition<TSortInput> = {
  field: keyof TSortInput;
  label?: string;
};

export const initialSortSubformValues = {
  index: 0,
  direction: SortEnumType.Asc,
};

// to GraphQL sort input, for example, to `[{ name: "ASC" }, { date: "DESC" }]`
export const toOrderClause = <TSortInput>(
  sort: SortState,
  configs: readonly SortDefinition<TSortInput>[],
): TSortInput =>
  ({
    [configs[sort.index].field]: sort.direction,
  }) as TSortInput;

export const formatSortDirection = (direction: SortEnumType) => {
  switch (direction) {
    case SortEnumType.Asc:
      return "ascending";
    case SortEnumType.Desc:
      return "descending";
    default:
      return assertNever(direction);
  }
};
