import { createContext, useCallback, useContext, useEffect, useMemo, useRef, useState } from "react";
import { favouritesService as svc } from "../services/favouritesService";
import { useAuthContext } from "../../auth/context/AuthContext";
import toast from "react-hot-toast";

const Ctx = createContext(null);
const keyForUser = (userId) => `mtm:favs:v1:${userId || "anon"}`;
const mediaKey = (m) => `${m.source}|${m.type}|${m.externalId}`; // stable key for quick lookups

export function FavouritesProvider({ children }) {
  const { user } = useAuthContext();
  const userId = user?.id ?? "anon";

  const [mediaSet, setMediaSet] = useState(() => new Set());
  const [destinationSet, setDestinationSet] = useState(() => new Set());
  const [loading, setLoading] = useState(true);
  const [savedMedia, setSavedMedia] = useState([]);
  const [savedDestinations, setSavedDestinations] = useState([]);


  // load from localStorage immediately for snappy boot
  useEffect(() => {
    if (!user) return;
    let cancelled = false;
    const fromLocal = localStorage.getItem(keyForUser(userId));
    if (fromLocal) {
      try {
        const parsed = JSON.parse(fromLocal);
        setMediaSet(new Set(parsed.media || []));
        setDestinationSet(new Set(parsed.destinations || []));
        setSavedMedia(parsed.savedMedia || []);               // hydrate fast from cache if present
        setSavedDestinations(parsed.savedDestinations || []);
      } catch { }
    }

    setLoading(true);
    Promise.all([svc.fetchMyMedia(), svc.fetchMyDestinations()])
      .then(([media, dests]) => {
        if (cancelled) return;
        const mSet = new Set(media.map(x => mediaKey({ source: x.source, type: x.type, externalId: x.externalId })));
        const dSet = new Set(dests.map(x => x.id));
        setMediaSet(mSet);
        setDestinationSet(dSet);
        setSavedMedia(media);                                 // <-- keep full lists
        setSavedDestinations(dests);
      })
      .catch(e => !cancelled && console.error(e))
      .finally(() => !cancelled && setLoading(false));

    return () => { cancelled = true; };
  }, [userId]);

  // persist snapshot
  useEffect(() => {
    const snapshot = JSON.stringify({
      media: Array.from(mediaSet),
      destinations: Array.from(destinationSet),
      savedMedia,
      savedDestinations
    });
    localStorage.setItem(keyForUser(userId), snapshot);
  }, [mediaSet, destinationSet, userId, savedMedia, savedDestinations]);

  const isMediaFavourited = useCallback(
    (source, type, externalId) => mediaSet.has(mediaKey({ source, type, externalId })),
    [mediaSet]
  );

  const isDestinationFavourited = useCallback(
    (destinationId) => destinationSet.has(destinationId),
    [destinationSet]
  );

  // --- optimistic actions ---

  const favouriteMedia = useCallback(async ({ source, type, externalId, title = null, posterPath = null }) => {
    const k = mediaKey({ source, type, externalId });
    setMediaSet(prev => new Set(prev).add(k));
    try {
      await svc.favouriteMedia({ source, mediaType: type, externalId, title, posterPath });
      toast.success("Added to favourites")
    } catch (e) {
      // rollback
      setMediaSet(prev => {
        const next = new Set(prev);
        next.delete(k);
        return next;
      });
      throw e;
    }
  }, []);

  const unfavouriteMedia = useCallback(async ({ source, type, externalId, mediaId = null }) => {
    const k = mediaKey({ source, type, externalId });
    setMediaSet(prev => {
      const next = new Set(prev);
      next.delete(k);
      return next;
    });
    try {
      if (mediaId != null) await svc.unfavouriteMediaByMediaId(mediaId);
      else await svc.unfavouriteMediaByExternal(source, type, externalId);
      toast.success("Removed from favourites");
    } catch (e) {
      // rollback
      setMediaSet(prev => new Set(prev).add(k));
      throw e;
    }
  }, []);

  const toggleMedia = useCallback(async ({ source, type, externalId, mediaId = null, title = null, posterPath = null }) => {
    console.log("User is: ", user);
    if (!user) {
      toast.error("You must be logged in to save favourites")
      return;
    }
    try {
      if (isMediaFavourited(source, type, externalId))
        return await unfavouriteMedia({ source, type, externalId, mediaId });
      return await favouriteMedia({ source, type, externalId, title, posterPath });
    } catch (e) {
      console.error('toggleMedia error:', e);
      return;
    }
  }, [isMediaFavourited, favouriteMedia, unfavouriteMedia]);

  const favouriteDestination = useCallback(async (destinationId) => {
    setDestinationSet(prev => new Set(prev).add(destinationId));
    try {
      await svc.favouriteDestination(destinationId);
      toast.success("Added to favourites")
    } catch (e) {
      setDestinationSet(prev => {
        const next = new Set(prev);
        next.delete(destinationId);
        return next;
      });
      throw e;
    }
  }, []);

  const unfavouriteDestination = useCallback(async (destinationId) => {
    setDestinationSet(prev => {
      const next = new Set(prev);
      next.delete(destinationId);
      return next;
    });
    try {
      await svc.unfavouriteDestination(destinationId);
      toast.success("Removed from favourites");
    } catch (e) {
      setDestinationSet(prev => new Set(prev).add(destinationId));
      throw e;
    }
  }, []);

  const toggleDestination = useCallback(async (destinationId) => {
    if (!user) {
      toast.error("You must be logged in to save favourites")
      return;
    }
    try {
      if (isDestinationFavourited(destinationId))
        return unfavouriteDestination(destinationId);
      return favouriteDestination(destinationId);
    } catch (e) {
      console.error('toggleDestination error:', e);
      return;
    }
  }, [isDestinationFavourited, favouriteDestination, unfavouriteDestination]);

  const value = useMemo(() => ({
    loading,
    // queries
    isMediaFavourited,
    isDestinationFavourited,
    // commands
    favouriteMedia,
    unfavouriteMedia,
    toggleMedia,
    favouriteDestination,
    unfavouriteDestination,
    toggleDestination,
    savedDestinations,
    savedMedia
  }), [
    loading,
    isMediaFavourited,
    isDestinationFavourited,
    favouriteMedia,
    unfavouriteMedia,
    toggleMedia,
    favouriteDestination,
    unfavouriteDestination,
    toggleDestination,
    savedDestinations,
    savedMedia
  ]);

  return <Ctx.Provider value={value}>{children}</Ctx.Provider>;
}

export const useFavouritesContext = () => {
  const ctx = useContext(Ctx);
  if (!ctx) throw new Error("useFavourites must be used within FavouritesProvider");
  return ctx;
};
