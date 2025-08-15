import { useCallback, useEffect, useState } from "react";
import { destinationsService } from "../services/destinationsService";

// Fetch ALL destinations
export function useDestinations({ enabled = true } = {}) {
  const [data, setData] = useState(null); // expects an array
  const [loading, setLoading] = useState(Boolean(enabled));
  const [error, setError] = useState(null);

  const fetchList = useCallback(async () => {
    if (!enabled) return;
    setLoading(true);
    setError(null);
    try {
      const items = await destinationsService.fetchDestinations();
      setData(items);
    } catch (e) {
      setError(e);
    } finally {
      setLoading(false);
    }
  }, [enabled]);

  useEffect(() => {
    fetchList();
  }, [fetchList]);

  return { data, loading, error, refetch: fetchList };
}

// Fetch ONE destination by id
export function useDestination(id, { enabled = true } = {}) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(Boolean(enabled && id));
  const [error, setError] = useState(null);

  const fetchOne = useCallback(async () => {
    if (!enabled || !id) return;
    setLoading(true);
    setError(null);
    try {
      const item = await destinationsService.fetchDestinationById(id);
      setData(item);
    } catch (e) {
      setError(e);
    } finally {
      setLoading(false);
    }
  }, [id, enabled]);

  useEffect(() => {
    fetchOne();
  }, [fetchOne]);

  return { data, loading, error, refetch: fetchOne };
}

// Create
export function useCreateDestination({ onSuccess, onError } = {}) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const create = useCallback(async (dto) => {
    setLoading(true);
    setError(null);
    try {
      const created = await destinationsService.createDestination(dto);
      onSuccess?.(created);
      return created;
    } catch (e) {
      setError(e);
      onError?.(e);
      throw e;
    } finally {
      setLoading(false);
    }
  }, [onSuccess, onError]);

  return { create, loading, error };
}

// Update
export function useUpdateDestination({ onSuccess, onError } = {}) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const update = useCallback(async (id, dto) => {
    setLoading(true);
    setError(null);
    try {
      const ok = await destinationsService.updateDestination(id, dto);
      onSuccess?.(ok);
      return ok;
    } catch (e) {
      setError(e);
      onError?.(e);
      throw e;
    } finally {
      setLoading(false);
    }
  }, [onSuccess, onError]);

  return { update, loading, error };
}

// Delete
export function useDeleteDestination({ onSuccess, onError } = {}) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const remove = useCallback(async (id) => {
    setLoading(true);
    setError(null);
    try {
      const ok = await destinationsService.deleteDestination(id);
      onSuccess?.(ok);
      return ok;
    } catch (e) {
      setError(e);
      onError?.(e);
      throw e;
    } finally {
      setLoading(false);
    }
  }, [onSuccess, onError]);

  return { remove, loading, error };
}
