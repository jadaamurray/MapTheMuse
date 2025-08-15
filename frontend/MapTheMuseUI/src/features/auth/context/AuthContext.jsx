import { createContext, useContext, useState, useEffect } from 'react';
import { AuthService } from '../services/authService';
//import { useNavigate } from 'react-router-dom';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null); // stores current user
  const [loading, setLoading] = useState(true); // for initial fetch
  //const navigate = useNavigate();

  // On mount, fetch the current user (if JWT cookie exists)
  useEffect(() => {
    const fetchUser = async () => {
      //console.log('Fetching user...')
      try {
        const profile = await AuthService.getCurrentUser();
        setUser(profile);
      } catch (err) {
        setUser(null); // Not logged in or token invalid
      } finally {
        setLoading(false);
      }
    };

    fetchUser();
  }, []);

  return (
    <AuthContext.Provider value={{ user, setUser, loading }}>
      {!loading && children}
    </AuthContext.Provider>
  );
};

export const useAuthContext = () => useContext(AuthContext);
