// src/features/favourites/components/FavouriteMediaButton.jsx
import { useState } from "react";
import { IconButton, Tooltip } from "@mui/material";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import FavoriteIcon from "@mui/icons-material/Favorite";
import { useFavourites } from "../context/FavouritesContext";

/**
 * Props:
 * - source: "TMDB" | ...
 * - type: "Movie" | "Tv" | "Book" | "Song" | "Album" | "Artwork"
 * - externalId: string (e.g. TMDB id)
 * - mediaId?: number (optional, if you have it)
 * - title?: string|null (helps seed server cache)
 * - posterPath?: string|null (helps seed server cache)
 * - light?: boolean (white-on-hero)
 */
export default function FavouriteMediaButton({
    source,
    type,
    externalId,
    mediaId = null,
    title = null,
    posterPath = null,
    size = "small",
    light = false,
}) {
    const { isMediaFavourited, toggleMedia } = useFavourites();
    const [pending, setPending] = useState(false);
    const fav = isMediaFavourited(source, type, externalId);

    const onClick = async (e) => {
        e.preventDefault();
        if (pending) return;
        setPending(true);
        try {
            await toggleMedia({ source, type, externalId, mediaId, title, posterPath });
        } catch (err) {
            console.error("Failed to toggle media favourite:", err);
        } finally {
            setPending(false);
        }
    };

    const iconProps = light ? { htmlColor: "#fff" } : {};
    const bg = light ? "rgba(255,255,255,.2)" : "action.hover";
    const bgHover = light ? "rgba(255,255,255,.3)" : "action.selected";

    return (
        <Tooltip title={fav ? "Saved" : "Save"}>
            <span>
                <IconButton
                    onClick={onClick}
                    size={size}
                    disabled={pending}
                    aria-pressed={fav}
                    aria-label={fav ? "Remove from favourites" : "Save to favourites"}
                    color={fav ? "error" : "default"}
                    sx={{
                        bgcolor: fav ? "error.light" : "action.hover",
                        "&:hover": { bgcolor: fav ? "error.main" : "action.selected" },
                        color: fav ? "error.contrastText" : undefined,  // ensure white icon on red bg
                    }}
                >
                    {fav ? <FavoriteIcon {...iconProps} /> : <FavoriteBorderIcon {...iconProps} />}
                </IconButton>
            </span>
        </Tooltip>
    );
}
