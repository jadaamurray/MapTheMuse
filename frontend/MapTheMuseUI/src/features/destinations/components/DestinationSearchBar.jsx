import { useEffect, useMemo, useState } from "react";
import {
  Box, Paper, Stack, TextField, Chip, IconButton, Button,
  InputAdornment, Typography, CircularProgress, Divider
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import TuneIcon from "@mui/icons-material/Tune";
import ClearIcon from "@mui/icons-material/Clear";

const DEFAULT_CONTINENTS = ["Africa","Asia","Europe","North America","South America","Oceania","Antarctica"];
const DEFAULT_FACT_KEYS  = ["History","Culture","Food","Art","Architecture","Nature","Film","Music","Literature"];

export default function DestinationSearchBar({
  continents = DEFAULT_CONTINENTS,
  factKeys   = DEFAULT_FACT_KEYS,
  autoSearch = true,
  loading = false,
  onSearch,            // (filters) => void
}) {
  const [query, setQuery] = useState("");
  const [selectedContinent, setSelectedContinent] = useState("");
  const [selectedFactKey, setSelectedFactKey] = useState("");
  const [filtersOpen, setFiltersOpen] = useState(true);

  // Debounce autosubmit
  useEffect(() => {
    if (!autoSearch || !onSearch) return;
    const t = setTimeout(() => {
      onSearch({
        continent: selectedContinent || undefined,
        factKey: selectedFactKey || undefined,
        // q: query || undefined, // add if your API supports free text
      });
    }, 400);
    return () => clearTimeout(t);
  }, [query, selectedContinent, selectedFactKey, autoSearch, onSearch]);

  const canClear = useMemo(
    () => !!(query || selectedContinent || selectedFactKey),
    [query, selectedContinent, selectedFactKey]
  );

  const submit = () => {
    onSearch?.({
      continent: selectedContinent || undefined,
      factKey: selectedFactKey || undefined,
    });
  };

  const clearAll = () => {
    setQuery("");
    setSelectedContinent("");
    setSelectedFactKey("");
    onSearch?.({});
  };

  return (
    <Stack spacing={2}>
      <Paper elevation={6} sx={{ p: 1, borderRadius: 3, backdropFilter: "blur(6px)" }}>
        <Stack direction="row" alignItems="center" spacing={1} sx={{ px: 1 }}>
          <TextField
            fullWidth placeholder="Search destinations…"
            value={query} onChange={(e) => setQuery(e.target.value)}
            variant="outlined" size="medium"
            InputProps={{
              startAdornment: (
                <InputAdornment position="start"><SearchIcon /></InputAdornment>
              ),
            }}
          />
          <IconButton aria-label="Filters" onClick={() => setFiltersOpen(v => !v)} size="large">
            <TuneIcon />
          </IconButton>
          {canClear && (
            <IconButton aria-label="Clear filters" onClick={clearAll} size="large">
              <ClearIcon />
            </IconButton>
          )}
          <Button onClick={submit} variant="contained" disableElevation sx={{ borderRadius: 2, px: 2 }}>
            {loading ? <CircularProgress size={20} /> : "Search"}
          </Button>
        </Stack>

        {filtersOpen && (
          <>
            <Divider sx={{ my: 1 }} />
            <Box sx={{ px: 1, pb: 1 }}>
              <Typography variant="caption" sx={{ display: "block", mb: 0.5 }}>Continent</Typography>
              <Stack direction="row" flexWrap="wrap" gap={1} sx={{ mb: 1 }}>
                {continents.map(c => {
                  const selected = selectedContinent === c;
                  return (
                    <Chip key={c} label={c}
                      variant={selected ? "filled" : "outlined"}
                      color={selected ? "primary" : "default"}
                      onClick={() => setSelectedContinent(prev => (prev === c ? "" : c))}
                      sx={{ borderRadius: 2 }}
                    />
                  );
                })}
              </Stack>

              <Typography variant="caption" sx={{ display: "block", mb: 0.5 }}>Fact</Typography>
              <Stack direction="row" flexWrap="wrap" gap={1}>
                {factKeys.map(k => {
                  const selected = selectedFactKey === k;
                  return (
                    <Chip key={k} label={k}
                      variant={selected ? "filled" : "outlined"}
                      color={selected ? "secondary" : "default"}
                      onClick={() => setSelectedFactKey(prev => (prev === k ? "" : k))}
                      sx={{ borderRadius: 2 }}
                    />
                  );
                })}
              </Stack>
            </Box>
          </>
        )}
      </Paper>
    </Stack>
  );
}
