import { useMemo, useState } from "react";
import { IconButton, Tooltip } from "@mui/material";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import FavoriteIcon from "@mui/icons-material/Favorite";
import { useFavouritesContext } from "../context/FavouritesContext";

export default function FavouriteDestinationButton({
  destinationId,
  size = "medium",
  light = false, // white-on-hero style
}) {
  const { isDestinationFavourited, toggleDestination } = useFavouritesContext();
  const [pending, setPending] = useState(false);

  // Normalise id once (avoid string/number mismatches in Set.has)
  const id = useMemo(() => Number(destinationId), [destinationId]);
  const saved = Number.isFinite(id) ? isDestinationFavourited(id) : false;

  const onClick = async (e) => {
    // If this sits inside a CardActionArea/Link, prevent navigation
    e.preventDefault();
    e.stopPropagation();

    if (!Number.isFinite(id)) {
      console.warn("FavouriteDestinationButton: invalid destinationId", destinationId);
      return;
    }
    if (pending) return;

    setPending(true);
    try {
      await toggleDestination(id);
    } catch (err) {
      console.error("Failed to toggle favourite:", err);
    } finally {
      setPending(false);
    }
  };

  const iconProps = light ? { htmlColor: "#fff" } : {};
  const bg = light ? "rgba(255,255,255,.2)" : "action.hover";
  const bgHover = light ? "rgba(255,255,255,.3)" : "action.selected";

  return (
    <Tooltip title={saved ? "Saved" : "Save"}>
      <span>
        <IconButton
          onClick={onClick}
          size={size}
          disabled={pending}
          aria-pressed={saved}
          aria-label={saved ? "Remove from favourites" : "Save to favourites"}
          sx={{ bgcolor: bg, "&:hover": { bgcolor: bgHover } }}
        >
          {saved ? <FavoriteIcon {...iconProps} /> : <FavoriteBorderIcon {...iconProps} />}
        </IconButton>
      </span>
    </Tooltip>
  );
}
