import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthService } from '../services/authService';
import { useAuthContext } from '../context/AuthContext';
import toast from 'react-hot-toast';

const useAuth = () => {
  const navigate = useNavigate();
  const { setUser } = useAuthContext();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // LOGIN
  const login = async (credentials) => {
    setLoading(true);
    setError(null);
    try {
      console.log('logging in with', credentials)
      await AuthService.login(credentials); // cookie is set by backend
      const profile = await AuthService.getCurrentUser(); // fetch user details
      //console.log('profile is', profile)
      setUser(profile); // store user in context
      navigate('/profile');
      //console.log('Logged in successfully');
      toast.success('Logged in successfully');
    } catch (err) {
        setError(err.response?.data || 'Login failed');
      console.error('Registration error:', err.response?.data);;
    } finally {
      setLoading(false);
    }
  };

  // REGISTER
  const register = async (data, setFieldErrors) => {
    setLoading(true);
    setError(null);
    console.log('Registering with data: ', data)
    try {
      await AuthService.register(data); // backend sets cookie
      const profile = await AuthService.getCurrentUser();
      setUser(profile);
      navigate('/profile');
      toast.success('Account created');
    } catch (err) {
      if (err.fieldErrors && setFieldErrors) {
        setFieldErrors(err.fieldErrors);
        console.log('field errors: ', fieldErrors);
      } else {
        setError(err.response?.data || 'Registration failed');
      }
      console.error('Registration error:', err.response?.data);

    } finally {
      setLoading(false);
    }
  };

  // LOGOUT
  const logout = async () => {
    try {
      await AuthService.logout();
      setUser(null);
      //navigate('/login');
      toast.success('Logged out');
    } catch (err) {
      toast.error('Logout failed');
    }
  };

  return {
    login,
    register,
    logout,
    loading,
    error,
  };
};

export default useAuth;
