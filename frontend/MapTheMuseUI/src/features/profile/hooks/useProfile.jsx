import { useAuthContext } from "../../auth/context/AuthContext";
import { profileService } from "../services/profileService";
import { useState, useCallback } from "react";
import { extractError } from "../../../utils/extractError";

export function useProfile() {
  const { user, refreshUser } = useAuthContext();
  const [loading, setLoading] = useState(false);
  const [error, setError]   = useState(null);

  const run = useCallback(async (fn) => {
    setLoading(true);
    setError(null);
    try {
      const result = await fn();
      return result;
    } catch (e) {
      setError(extractError(e));
      throw e;
    } finally {
      setLoading(false);
    }
  }, []);

  const update = useCallback(async (payload) => {
    await run(async () => {
      await profileService.update(payload);
      await refreshUser();
    });
  }, [run, refreshUser]);

  const changeUserName = useCallback(async (newUserName) => {
    await run(async () => {
      await profileService.changeUserName(newUserName);
      await refreshUser();
    });
  }, [run, refreshUser]);

  const startEmailChange = useCallback(async (newEmail) => {
    await run(async () => {
      await profileService.startEmailChange( newEmail ); 
      // no refresh until confirmed
    });
  }, [run]);

  const changePassword = useCallback(async ( currentPassword, newPassword ) => {
    await run(async () => {
      await profileService.changePassword( currentPassword, newPassword );
    });
  }, [run]);

  const clearError = () => setError(null);

  return {
    user,
    loading,
    error,
    clearError,
    update,
    changeUserName,
    startEmailChange,
    changePassword,
  };
}