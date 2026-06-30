export type Prettify<T> = {
  [K in keyof T]: T[K];
} & {};

export type PrettifyDeep<T> = {
  [K in keyof T]: T[K] extends object ? PrettifyDeep<T[K]> : T[K];
} & {};
