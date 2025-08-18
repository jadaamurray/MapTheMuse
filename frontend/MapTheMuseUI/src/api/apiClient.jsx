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
  response => response,
  error => {
    const { response } = error;

    if (response?.status === 400 && response?.data?.errors) {
      // Extract and flatten field-level validation errors
      const fieldErrors = {};

      for (const key in response.data.errors) {
        fieldErrors[key] = response.data.errors[key].join(' ');
      }

      // Attach to error object
      error.fieldErrors = fieldErrors;
    }

    return Promise.reject(error);
  }
);
export default apiClient;