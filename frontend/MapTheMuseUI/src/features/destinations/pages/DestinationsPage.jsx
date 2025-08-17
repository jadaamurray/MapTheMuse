import FlightBoard from "../components/FlightBoard/FlightBoard";
import {
  Box, Typography, Grid, Card, CardContent, CardMedia, Alert, Skeleton, Stack, Button, Container
} from "@mui/material";
import { useDestinations } from "../hooks/useDestinations";
import DestinationSearchBar from "../components/DestinationSearchBar";
import { searchDestinations } from "../services/searchDestinations";
import { useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";

export default function DestinationsPage() {
  const navigate = useNavigate();
  const { data: destinations, loading: initialLoading, error: initialError } = useDestinations();

  const [results, setResults] = useState([]);
  const [searchLoading, setSearchLoading] = useState(false);
  const [searchError, setSearchError] = useState(null);
  const [hasSearched, setHasSearched] = useState(false);

  const handleSearch = useCallback(async (filters = {}) => {
    try {
      setHasSearched(true);
      setSearchLoading(true);
      setSearchError(null);
      const data = await searchDestinations(filters);
      setResults(data);
    } catch (e) {
      setSearchError(e?.message || "Search failed");
    } finally {
      setSearchLoading(false);
    }
  }, []);

  if (initialLoading) return <Box py={4}>Loading…</Box>;
  if (initialError)   return <Box py={4}>Failed to load destinations</Box>;

  const cards = (hasSearched ? results : (destinations ?? [])).slice(0, 24); // cap initial render

  return (
    <Container maxWidth="lg" sx={{ py: { xs: 6, md: 10 } }}>
      {/* Hero */}
      <Box sx={{ textAlign: "center", mb: { xs: 4, md: 6 } }}>
        <Typography variant="h3" sx={{ color: "primary.main", fontWeight: 800, mb: 1 }}>
          Where are you off to?
        </Typography>
        <Typography variant="body1" sx={{ opacity: 0.8 }}>
          Pick your next adventure from our curated flight board. Don’t see it? Search below.
        </Typography>
      </Box>

      {/* Flight Board */}
      <Box sx={{ position: "relative", zIndex: 1, mb: { xs: 4, md: 6 } }}>
        <FlightBoard destinations={destinations ?? []} />
      </Box>

      {/* Search Panel */}
      <Box
        sx={{
          bgcolor: "background.paper",
          border: 1,
          borderColor: "divider",
          borderRadius: 3,
          px: { xs: 2, md: 3 },
          py: { xs: 2, md: 3 },
          boxShadow: "0 4px 24px rgba(0,0,0,0.05)",
          mb: { xs: 3, md: 5 }
        }}
      >
        <Stack direction={{ xs: "column", sm: "row" }} alignItems="center" spacing={2}>
          <Typography variant="h6" sx={{ flexShrink: 0, fontWeight: 700 }}>
            Can’t find your destination?
          </Typography>
          <Box sx={{ flex: 1, width: "100%" }}>
            <DestinationSearchBar onSearch={handleSearch} loading={searchLoading} />
          </Box>
          {/* Optional future: <Button onClick={() => navigate('/search')}>Advanced search</Button> */}
        </Stack>

        {searchError && <Alert severity="error" sx={{ mt: 2 }}>{searchError}</Alert>}
      </Box>

      {/* Results / Gallery using thumbUrl */}
      <Grid container spacing={3}>
        {searchLoading
          ? Array.from({ length: 8 }).map((_, i) => (
              <Grid item xs={12} sm={6} md={4} lg={3} key={`sk-${i}`}>
                <Card sx={{ borderRadius: 3, overflow: "hidden" }}>
                  <Skeleton variant="rectangular" height={160} />
                  <CardContent>
                    <Skeleton width="60%" />
                    <Skeleton width="90%" />
                  </CardContent>
                </Card>
              </Grid>
            ))
          : cards.map((d) => {
              const img = d.thumbUrl || d.imageUrl || "";
              return (
                <Grid item xs={12} sm={6} md={4} lg={3} key={d.id}>
                  <Card
                    sx={{
                      borderRadius: 3,
                      overflow: "hidden",
                      cursor: "pointer",
                      transition: "transform .15s ease, box-shadow .15s ease",
                      "&:hover": { transform: "translateY(-2px)", boxShadow: 6 }
                    }}
                    onClick={() => navigate(`/destinations/${d.slug || d.id}`)}
                  >
                    {/* image */}
                    {img ? (
                      <CardMedia
                        component="img"
                        image={img}
                        alt={d.name}
                        loading="lazy"
                        sx={{ aspectRatio: "4/3", objectFit: "cover" }}
                        onError={(e) => { e.currentTarget.style.display = "none"; }}
                      />
                    ) : null}

                    {/* text */}
                    <CardContent sx={{ display: "grid", gap: 0.5 }}>
                      <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                        {d.name}
                      </Typography>
                      {d.shortDescription && (
                        <Typography variant="body2" sx={{ opacity: 0.8 }}>
                          {d.shortDescription}
                        </Typography>
                      )}
                    </CardContent>
                  </Card>
                </Grid>
              );
            })}

        {hasSearched && !searchLoading && cards.length === 0 && (
          <Grid item xs={12}>
            <Typography variant="body2" sx={{ opacity: 0.7, mt: 2, textAlign: "center" }}>
              No destinations yet — try a different filter.
            </Typography>
          </Grid>
        )}
      </Grid>

      {/* CTA to add/search more */}
      <Box sx={{ textAlign: "center", mt: { xs: 4, md: 6 } }}>
        <Button variant="outlined" onClick={() => navigate("/destinations/search")}>
          Can’t find it? Try advanced search
        </Button>
      </Box>
    </Container>
  );
}
