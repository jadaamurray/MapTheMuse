import FlightBoard from "../components/FlightBoard/FlightBoard";
import {
  Box, Typography, Grid, Card, CardContent, Alert
} from "@mui/material";
import { useDestinations } from "../hooks/useDestinations";
import DestinationSearchBar from "../components/DestinationSearchBar";
import { searchDestinations } from "../services/searchDestinations";
import { useState, useCallback } from "react";

export default function DestinationsPage() {
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

  return (
    <Box sx={{ width: "100%", py: 25, display: "flex", gap: 15, flexDirection: "column" }}>
      <Box sx={{ alignItems: "center" }}>
        <Typography variant="h1" sx={{ color: "primary.main" }}>Where are you off to?</Typography>
        <Typography variant="body2">Pick your next adventure from our curated flight board</Typography>
      </Box>

      <Box sx={{ position: "sticky", top: 64, zIndex: 1 }}>
        <FlightBoard destinations={destinations ?? []} />
      </Box>

      <Box sx={{ p: { xs: 2, md: 3 } }}>
        <DestinationSearchBar onSearch={handleSearch} loading={searchLoading} />

        {searchError && <Alert severity="error" sx={{ mt: 2 }}>{searchError}</Alert>}

        <Grid container spacing={2} sx={{ mt: 2 }}>
          {results.map((d) => (
            <Grid key={d.id} xs={12} sm={6} md={4} lg={3}>
              <Card sx={{ borderRadius: 3 }}>
                <CardContent>
                  <Typography variant="h6">{d.name}</Typography>
                  {d.shortDescription && (
                    <Typography variant="body2" sx={{ mt: 0.5 }}>{d.shortDescription}</Typography>
                  )}
                </CardContent>
              </Card>
            </Grid>
          ))}

          {hasSearched && !searchLoading && results.length === 0 && (
            <Grid xs={12}>
              <Typography variant="body2" sx={{ opacity: 0.7, mt: 2 }}>
                No destinations yet — try a different filter.
              </Typography>
            </Grid>
          )}
        </Grid>
      </Box>
    </Box>
  );
}
