// src/features/media/hooks/useDestinationMedia.jsx
import { useCallback, useEffect, useMemo, useState } from "react";
import { destinationMediaService as svc } from "../services/destinationMediaService";

// If the API ever returns numeric enums, normalise them to strings:
const MEDIA_TYPE_NAME = { 1: "Movie", 2: "Tv", 3: "Book", 4: "Song", 5: "Album", 6: "Artwork" };
const normaliseType = (t) => (typeof t === "string" ? t : (MEDIA_TYPE_NAME[t] ?? String(t)));
const normaliseItems = (arr) => (Array.isArray(arr) ? arr.map(i => ({ ...i, type: normaliseType(i.type) })) : []);

// { refresh, addLink, addLinksBulk, updateNote, reorder, removeLink }

export function useDestinationMedia(destinationId) {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [err, setErr] = useState(null);

  const canQuery = useMemo(() => Number.isFinite(Number(destinationId)) && Number(destinationId) > 0, [destinationId]);

  // initial + id changes
  useEffect(() => {
    let cancelled = false;
    if (!canQuery) {
      setItems([]);
      setLoading(false);
      setErr(null);
      return;
    }

    setLoading(true);
    setErr(null);

    svc.fetchForDestination(destinationId)
      .then((data) => { if (!cancelled) setItems(normaliseItems(data)); })
      .catch((e) => { if (!cancelled) setErr(e?.message ?? "Failed to load destination media"); })
      .finally(() => { if (!cancelled) setLoading(false); });

    return () => { cancelled = true; };
  }, [destinationId, canQuery]);

  const refresh = useCallback(async () => {
    if (!canQuery) return;
    try {
      const data = await svc.fetchForDestination(destinationId);
      setItems(normaliseItems(data));
    } catch (e) {
      setErr(e?.message ?? "Failed to refresh destination media");
    }
  }, [destinationId, canQuery]);

  const addLink = useCallback(async (payload) => {
    await svc.link(destinationId, payload);
    await refresh();
  }, [destinationId, refresh]);

  const addLinksBulk = useCallback(async (list) => {
    const res = await svc.bulkLink(destinationId, list);
    await refresh();
    return res; // { created, createdIds, duplicates, errors }
  }, [destinationId, refresh]);

  const updateNote = useCallback(async (linkId, note) => {
    await svc.updateNote(destinationId, linkId, note);
    // optimistic update
    setItems(prev => prev.map(i => (i.linkId === linkId ? { ...i, contextNote: note } : i)));
  }, [destinationId]);

  const reorder = useCallback(async (updates) => {
    await svc.reorder(destinationId, updates);
    await refresh();
  }, [destinationId, refresh]);

  const removeLink = useCallback(async (linkId) => {
    await svc.unlink(linkId);
    setItems(prev => prev.filter(i => i.linkId !== linkId));
  }, []);

  return [
    { items, loading, error: err },
    { refresh, addLink, addLinksBulk, updateNote, reorder, removeLink },
  ];
}
