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
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      window.location.href = '/login'; // or use navigate() if using react-router. Put this in the useAuth hook
    }

    return Promise.reject(error.response?.data || error.message);
  }
);

export default apiClient;