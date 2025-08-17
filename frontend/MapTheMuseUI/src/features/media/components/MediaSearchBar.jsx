import { useEffect, useMemo, useState } from "react";
import {
  Box, Paper, Stack, TextField, Chip,
  InputAdornment, IconButton, Button, Divider
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import TuneIcon from "@mui/icons-material/Tune";
import ClearIcon from "@mui/icons-material/Clear";

const TYPES = ["Film", "Book", "Music"];

export default function MediaSearchBar({ onSearch, loading }) {
  const [q, setQ] = useState("");
  const [type, setType] = useState("");
  const [filtersOpen, setFiltersOpen] = useState(true);

  const canClear = useMemo(() => q || type, [q, type]);

  useEffect(() => {
    // initial load
    onSearch?.({ q, type });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const submit = () => onSearch?.({ q: q || undefined, type: type || undefined });

  const clearAll = () => {
    setQ(""); setType("");
    onSearch?.({});
  };

  return (
    <Paper elevation={3} sx={{ p: 1.25, borderRadius: 3 }}>
      <Stack direction="row" alignItems="center" spacing={1}>
        <TextField
          fullWidth
          size="medium"
          placeholder="Search title or creator…"
          value={q}
          onChange={(e) => setQ(e.target.value)}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start"><SearchIcon /></InputAdornment>
            ),
          }}
        />
        <IconButton onClick={() => setFiltersOpen((v) => !v)} title="Filters">
          <TuneIcon />
        </IconButton>
        {canClear && (
          <IconButton onClick={clearAll} title="Clear"><ClearIcon /></IconButton>
        )}
        <Button onClick={submit} variant="contained" disableElevation sx={{ borderRadius: 2 }}>
          {loading ? "Searching…" : "Search"}
        </Button>
      </Stack>

      {filtersOpen && (
        <>
          <Divider sx={{ my: 1 }} />
          <Box sx={{ px: 0.5, pb: 0.5 }}>
            <Stack direction="row" flexWrap="wrap" gap={1}>
              {TYPES.map((t) => {
                const selected = type === t;
                return (
                  <Chip
                    key={t}
                    label={t}
                    variant={selected ? "filled" : "outlined"}
                    color={selected ? "primary" : "default"}
                    onClick={() => setType((prev) => (prev === t ? "" : t))}
                    sx={{ borderRadius: 2 }}
                  />
                );
              })}
            </Stack>
          </Box>
        </>
      )}
    </Paper>
  );
}
