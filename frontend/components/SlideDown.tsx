import { useLayoutEffect, useRef, useState } from "react";

export default function SlideDown({
  open,
  children,
}: {
  open: boolean;
  children: React.ReactNode;
}) {
  const contentRef = useRef<HTMLDivElement>(null);
  const [height, setHeight] = useState<number | string>(0);
  const [transitioning, setTransitioning] = useState(false);

  useLayoutEffect(() => {
    const element = contentRef.current;
    if (!element) return;
    setTransitioning(true);
    const startHeight = open ? 0 : element.scrollHeight;
    const endHeight = open ? element.scrollHeight : 0;
    setHeight(startHeight);
    void element.offsetHeight; // force reflow
    requestAnimationFrame(() => {
      requestAnimationFrame(() => {
        setHeight(endHeight);
      });
    });
  }, [open]);

  const handleTransitionEnd = () => {
    setTransitioning(false);
    setHeight(open ? "auto" : 0);
  };

  if (!open && !transitioning) {
    return null;
  }

  return (
    <div
      onTransitionEnd={handleTransitionEnd}
      style={{
        height: height,
        pointerEvents: open ? "auto" : "none",
        overflow: "hidden",
        transition: "all 0.3s ease-in-out",
      }}
    >
      <div ref={contentRef}>{children}</div>
    </div>
  );
}
