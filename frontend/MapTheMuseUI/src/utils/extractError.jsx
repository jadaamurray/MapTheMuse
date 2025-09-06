export function extractError(err, fallback = "Something went wrong") {
  // axios-style
  if (err?.response?.data) {
    const d = err.response.data;
    return d.message || d.error || (typeof d === "string" ? d : fallback);
  }
  // fetch-style (you might throw { message } yourself)
  if (err?.message) return err.message;
  return fallback;
}
