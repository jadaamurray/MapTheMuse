import { useCallback, useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";
import MediaSearchBar from "../components/MediaSearchBar";
import MediaCard from "../components/MediaCard";
import { searchMedia } from "../services/mediaService";

/**
 * Masonry via CSS columns (no extra deps).
 * If you prefer @mui/lab/Masonry, we can swap later.
 */
export default function MediaPage() {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(false);

  const handleSearch = useCallback(async (filters = {}) => {
    try {
      setLoading(true);
      const data = await searchMedia(filters);
      // Expect either an array or a {items:[], total:...} – adjust as needed
      setItems(Array.isArray(data) ? data : data.items ?? []);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    handleSearch({});
  }, [handleSearch]);

  return (
    <Box sx={{ maxWidth: 1280, mx: "auto", px: { xs: 2, md: 3 }, py: { xs: 3, md: 4 } }}>
      <Typography variant="h4" sx={{ fontWeight: 800, mb: 2 }}>
        Explore media
      </Typography>

      <MediaSearchBar onSearch={handleSearch} loading={loading} />

      {/* Masonry-like layout */}
      <Box
        sx={{
          mt: 2,
          columnCount: { xs: 1, sm: 2, md: 3, lg: 4 },
          columnGap: 16 / 8, // theme spacing(2)
        }}
      >
        {items.map((m) => (
          <Box key={m.id} sx={{ breakInside: "avoid", mb: 2 }}>
            <MediaCard media={m} />
          </Box>
        ))}

        {!loading && items.length === 0 && (
          <Typography variant="body2" sx={{ opacity: 0.7, mt: 2 }}>
            No media found. Try a different filter.
          </Typography>
        )}
      </Box>
    </Box>
  );
}
