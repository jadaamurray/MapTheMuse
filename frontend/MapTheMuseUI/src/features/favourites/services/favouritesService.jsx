import apiClient from "../../../api/apiClient";

export const favouritesService = {
  // MEDIA
  fetchMyMedia: () => apiClient.get("users/me/favourites/media?page=1&pageSize=500").then(r => r.data),
  favouriteMedia: (payload) => apiClient.post("users/me/favourites/media", payload).then(r => r.data),
  unfavouriteMediaByMediaId: (mediaId) => apiClient.delete(`users/me/favourites/media/${mediaId}`).then(r => r.data),
  unfavouriteMediaByExternal: (source, type, externalId) => apiClient.delete(`users/me/favourites/media/external/${source}/${type}/${externalId}`).then(r => r.data),

  // DESTINATIONS
  fetchMyDestinations: () => apiClient.get("users/me/favourites/destinations?page=1&pageSize=500").then(r => r.data),
  favouriteDestination: (destinationId) => apiClient.post(`users/me/favourites/destinations/${destinationId}`).then(r => r.data),
  unfavouriteDestination: (destinationId) => apiClient.delete(`users/me/favourites/destinations/${destinationId}`).then(r => r.data),
};
