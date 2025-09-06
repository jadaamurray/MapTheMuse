import { createContext, useContext, useState, useEffect, useCallback } from 'react';
import { AuthService } from '../services/authService';
import { extractError } from '../../../utils/extractError';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true); // initial app load
  const [refreshing, setRefreshing] = useState(false); // on-demand refreshes
  const [error, setError] = useState(null);

  const refreshUser = useCallback(async ({ silently = true } = {}) => {
    if (!silently) setRefreshing(true);
    try {
      const profile = await AuthService.getCurrentUser();
      setUser(profile);
      setError(null);
      return profile;
    } catch (err) {
      setUser(null);
      setError(extractError(err, "Failed to fetch current user"));
/*       if(!silently) throw err;
 */      return null;
    } finally {
      if (!silently) setRefreshing(false);
    }
  }, []);

  // On mount, fetch current user (if JWT cookie exists)
  useEffect(() => {
    (async () => {
      try {
        console.log('getting current user...')
        await refreshUser({ silently: true });
      } finally {
        setLoading(false);
      }
    })();
  }, [refreshUser]);

  return (
    <AuthContext.Provider value={{ user, setUser, loading, refreshing, refreshUser, error }}>
      {!loading && children}
    </AuthContext.Provider>
  );
};

export const useAuthContext = () => useContext(AuthContext);
