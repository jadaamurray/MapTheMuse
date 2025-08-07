import axios from 'axios';

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  withCredentials: true,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor – attach token if exists
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('authToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    } // remove token logic after implementing cookie auth
    return config;
},
  (error) => {
    return Promise.reject(error);
  }
);

// Response Interceptor – handle common errors globally
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('authToken'); // rmeove after implementing cookie auth
      window.location.href = '/login'; // or use navigate() if using react-router
    }

    return Promise.reject(error.response?.data || error.message);
  }
);

export default apiClient;