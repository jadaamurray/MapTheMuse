export default function SpotifyEmbed({ playlistId, height = 500, theme = 0 }) {
  if (!playlistId) return null;
  const src = `https://open.spotify.com/embed/playlist/${encodeURIComponent(
    playlistId
  )}?theme=${theme}`;

  return (
    <iframe
      title="Spotify playlist"
      src={src}
      width="100%"
      height={height}
      allow="autoplay; clipboard-write; encrypted-media; fullscreen; picture-in-picture"
      loading="lazy"
      style={{ borderRadius: 12 }}
    />
  );
}
