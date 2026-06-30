export const uuidRegex =
  /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;

export const isUuid = <T extends string>(value: T) => uuidRegex.test(value);

export const notEmpty = <T extends string>(value: T) => value.trim().length > 0;

export const capitalize = <T extends string>(value: T) =>
  (value[0].toUpperCase() + value.slice(1)) as Capitalize<T>;

export const replacePrefix = (
  value: string,
  oldPrefix: string,
  newPrefix: string = "",
): string =>
  value.startsWith(oldPrefix)
    ? newPrefix + value.slice(oldPrefix.length)
    : value;

export const removePrefix = (value: string, prefix: string): string =>
  value.startsWith(prefix) ? value.slice(prefix.length) : value;

export const addPrefix = (value: string, prefix: string): string =>
  value.startsWith(prefix) ? value : prefix + value;

const ACRONYMS = ["ID", "URL", "UUID"];
type CaseStyle = "all-upper" | "first-upper" | "none-upper";

export const humanize = (
  key: PropertyKey,
  style: CaseStyle = "none-upper",
): string => {
  if (!key) return "";
  const words = String(key)
    .replace(/([a-z])([A-Z])/g, "$1 $2")
    .replace(/[_-]/g, " ")
    .trim()
    .split(/\s+/); // Get individual words
  const formattedWords = words.map((word, index) => {
    const upper = word.toUpperCase();
    if (ACRONYMS.includes(upper)) {
      return upper;
    }
    switch (style) {
      case "all-upper": // title case
        return word.charAt(0).toUpperCase() + word.slice(1).toLowerCase();
      case "first-upper": // sentence case
        return index === 0
          ? word.charAt(0).toUpperCase() + word.slice(1).toLowerCase()
          : word.toLowerCase();
      case "none-upper": // lowercase
        return word.toLowerCase();
      default:
        return word;
    }
  });
  return formattedWords.join(" ");
};

export const getLabel = (
  {
    field,
    label,
  }: {
    field: PropertyKey;
    label?: string;
  },
  style: CaseStyle = "none-upper",
) => label ?? humanize(field, style);
