import React, {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useRef,
  useState,
} from "react";
import { destinationsService } from "../services/destinationsService";

const DestinationsContext = createContext(null);

export function DestinationsProvider({ children, autoLoad = true }) {
  const [items, setItems] = useState(null);     // list of destinations
  const [byId, setById] = useState(new Map());  // cache for detail views
  const [loading, setLoading] = useState(false); // list loading
  const [busy, setBusy] = useState(false);       // create/update/delete
  const [error, setError] = useState(null);


  // protect setState after unmount
  const mountedRef = useRef(true);
  useEffect(() => {
    mountedRef.current = true;
    return () => { mountedRef.current = false; };
  }, []);

  const safeSet = (fn) => {
    if (mountedRef.current) fn();
  };

  // Load all destinations (no filters/paging yet)
  const load = useCallback(async () => {
    setLoading(true);
    setError(null);
    console.log('loading destinations...')
    try {
      const data = await destinationsService.fetchDestinations();
      //console.log('data: ', data)
      safeSet(() => setItems(Array.isArray(data) ? data : (data?.items ?? [])));
      //console.log('🧠 Context setItems called');
      //console.log('destinations: ', items);
    } catch (e) {
      safeSet(() => setError(e));
    } finally {
      safeSet(() => setLoading(false));
    }
  }, []);

  useEffect(() => {
    if (autoLoad) load();
  }, [autoLoad, load]);

  // Get one destination (uses cache, fetches if needed)
  const getById = useCallback(async (id, { force = false } = {}) => {
    if (!force && byId.has(id)) return byId.get(id);
    try {
      const detail = await destinationsService.fetchDestinationById(id);
      safeSet(() => setById(prev => {
        const next = new Map(prev);
        next.set(id, detail);
        return next;
      }));
      return detail;
    } catch (e) {
      safeSet(() => setError(e));
      throw e;
    }
  }, [byId]);

  // Create
  const create = useCallback(async (dto) => {
    setBusy(true);
    setError(null);
    try {
      const created = await destinationsService.createDestination(dto);
      safeSet(() => {
        setItems(prev => (prev ? [created, ...prev] : [created]));
        setById(prev => {
          const next = new Map(prev);
          next.set(created.id, created);
          return next;
        });
      });
      return created;
    } catch (e) {
      safeSet(() => setError(e));
      throw e;
    } finally {
      safeSet(() => setBusy(false));
    }
  }, []);

  // Update (optimistic merge)
  const update = useCallback(async (id, dto) => {
    setBusy(true);
    setError(null);
    try {
      const ok = await destinationsService.updateDestination(id, dto); // returns true on 204
      if (ok) {
        safeSet(() => {
          setItems(prev => prev?.map(d => (d.id === id ? { ...d, ...dto } : d)) ?? prev);
          setById(prev => {
            const existing = prev.get(id);
            const updated = existing ? { ...existing, ...dto } : { id, ...dto };
            const next = new Map(prev);
            next.set(id, updated);
            return next;
          });
        });
      }
      return ok;
    } catch (e) {
      safeSet(() => setError(e));
      throw e;
    } finally {
      safeSet(() => setBusy(false));
    }
  }, []);

  // Delete
  const remove = useCallback(async (id) => {
    setBusy(true);
    setError(null);
    try {
      const ok = await destinationsService.deleteDestination(id); // true on 204
      if (ok) {
        safeSet(() => {
          setItems(prev => prev?.filter(d => d.id !== id) ?? prev);
          setById(prev => {
            const next = new Map(prev);
            next.delete(id);
            return next;
          });
        });
      }
      return ok;
    } catch (e) {
      safeSet(() => setError(e));
      throw e;
    } finally {
      safeSet(() => setBusy(false));
    }
  }, []);

  const value = useMemo(() => ({
    items,            // null until loaded, then array
    byId,             // Map<id, detail>
    loading,          // list loading state
    busy,             // create/update/delete in-flight
    error,            // last error
    load,             // refresh list
    refresh: load,    // alias
    getById,          // fetch detail (cached)
    create,
    update,
    remove,
  }), [items, byId, loading, busy, error, load, getById, create, update, remove]);

  return (
    <DestinationsContext.Provider value={value}>
      {children}
    </DestinationsContext.Provider>
  );
}

export function useDestinationsContext() {
  const ctx = useContext(DestinationsContext);
  if (!ctx) throw new Error("useDestinationsContext must be used within DestinationsProvider");
  return ctx;
}
