import apiClient from "./apiClient";

export const destinationsService = {
fetchDestinations: () => apiClient.get('/destinations').then(r => r.data),
fetchDestinationById: (id) => apiClient.get(`/destinations/${id}`).then(r => r.data),
createDestination: (data) => apiClient.post('/destinations', data).then(r => r.data),
updateDestination: (id, data) => apiClient.put(`/destinations/${id}`, data).then(r => r.data),
deleteDestination: (id) => apiClient.delete(`/destinations/${id}`).then(r => r.data)
}