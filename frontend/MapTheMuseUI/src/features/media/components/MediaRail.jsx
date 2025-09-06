import { Box } from "@mui/material";
import MediaCard from "./MediaCard";

/**
 * Pinterest-style horizontal rail with scroll-snap.
 * Expects items: [{ linkId, title, year?, creator?, posterUrl?, posterPath?, contextNote?, type?|mediaType?, source, externalId, mediaId? }]
 */
export default function MediaRail({ items = [], cardProps }) {
  if (!items?.length) return null;

  return (
    <Box
      sx={{
        position: "relative",
        "--gap": "16px",
        display: "grid",
        gridAutoFlow: "column",
        gridAutoColumns: { xs: "48%", sm: "46%", md: "30%", lg: "22%" },
        gap: "var(--gap)",
        overflowX: "auto",
        pb: 1,
        scrollSnapType: "x mandatory",
        scrollPadding: "var(--gap)",
        "& > *": { scrollSnapAlign: "start" },
        scrollbarWidth: "thin",
        "&::-webkit-scrollbar": { height: 8 },
        "&::-webkit-scrollbar-thumb": (t) => ({
          backgroundColor: t.palette.action.hover,
          borderRadius: 999,
        }),
      }}
    >
      {items.map((m) => (
        <MediaCard
          key={m.linkId ?? `${m.title}-${m.externalId ?? ""}`}
          item={m}
          {...cardProps}
        />
      ))}
    </Box>
  );
}
