// Taken from https://stackoverflow.com/questions/43118692/typescript-filter-out-nulls-from-an-array/46700791#46700791
export function notEmpty<TValue>(
  value: TValue | null | undefined,
): value is TValue {
  return value !== null && value !== undefined;
}

export function isTruthy<T>(
  value: T | false | null | undefined | "" | 0,
): value is T {
  return Boolean(value);
}

export function isMember<T extends string>(
  value: string,
  array: readonly T[],
): value is T {
  return array.includes(value as T);
}

export const intersperse = (
  array: React.ReactNode[],
  separator: string = " ",
): React.ReactNode[] =>
  array.reduce(
    (accumulator: React.ReactNode[], currrent, index) =>
      index === 0 ? [currrent] : [...accumulator, separator, currrent],
    [],
  );

/**
 * Unwraps T if T is an array, otherwise becomes `never`.
 * When applied to a union like `A[] | B[]`, it results in `A | B`.
 */
type UnboxArray<T> = T extends (infer U)[] ? U : never;

/**
 * Turns `A[] | B[] | C[]` into `readonly (A | B | C)[]` without runtime costs.
 */
export function asReadonlyMixed<T extends any[]>(
  array: T,
): readonly UnboxArray<T>[] {
  return array as any;
}

export const range = (start: number, end: number): number[] =>
  Array.from({ length: end - start + 1 }, (_, i) => start + i);

export function pluralize(count: number, noun: string, suffix = "s") {
  return `${noun}${count !== 1 ? suffix : ""}`;
}

export function pluralizeIrregular(
  count: number,
  singular: string,
  plural?: string,
) {
  return count === 1 ? singular : (plural ?? `${singular}s`);
}
