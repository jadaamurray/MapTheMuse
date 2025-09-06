import React from "react";
import { Box, Skeleton, Typography } from "@mui/material";
import SpotifyEmbed from "../../../components/Media/SpotifyEmbed";
import { getSpotifyPlaylistId } from "../../../utils/getSpotifyPlaylistId";
import { useDestination } from "../hooks/useDestinations";

export default function DestinationSpotifyPlaylist({ destinationId, height = 352, theme = 0 }) {
  const { data, loading, error } = useDestination(destinationId);

  if (loading) {
    return <Skeleton variant="rounded" height={height} sx={{ borderRadius: 2 }} />;
  }
  if (error) {
    return <Typography colour="error">Couldn’t load playlist.</Typography>;
  }

  const raw = data?.spotifyPlaylistId; // e.g. "76Tp2EF3ZrhfcFJbDdXl71" or a full URL
  const id = getSpotifyPlaylistId(raw);

  if (!id) {
    // no playlist configured for this destination
    return null;
  }

  return (
    <Box sx={{ borderRadius: 2, overflow: "hidden" }}>
      <SpotifyEmbed playlistId={id} height={height} theme={theme} />
    </Box>
  );
}
