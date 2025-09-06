import { useEffect } from "react";

export function useCardWheel(ref, { cardWidth, step } = {}) {
  useEffect(() => {
    const el = ref.current;
    if (!el) return;

    const getStep = () => {
      if (cardWidth) return cardWidth;
      if (step) return step;

      const first = el.querySelector(":scope > *");
      const rect = first?.getBoundingClientRect();
      const cs = el && getComputedStyle(el);
      const gapPx = parseFloat(cs?.columnGap || cs?.gap || "0") || 0;
      return (rect?.width || el.clientWidth * 0.9) + gapPx;
    };

    const onWheel = (e) => {
      const primaryDelta =
        Math.abs(e.deltaY) > Math.abs(e.deltaX) ? e.deltaY : e.deltaX;
      if (!primaryDelta) return;

      const dir = Math.sign(primaryDelta); // 1 = right, -1 = left
      const maxLeft = el.scrollWidth - el.clientWidth;
      const atStart = el.scrollLeft <= 0;
      const atEnd = el.scrollLeft >= maxLeft - 1;

      const canScroll =
        (dir < 0 && !atStart) || (dir > 0 && !atEnd);

      if (!canScroll) {
        // let the page scroll normally
        return;
      }

      e.preventDefault();
      e.stopPropagation();
      el.scrollBy({ left: dir * getStep(), behavior: "smooth" });
    };

    el.addEventListener("wheel", onWheel, { passive: false });
    return () => el.removeEventListener("wheel", onWheel, { passive: false });
  }, [ref, cardWidth, step]);
}
