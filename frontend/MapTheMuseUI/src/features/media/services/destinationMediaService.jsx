import apiClient from "../../../api/apiClient";

export const getDestinationMedia = (destinationId) =>
  apiClient.get(`/destinations/${destinationId}/media`).then(r => r.data);
