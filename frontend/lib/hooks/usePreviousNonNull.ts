import { useRef } from "react";

export function usePreviousNonNull<T>(value: T | null) {
  const ref = useRef<T | null>(null);
  if (value !== null) {
    ref.current = value;
  }
  return ref.current;
}
