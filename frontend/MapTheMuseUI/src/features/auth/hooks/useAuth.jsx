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
      console.log('logged in successfully');
      console.log('getting profile information');
      const profile = await AuthService.getCurrentUser(); // fetch user details
      console.log('profile is', profile);
      setUser(profile); // store user in context
      navigate('/profile');
      console.log('Logged in successfully');
      toast.success('Logged in successfully');
    } catch (err) {
        setError(err.response?.data || 'Login failed');
      console.error('registration/login error:', err.response?.data);;
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
      await AuthService.register(data);
      console.log('trying to log in')
      await AuthService.login(data);
      console.log('getting user profile');
      const profile = await AuthService.getCurrentUser();
      console.log('user found, setting profile');
      setUser(profile);
      console.log('moving to profile');
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
      console.log('logging out');
      await AuthService.logout();
      setUser(null);
      console.log('logged out.')
      toast.success('Logged out');
    } catch (err) {
      console.log('logout error', err);
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
