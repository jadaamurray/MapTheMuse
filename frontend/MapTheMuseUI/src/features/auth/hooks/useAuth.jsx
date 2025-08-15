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
      setError(err?.message || 'Login failed');
      toast.error('Login failed');
    } finally {
      setLoading(false);
    }
  };

  // REGISTER
  const register = async (data) => {
    setLoading(true);
    setError(null);
    console.log('Registering with data: ', data)
    try {
      await AuthService.register(data); // backend sets cookie
      const profile = await AuthService.getCurrentUser();
      setUser(profile);
      //navigate('/dashboard');
      toast.success('Account created');
    } catch (err) {
      setError(err?.message || 'Registration failed');
      toast.error('Registration failed');
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
