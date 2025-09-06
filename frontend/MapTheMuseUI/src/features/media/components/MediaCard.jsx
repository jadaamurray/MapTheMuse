// src/features/media/components/MediaCard.jsx
import { Card, CardContent, CardMedia, Typography, Box, Chip } from "@mui/material";
import FavouriteMediaButton from "../../favourites/components/FavouriteMediaButton";

export default function MediaCard({
  item,
  showFavourite = true,
  showChip = true,
  elevation = 0,
  sx,
}) {
  const m = item ?? {};
  const poster =
    m.posterUrl || m.posterPath || "/placeholder/cover-gradient.jpg";
  const chipLabel = m.year ?? m.type ?? m.mediaType;

  return (
    <Card
      elevation={elevation}
      sx={{
        borderRadius: 3,
        overflow: "hidden",
        transition: "transform .15s ease, box-shadow .15s ease",
        ":hover": { transform: "translateY(-2px)", boxShadow: 4 },
        ...sx,
      }}
    >
      <Box sx={{ position: "relative" }}>
        <CardMedia
          component="img"
          image={poster}
          alt={m.title || m.externalId || "Poster"}
          sx={{ aspectRatio: "2/3", objectFit: "cover" }}
        />

        {/* Chip (top-left) */}
        {showChip && !!chipLabel && (
          <Chip
            size="small"
            label={chipLabel}
            sx={{
              position: "absolute",
              top: 8,
              left: 8,
              bgcolor: "rgba(0,0,0,.55)",
              color: "common.white",
              borderRadius: 2,
            }}
          />
        )}

        {/* Favourite (top-right) */}
        {showFavourite && (
          <Box sx={{ position: "absolute", top: 8, right: 8, zIndex: 1 }}>
            <FavouriteMediaButton
              source={m.source}
              type={m.type ?? m.mediaType}
              externalId={m.externalId}
              mediaId={m.mediaId}
              title={m.title ?? null}
              posterPath={m.posterPath ?? m.posterUrl ?? null}
              size="small"
            />
          </Box>
        )}
      </Box>

      <CardContent sx={{ pb: 2 }}>
        <Typography variant="subtitle1" sx={{ fontWeight: 700 }} noWrap title={m.title}>
          {m.title ?? m.externalId}
        </Typography>
        {(m.creator || m.contextNote || m.type || m.mediaType) && (
          <Typography variant="caption" sx={{ color: "text.secondary" }} noWrap>
            {(m.creator ? `${m.creator} · ` : "") + (m.contextNote || m.type || m.mediaType)}
          </Typography>
        )}
      </CardContent>
    </Card>
  );
}
