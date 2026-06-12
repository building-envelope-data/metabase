import {
  FloatFilterInput,
  IntFilterInput,
  StringFilterInput,
  UrlFilterInput,
  UuidFilterInput,
} from "../__generated__/graphql";

type DistributiveOmit<T, K extends keyof any> = T extends any
  ? Omit<T, K>
  : never;

type Scalar = string | number | boolean;

export type EnumFilterInput<TEnum> = {
  equalTo?: TEnum | null;
  in?: TEnum[] | null;
  notEqualTo?: TEnum | null;
  notIn?: TEnum[] | null;
};

export type ListFilterInput<TItemFilterInput = any> = {
  all?: TItemFilterInput | null;
  none?: TItemFilterInput | null;
  some?: TItemFilterInput | null;
};

type FilterOperatorKind = "single" | "multiple";

const ScalarFilterOperatorMap = {
  contains: { label: "contains", kind: "single" },
  startsWith: { label: "starts with", kind: "single" },
  endsWith: { label: "ends with", kind: "single" },
  equalTo: { label: "=", kind: "single" },
  notEqualTo: { label: "≠", kind: "single" },
  greaterThan: { label: ">", kind: "single" },
  greaterThanOrEqualTo: { label: "≥", kind: "single" },
  lessThan: { label: "<", kind: "single" },
  lessThanOrEqualTo: { label: "≤", kind: "single" },
  in: { label: "∈", kind: "multiple" },
  notIn: { label: "∉", kind: "multiple" },
} as const satisfies Record<
  string,
  { label: string; kind: FilterOperatorKind }
>;

const ListFilterOperatorMap = {
  all: { label: "all", kind: "single" },
  none: { label: "none", kind: "single" },
  some: { label: "some", kind: "single" },
} as const satisfies Record<
  string,
  { label: string; kind: FilterOperatorKind }
>;

const FilterOperatorMap = {
  ...ScalarFilterOperatorMap,
  ...ListFilterOperatorMap,
} as const;

type ScalarFilterOperator = keyof typeof ScalarFilterOperatorMap;
type ListFilterOperator = keyof typeof ListFilterOperatorMap;
export type FilterOperator = ScalarFilterOperator | ListFilterOperator;

export const getFilterOperatorLabel = (key: FilterOperator) =>
  FilterOperatorMap[key].label;
export const isFilterOperatorSingle = (key: FilterOperator) =>
  FilterOperatorMap[key].kind === "single";

const scalarFilterOperators = new Set(
  Object.keys(ScalarFilterOperatorMap),
) as ReadonlySet<ScalarFilterOperator>;
const listFilterOperators = new Set(
  Object.keys(ListFilterOperatorMap),
) as ReadonlySet<ListFilterOperator>;

const isScalarFilterOperator = (
  value: FilterOperator | undefined,
): value is ScalarFilterOperator => {
  return scalarFilterOperators.has(value as ScalarFilterOperator);
};
const isListFilterOperator = (
  value: FilterOperator | undefined,
): value is ListFilterOperator => {
  return listFilterOperators.has(value as ListFilterOperator);
};

const defineFilterOperators = <K extends FilterOperator>(
  keys: readonly K[],
): readonly K[] => keys;

type ScalarFilterState = {
  readonly operator: ScalarFilterOperator;
  readonly value: Scalar | Scalar[] | null;
};
type ListFilterState = {
  readonly operator: ListFilterOperator;
  readonly value: FilterState;
};
export type ObjectFilterState = {
  readonly operator?: undefined;
  readonly index: number;
  readonly value: FilterState;
};

type FilterState = ScalarFilterState | ListFilterState | ObjectFilterState;

const ScalarFilterTypeMap = {
  enum: {
    operators: defineFilterOperators(["equalTo", "notEqualTo", "in", "notIn"]),
    initialOperator: 0,
  },
  float: {
    operators: defineFilterOperators([
      "equalTo",
      "notEqualTo",
      "greaterThan",
      "greaterThanOrEqualTo",
      "lessThan",
      "lessThanOrEqualTo",
      "in",
      "notIn",
    ]),
    initialOperator: 0,
  },
  int: {
    operators: defineFilterOperators([
      "equalTo",
      "notEqualTo",
      "greaterThan",
      "greaterThanOrEqualTo",
      "lessThan",
      "lessThanOrEqualTo",
      "in",
      "notIn",
    ]),
    initialOperator: 0,
  },
  string: {
    operators: defineFilterOperators([
      "contains",
      "startsWith",
      "endsWith",
      "equalTo",
      "notEqualTo",
      "in",
      "notIn",
    ]),
    initialOperator: 0,
  },
  url: {
    operators: defineFilterOperators(["equalTo", "notEqualTo", "in", "notIn"]),
    initialOperator: 0,
  },
  uuid: {
    operators: defineFilterOperators(["equalTo", "notEqualTo", "in", "notIn"]),
    initialOperator: 0,
  },
} as const satisfies Record<
  string,
  { operators: readonly ScalarFilterOperator[]; initialOperator: number }
>;

const ListFilterTypeMap = {
  list: {
    operators: defineFilterOperators(["some", "all", "none"]),
    initialOperator: 0,
  },
} as const satisfies Record<
  string,
  { operators: readonly ListFilterOperator[]; initialOperator: number }
>;

export const FilterTypeMap = {
  ...ScalarFilterTypeMap,
  ...ListFilterTypeMap,
} as const;

type ScalarFilterType = keyof typeof ScalarFilterTypeMap;
type ListFilterType = keyof typeof ListFilterTypeMap;
type ObjectFilterType = typeof objectFilterType;
type FilterType = ScalarFilterType | ListFilterType | ObjectFilterType;

const scalarFilterTypes = new Set(
  Object.keys(ScalarFilterTypeMap),
) as ReadonlySet<ScalarFilterType>;
const listFilterTypes = new Set(
  Object.keys(ListFilterTypeMap),
) as ReadonlySet<ListFilterType>;
const objectFilterType = "object" as const;

const isScalarFilterType = (value: FilterType): value is ScalarFilterType => {
  return scalarFilterTypes.has(value as ScalarFilterType);
};
const isListFilterType = (value: FilterType): value is ListFilterType => {
  return listFilterTypes.has(value as ListFilterType);
};

type BaseFilterDefinition<TKey> = {
  readonly field: TKey;
  readonly label?: string;
};

type EnumFilterDefinition<
  TFilterInput,
  TKey extends keyof TFilterInput,
  TEnum,
> = BaseFilterDefinition<TKey> & {
  readonly type: "enum" & ScalarFilterType;
  readonly field: TKey;
  readonly enumObject: TEnum;
  readonly _validation?: TFilterInput[TKey] extends EnumFilterInput<TEnum>
    ? TKey
    : never;
};

type FloatFilterDefinition<
  TFilterInput,
  TKey extends keyof TFilterInput,
> = BaseFilterDefinition<TKey> & {
  readonly type: "float" & ScalarFilterType;
  readonly _validation?: TFilterInput[TKey] extends FloatFilterInput
    ? TKey
    : never;
};

type IntFilterDefinition<
  TFilterInput,
  TKey extends keyof TFilterInput,
> = BaseFilterDefinition<TKey> & {
  readonly type: "int" & ScalarFilterType;
  readonly _validation?: TFilterInput[TKey] extends IntFilterInput
    ? TKey
    : never;
};

type StringFilterDefinition<
  TFilterInput,
  TKey extends keyof TFilterInput,
> = BaseFilterDefinition<TKey> & {
  readonly type: "string" & ScalarFilterType;
  readonly _validation?: TFilterInput[TKey] extends StringFilterInput
    ? TKey
    : never;
};

type UrlFilterDefinition<
  TFilterInput,
  TKey extends keyof TFilterInput,
> = BaseFilterDefinition<TKey> & {
  readonly type: "url" & ScalarFilterType;
  readonly _validation?: TFilterInput[TKey] extends UrlFilterInput
    ? TKey
    : never;
};

type UuidFilterDefinition<
  TFilterInput,
  TKey extends keyof TFilterInput,
> = BaseFilterDefinition<TKey> & {
  readonly type: "uuid" & ScalarFilterType;
  readonly _validation?: TFilterInput[TKey] extends UuidFilterInput
    ? TKey
    : never;
};

type ScalarFilterDefinitionMap<
  TFilterInput,
  TKey extends keyof TFilterInput,
> = {
  enum: TFilterInput[TKey] extends EnumFilterInput<infer TEnum>
    ? EnumFilterDefinition<TFilterInput, TKey, TEnum>
    : never;
  float: FloatFilterDefinition<TFilterInput, TKey>;
  int: IntFilterDefinition<TFilterInput, TKey>;
  string: StringFilterDefinition<TFilterInput, TKey>;
  url: UrlFilterDefinition<TFilterInput, TKey>;
  uuid: UuidFilterDefinition<TFilterInput, TKey>;
};

type ScalarFilterDefinition<
  TFilterInput,
  TKey extends keyof TFilterInput,
> = ScalarFilterDefinitionMap<TFilterInput, TKey>[ScalarFilterType];

type ListItemFilterDefinition<TFilterInput, TKey extends keyof TFilterInput> =
  NonNullable<TFilterInput[TKey]> extends {
    all?: infer TItemFilterInput;
  }
    ? DistributiveOmit<FilterDefinition<NonNullable<TItemFilterInput>>, "field">
    : never;

export type ListFilterDefinition<
  TFilterInput,
  TKey extends keyof TFilterInput,
> = BaseFilterDefinition<TKey> & {
  readonly type: "list" & ListFilterType;
  readonly item: ListItemFilterDefinition<TFilterInput, TKey>;
  readonly _validation?: TFilterInput extends ListFilterInput ? TKey : never;
};

export type ObjectFilterDefinition<
  TFilterInput,
  TKey extends keyof TFilterInput,
> = BaseFilterDefinition<TKey> & {
  readonly type: "object" & ObjectFilterType;
  readonly items: readonly FilterDefinition<NonNullable<TFilterInput[TKey]>>[];
};

type FieldFilterDefinition<TFilterInput, TKey extends keyof TFilterInput> =
  | ScalarFilterDefinition<TFilterInput, TKey>
  | ListFilterDefinition<TFilterInput, TKey>
  | ObjectFilterDefinition<TFilterInput, TKey>;

export type AnyFilterDefinition = FieldFilterDefinition<any, any>;

export type FilterDefinition<TFilterInput> = {
  [TKey in keyof TFilterInput]: FieldFilterDefinition<TFilterInput, TKey>;
}[keyof TFilterInput];

const isScalarFilterDefinition = <TFilterInput>(
  value: FilterDefinition<TFilterInput>,
): value is ScalarFilterDefinition<TFilterInput, keyof TFilterInput> => {
  return isScalarFilterType(value.type);
};
const isListFilterDefinition = <TFilterInput>(
  value: FilterDefinition<TFilterInput>,
): value is ListFilterDefinition<TFilterInput, keyof TFilterInput> => {
  return isListFilterType(value.type);
};
const isObjectFilterDefinition = <TFilterInput>(
  value: FilterDefinition<TFilterInput>,
): value is ObjectFilterDefinition<TFilterInput, keyof TFilterInput> => {
  return value.type === objectFilterType;
};

const isScalarFilterState = (
  value: FilterState,
): value is ScalarFilterState => {
  return isScalarFilterOperator(value.operator);
};
const isListFilterState = (value: FilterState): value is ListFilterState => {
  return isListFilterOperator(value.operator);
};
const isObjectFilterState = (
  value: FilterState,
): value is ObjectFilterState => {
  return value.operator === undefined;
};

export const createFilterStateReducer = <TOutput>(
  reduceScalar: (
    scalar: ScalarFilterState,
    context: ScalarFilterDefinition<any, any>,
  ) => TOutput,
  reduceList: (
    list: ListFilterState,
    context: ListFilterDefinition<any, any>,
    reduce: () => TOutput,
  ) => TOutput,
  reduceObject: (
    object: ObjectFilterState,
    context: ObjectFilterDefinition<any, any>,
    reduce: () => TOutput,
  ) => TOutput,
) => {
  return (
    root: ObjectFilterState,
    rootFilterDefinitions: readonly AnyFilterDefinition[],
  ): TOutput => {
    const reduce = (
      filter: FilterState,
      context: AnyFilterDefinition,
    ): TOutput => {
      if (isScalarFilterState(filter)) {
        if (context.type == "list" || context.type == "object") {
          console.error("Filter: ", filter, "Context: ", context);
          throw Error(`Expected scalar filter definition, got ${context.type}`);
        }
        return reduceScalar(filter, context);
      }
      if (isListFilterState(filter)) {
        if (context.type != "list") {
          console.error("Filter: ", filter, "Context: ", context);
          throw Error(`Expected list filter definition, got ${context.type}`);
        }
        return reduceList(filter, context, () =>
          reduce(filter.value, context.item),
        );
      }
      if (isObjectFilterState(filter)) {
        if (context.type != "object") {
          console.error("Filter: ", filter, "Context: ", context);
          throw Error(`Expected object filter definition, got ${context.type}`);
        }
        return reduceObject(filter, context, () =>
          reduce(filter.value, context.items[filter.index]),
        );
      }
      return assertNever(filter);
    };
    const rootFilterDefinition: ObjectFilterDefinition<any, any> = {
      type: "object",
      field: "root",
      items: rootFilterDefinitions as FilterDefinition<any>[],
    };
    return reduceObject(root, rootFilterDefinition, () =>
      reduce(root.value, rootFilterDefinition.items[root.index]),
    );
  };
};

export const createFilterDefinitionReducer = <
  TScalarOutput,
  TListOutput,
  TObjectOutput,
>(
  reduceScalar: (scalar: ScalarFilterDefinition<any, any>) => TScalarOutput,
  reduceList: (
    list: ListFilterDefinition<any, any>,
    reduceValue: (
      filter: AnyFilterDefinition,
    ) => TScalarOutput | TListOutput | TObjectOutput,
  ) => TListOutput,
  reduceObject: (
    object: ObjectFilterDefinition<any, any>,
    reduceValue: (
      filter: AnyFilterDefinition,
    ) => TScalarOutput | TListOutput | TObjectOutput,
  ) => TObjectOutput,
) => {
  return (
    root: AnyFilterDefinition,
  ): TScalarOutput | TListOutput | TObjectOutput => {
    const reduce = (
      filter: AnyFilterDefinition,
    ): TScalarOutput | TListOutput | TObjectOutput => {
      if (isScalarFilterDefinition(filter)) {
        return reduceScalar(filter);
      }
      if (isListFilterDefinition(filter)) {
        return reduceList(filter, (x) => reduce(x));
      }
      if (isObjectFilterDefinition(filter)) {
        return reduceObject(filter, (x) => reduce(x));
      }
      return null as TScalarOutput | TListOutput | TObjectOutput;
      // TODO make the following succeed: return assertNever(filter);
    };
    return reduce(root);
  };
};

// to GraphQL filter input, for example, to `{ name: { contains: "Simon" } }`
export const toWhereClause = <TFilterInput>(
  root: ObjectFilterState,
  rootFilterDefinitions: readonly FilterDefinition<TFilterInput>[],
): TFilterInput =>
  // the initial context type is `readonly FilterDefinition<TFilterInput>[]` and subsequent
  // types are of the form `readonly FilterDefinition<TFilterInput[keyof TFilterInput]>[]`
  createFilterStateReducer<any>(
    (scalar, _) => ({
      [scalar.operator]: scalar.value,
    }),
    (list, _, reduceValue) => ({
      [list.operator]: reduceValue(),
    }),
    (object, context, reduceValue) => ({
      [context.items[object.index].field]: reduceValue(),
    }),
  )(root, rootFilterDefinitions as AnyFilterDefinition[]) as TFilterInput;

export const getInitialFilterSubformValues = (
  filterDefinition: AnyFilterDefinition,
): FilterState =>
  createFilterDefinitionReducer<
    ScalarFilterState,
    ListFilterState,
    ObjectFilterState
  >(
    (scalar) => ({
      operator:
        ScalarFilterTypeMap[scalar.type].operators[
          ScalarFilterTypeMap[scalar.type].initialOperator
        ],
      value: null,
    }),
    (list, reduce) => ({
      operator:
        ListFilterTypeMap[list.type].operators[
          ListFilterTypeMap[list.type].initialOperator
        ],
      value: reduce(list.item),
    }),
    (object, reduce) => ({
      index: 0,
      value: reduce(object.items[0]),
    }),
  )(filterDefinition);
