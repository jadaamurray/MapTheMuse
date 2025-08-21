import { Card, CardContent, CardMedia, Typography, Box, Chip } from "@mui/material";

/** media: { id, title, creator, mediaType, releaseDate, posterPath? } */
export default function MediaCard({ media }) {
  const year = media?.releaseDate
    ? String(media.releaseDate).slice(0, 4) // works with DateOnly serialised as "YYYY-MM-DD"
    : undefined;

  const poster = media?.posterPath; // optional – add when you enrich

  return (
    <Card sx={{ borderRadius: 3, overflow: "hidden" }}>
      <Box sx={{ position: "relative" }}>
        <CardMedia
          component="img"
          image={poster || "/placeholder/cover-gradient.jpg"}
          alt={media.title}
          sx={{ aspectRatio: "4/5", objectFit: "cover" }}
        />
        <Chip
          size="small"
          label={media.mediaType}
          sx={{
            position: "absolute", top: 8, left: 8,
            bgcolor: "rgba(0,0,0,0.6)", color: "white", borderRadius: 2
          }}
        />
      </Box>
      <CardContent sx={{ pb: 2 }}>
        <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
          {media.title}
        </Typography>
        <Typography variant="body2" sx={{ opacity: 0.8 }}>
          {media.creator}{year ? ` · ${year}` : ""}
        </Typography>
      </CardContent>
    </Card>
  );
}
