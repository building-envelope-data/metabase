import { Fragment } from "react/jsx-runtime";

export default function InlineList<TItem>({
  items,
  renderItem,
}: {
  items: readonly TItem[];
  renderItem: (item: TItem, index: number) => React.ReactNode;
}) {
  return (
    <>
      {/* <style>{` */}
      {/*   .inline-list { */}
      {/*     & > *:not(:last-child)::after { content: ", "; } */}
      {/*     & > *:first-child:nth-last-child(2)::after { content: " and "; } */}
      {/*     & > *:nth-last-child(2):not(:first-child)::after { content: ", and "; } */}
      {/*   } */}
      {/* `}</style> */}
      <span className="inline-list">
        {items.map((item, index) => {
          const isLast = index === items.length - 1;
          const isSecondToLast = index === items.length - 2;
          const isOnlyTwo = items.length === 2;
          return (
            <Fragment key={index}>
              {renderItem(item, index)}
              {!isLast &&
                (isSecondToLast ? (isOnlyTwo ? " and " : ", and ") : ", ")}
            </Fragment>
          );
        })}
      </span>
    </>
  );
}
