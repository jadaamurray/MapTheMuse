import axios from 'axios';

console.log('Using API base URL:', import.meta.env.VITE_API_BASE_URL);

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  withCredentials: true,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Response Interceptor – handle common errors globally
apiClient.interceptors.response.use(r => r, (error) => Promise.reject(error));

export default apiClient;