import { useState } from 'react';
//import { useNavigate } from 'react-router-dom';
import { AuthService } from '../api/authService';
import { useAuthContext } from '../context/AuthContext'; // if you're using context
import toast from 'react-hot-toast'; // optional for better UX

const useAuth = () => {
  //const navigate = useNavigate();
  const { setUser } = useAuthContext(); // assumes you store the user in context
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // LOGIN
  const login = async (credentials) => {
    setLoading(true);
    setError(null);
    try {
      await AuthService.login(credentials); // cookie is set by backend
      const profile = await AuthService.getCurrentUser(); // fetch user details
      setUser(profile); // store user in context
      //navigate('/dashboard');
      console.log('Logged in successfully');
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
