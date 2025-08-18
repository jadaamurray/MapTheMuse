import { useEffect, useState } from "react";
import { getDestinationMedia } from "../services/destinationMediaService";

export function useDestinationMedia(id) {
    const [media, setMedia] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        if (!id) return;
        setLoading(true);
        getDestinationMedia(id)
            .then(setMedia)
            .catch(setError)
            .finally(() => setLoading(false));
    }, [id]);

    return { media, loading, error };
}
