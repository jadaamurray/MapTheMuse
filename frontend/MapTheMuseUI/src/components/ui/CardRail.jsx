// CardRail.jsx
import React, { useRef } from "react";
import { Box, IconButton } from "@mui/material";
import ArrowBackIosNew from "@mui/icons-material/ArrowBackIosNew";
import ArrowForwardIos from "@mui/icons-material/ArrowForwardIos";
import { useCardWheel } from "../../hooks/useCardWheel";

export default function CardRail({
    children,
    gap = 5,
    px = 10,
    py = 9,
    showArrows = true,
    snap = "start", // 'start' | 'center'
    cardWidth,      // optional: if provided, scrolls exactly one card per click
    sx = {},
}) {
    const ref = useRef(null);
    
    useCardWheel(ref, { cardWidth });


    const scrollBy = (dir) => {
        const el = ref.current;
        if (!el) return;
        const amount = cardWidth ?? el.clientWidth * 0.9; // near a full view
        el.scrollBy({ left: dir * amount, behavior: "smooth" });
    };

    return (
        <Box sx={{ position: "relative", width: "100%", maxWidth: "100vw", overflow: "hidden", ...sx }}>
            {showArrows && (
                <>
                    <IconButton
                        onClick={() => scrollBy(-1)}
                        size="small"
                        sx={{
                            position: "absolute", left: 8, top: "50%", transform: "translateY(-50%)",
                            zIndex: 1, bgcolor: "background.paper", boxShadow: 1, "&:hover": { bgcolor: "background.paper" }
                        }}
                    >
                        <ArrowBackIosNew fontSize="small" />
                    </IconButton>
                    <IconButton
                        onClick={() => scrollBy(1)}
                        size="small"
                        sx={{
                            position: "absolute", right: 8, top: "50%", transform: "translateY(-50%)",
                            zIndex: 1, bgcolor: "background.paper", boxShadow: 1, "&:hover": { bgcolor: "background.paper" }
                        }}
                    >
                        <ArrowForwardIos fontSize="small" />
                    </IconButton>
                </>
            )}

            <Box
                ref={ref}
                sx={{
                    display: "flex",
                    gap,
                    px,
                    py,
                    overflowX: "auto",
                    overflowY: "hidden",
                    scrollSnapType: "x mandatory",
                    scrollBehavior: "smooth",
                    touchAction: "pan-x",
                    overscrollBehavior: "none",
                    overscrollBehaviorX: "contain",
                    WebkitOverflowScrolling: "touch",
                    "&::-webkit-scrollbar": { height: 0 },
                    scrollbarWidth: "none",
                    msOverflowStyle: "none",
                    scrollPaddingLeft: px,
                    scrollPaddingRight: px,
                    "& > *": {
                        flex: "0 0 auto",
                        scrollSnapAlign: snap,
                        scrollSnapStop: "always",
                    },
                }}
            >
                {children}
            </Box>
        </Box>
    );
}
