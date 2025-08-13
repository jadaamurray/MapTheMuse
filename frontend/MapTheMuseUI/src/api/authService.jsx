import apiClient from './apiClient';

export const AuthService = {
  login: (credentials) => apiClient.post('/account/login', credentials),
  register: (data) => apiClient.post('/account/register', data),
  verifyEmail: (userId, token) => apiClient.get('/account/verify-email', {
    params: { userId, token }
  }),
  logout: () => apiClient.post('/account/logout'),
  getCurrentUser: () => apiClient.get('/account/me').then(r => r.data),
};