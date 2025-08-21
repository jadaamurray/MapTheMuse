// src/features/media/components/MediaRail.jsx
import { Box, Card, CardContent, CardMedia, Chip, Typography } from "@mui/material";

/**
 * Pinterest-style horizontal rail with scroll-snap.
 * Expects items: [{ linkId, title, year?, creator?, posterUrl?, contextNote?, mediaType }]
 */
export default function MediaRail({ items = [] }) {
  if (!items?.length) return null;

  return (
    <Box
      sx={{
        position: "relative",
        "--gap": "16px",
        display: "grid",
        gridAutoFlow: "column",
        gridAutoColumns: { xs: "78%", sm: "46%", md: "30%", lg: "22%" },
        gap: "var(--gap)",
        overflowX: "auto",
        pb: 1,
        scrollSnapType: "x mandatory",
        scrollPadding: "var(--gap)",
        "& > *": { scrollSnapAlign: "start" },
        scrollbarWidth: "thin",
        "&::-webkit-scrollbar": { height: 8 },
        "&::-webkit-scrollbar-thumb": {
          backgroundColor: (t) => t.palette.action.hover,
          borderRadius: 999,
        },
      }}
    >
      {items.map((m) => (
        <Card
          key={m.linkId ?? `${m.title}-${m.externalId ?? ""}`}
          sx={{
            borderRadius: 3,
            overflow: "hidden",
            transition: "transform .15s ease, box-shadow .15s ease",
            ":hover": { transform: "translateY(-2px)", boxShadow: 4 },
          }}
        >
          <Box sx={{ position: "relative" }}>
            <CardMedia
              component="img"
              image={m.posterPath || "/placeholder/cover-gradient.jpg"}
              alt={m.title}
              sx={{ aspectRatio: "2/3", objectFit: "cover" }}
            />
            <Chip
              size="small"
              label={m.year ?? m.mediaType}
              sx={{
                position: "absolute",
                top: 8,
                left: 8,
                bgcolor: "rgba(0,0,0,.55)",
                color: "common.white",
                borderRadius: 2,
              }}
            />
          </Box>
          <CardContent sx={{ pb: 2 }}>
            <Typography variant="subtitle1" sx={{ fontWeight: 700 }} noWrap title={m.title}>
              {m.title}
            </Typography>
            <Typography variant="caption" sx={{ color: "text.secondary" }} noWrap>
              {(m.creator ? `${m.creator} · ` : "") + (m.contextNote || m.mediaType)}
            </Typography>
          </CardContent>
        </Card>
      ))}
    </Box>
  );
}
