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

export function pluralize(count: number, noun: string, suffix = "s") {
  return `${count} ${noun}${count !== 1 ? suffix : ""}`;
}

export function pluralizeIrregular(
  count: number,
  singular: string,
  plural?: string,
) {
  const word = count === 1 ? singular : (plural ?? `${singular}s`);
  return `${count} ${word}`;
}
