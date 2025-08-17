import apiClient from "../../../api/apiClient";

/**
 * Fetch media with optional filters.
 * Adjust param names to match your backend if different.
 */
export async function searchMedia({
  q,
  type,           // "Book" | "Film" | "Music" | etc.
  creator,
  yearFrom,
  yearTo,
  sort,           // e.g. "recent" | "title"
  page = 1,
  pageSize = 24,
} = {}) {
  const params = { page, pageSize };
  if (q)        params.q = q;
  if (type)     params.type = type;
  if (creator)  params.creator = creator;
  if (yearFrom) params.yearFrom = yearFrom;
  if (yearTo)   params.yearTo = yearTo;
  if (sort)     params.sort = sort;

  const { data } = await apiClient.get("/media", { params });
  return data;
}
