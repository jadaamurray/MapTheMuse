export function getSpotifyPlaylistId(input) {
  if (!input) return "";
  const s = String(input).trim();

  // spotify:playlist:<id>
  const mUri = s.match(/^spotify:playlist:([^:?/]+)$/i);
  if (mUri) return mUri[1];

  // https://open.spotify.com/(embed/)?playlist/<id>(?...)?#
  const mUrl = s.match(/open\.spotify\.com\/(?:embed\/)?playlist\/([^/?#]+)/i);
  if (mUrl) return mUrl[1];

  // assume it's already an ID
  return s;
}
