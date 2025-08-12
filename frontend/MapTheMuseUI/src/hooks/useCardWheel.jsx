import { useEffect } from "react";

export function useCardWheel(ref, { cardWidth } = {}) {
  useEffect(() => {
    const el = ref.current;
    if (!el) return;

    const getStep = () => {
      if (cardWidth) return cardWidth;
      const first = el.querySelector(":scope > *");
      const rect = first?.getBoundingClientRect();
      const cs = getComputedStyle(el);
      const gapPx = parseFloat(cs.columnGap || cs.gap || "0");
      return (rect?.width || el.clientWidth * 0.9) + gapPx;
    };

    const onWheel = (e) => {
      const dx = Math.abs(e.deltaY) > Math.abs(e.deltaX) ? e.deltaY : e.deltaX;
      if (!dx) return;

      e.preventDefault();
      e.stopPropagation();

      const step = getStep();
      el.scrollBy({ left: Math.sign(dx) * step, behavior: "smooth" });
    };

    el.addEventListener("wheel", onWheel, { passive: false });
    return () => el.removeEventListener("wheel", onWheel, { passive: false });
  }, [ref, cardWidth]);
}
