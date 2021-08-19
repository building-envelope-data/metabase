import { forwardRef } from "react";
import Highlighter from "react-highlight-words";

export type HighlightProps = {
  text: string | null | undefined;
  snippet: string | null | undefined;
};

export const Highlight = forwardRef<Highlighter, HighlightProps>(
  ({ text, snippet }, ref) => (
    <Highlighter
      ref={ref}
      highlightStyle={{ backgroundColor: "#ffc069", padding: 0 }}
      searchWords={snippet ? [snippet] : []}
      autoEscape
      textToHighlight={text || ""}
    />
  )
);
