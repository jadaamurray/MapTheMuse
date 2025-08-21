// src/features/media/services/destinationMediaService.js
import apiClient from "../../../api/apiClient";

/**
 * @typedef {"Movie"|"Tv"|"Book"|"Song"|"Album"|"Artwork"} MediaType
 */

/**
 * Response item shape (for reference):
 * {
 *   linkId: number,
 *   source: string,
 *   externalId: string,
 *   type: MediaType,         // enum as string from API
 *   title: string|null,
 *   year: number|null,
 *   creator: string|null,
 *   posterPath: string|null,
 *   overview: string|null,
 *   contextNote: string|null
 * }
 */

export const destinationMediaService = {
  // GET all media linked to a destination
  fetchForDestination: (destinationId) =>
    apiClient.get(`destinations/${destinationId}/media`).then(r => r.data),

  // POST link a single media item to a destination
  // payload: { source, mediaType, externalId, title?, posterPath?, contextNote?, orderIndex? }
  link: (destinationId, data) =>
    apiClient.post(`destinations/${destinationId}/media`, data).then(r => r.data),

  // POST bulk link several items
  // items: Array<{ source, mediaType, externalId, title?, posterPath?, contextNote?, orderIndex? }>
  bulkLink: (destinationId, items) =>
    apiClient.post(`destinations/${destinationId}/media/bulk`, items).then(r => r.data),

  // PATCH update the note for a specific link (server expects a plain JSON string body)
  updateNote: (destinationId, linkId, note) =>
    apiClient.patch(
      `destinations/${destinationId}/media/${linkId}/note`,
      JSON.stringify(note),
      { headers: { "Content-Type": "application/json" } }
    ).then(r => r.data),

  // POST reorder one or more links: [{ linkId, orderIndex }]
  reorder: (destinationId, updates) =>
    apiClient.post(`destinations/${destinationId}/media/reorder`, updates).then(r => r.data),

  // DELETE unlink by link id
  unlink: (linkId) =>
    apiClient.delete(`destinations/media/${linkId}`).then(r => r.data),
};
