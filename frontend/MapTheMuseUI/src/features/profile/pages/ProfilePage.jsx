// src/features/profile/pages/ProfilePage.jsx
import {
  Box,
  Typography,
  Button,
  Tabs,
  Tab,
  Grid,
  Card,
  CardMedia,
  CardContent,
  CardActionArea,
  Divider,
  Skeleton
} from "@mui/material";
import { useMemo, useState } from "react";
import { useAuthContext } from "../../auth/context/AuthContext";
import ProfileHeader from "../components/ProfileHeader";
import { useFavourites } from "../../favourites/context/FavouritesContext";
import MediaRail from "../../media/components/MediaRail";
import { useParams, useNavigate, Link as RouterLink } from "react-router-dom";
import MediaCard from "../../media/components/MediaCard";


// Demo lists (swap to real data later)
const mockItineraries = [
  { id: 101, title: "Tokyo in 5 Days", stops: 12, cover: "https://images.unsplash.com/photo-1558981806-ec527fa84c39?q=80&w=1600&auto=format&fit=crop", lastUpdated: "2025-06-18" },
  { id: 102, title: "Cinematic Rome", stops: 8, cover: "https://unsplash.com/photos/white-concrete-building-under-blue-sky-during-daytime-bzItNUjmjpU?utm_content=creditShareLink&utm_medium=referral&utm_source=unsplash", lastUpdated: "2025-04-03" },
  { id: 103, title: "Art & Architecture in Paris", stops: 10, cover: "https://images.unsplash.com/photo-1543340900-63b7344b53a7?q=80&w=1600&auto=format&fit=crop", lastUpdated: "2025-02-22" },
];

const mockSaved = [
  { id: "S1", type: "Film", title: "Before Sunrise", location: "Vienna, Austria", image: "https://images.unsplash.com/photo-1505764706515-aa95265c5abc?q=80&w=1600&auto=format&fit=crop" },
  { id: "S2", type: "Book", title: "Norwegian Wood", location: "Tokyo, Japan", image: "https://images.unsplash.com/photo-1513530534585-c7b1394c6d51?q=80&w=1600&auto=format&fit=crop" },
  { id: "S3", type: "Music", title: "Fado Classics", location: "Lisbon, Portugal", image: "https://images.unsplash.com/photo-1505761671935-60b3a7427bad?q=80&w=1600&auto=format&fit=crop" },
];

// ---replace with real API in separate hook file when ready ----------------------
// decide which user's profile to show
function useUserProfile(routeUserId, currentUser) {
  // If no :userId, this is "me"
  if (!routeUserId) return { profileUser: currentUser, loading: false };

  // TODO: replace with real fetch for someone else’s profile
  // e.g. const {data, isLoading} = useGetUserByIdQuery(routeUserId)
  // For now, return a minimal object so the page renders.
  return { profileUser: { id: routeUserId, userName: "traveller" }, loading: false };
}
// ------------------------------------------------------------
export default function ProfilePage() {
  const { user: currentUser, loading: authLoading } = useAuthContext();
  const { userId: routeUserId } = useParams();
  const [tab, setTab] = useState(0);
  const { loading: favLoading, savedDestinations, savedMedia } = useFavourites(); // TODO: fix favourites so they pull profileUser favourites and not currentUser favourites
  const navigate = useNavigate();

  console.log('profile userId: ', routeUserId);

  const { profileUser, loading: profileLoading } = useUserProfile(routeUserId, currentUser);

  // ownber check (toggle edit button and private sections)
  const isOwner = !!(currentUser?.id && profileUser?.id && currentUser.id === profileUser.id);

  console.log('saved destinations: ', savedDestinations);

  // Map AppUser to UI fields
  const fullName = useMemo(() => {
    const f = profileUser?.firstName ?? "";
    const l = profileUser?.lastName ?? "";
    const name = `${f} ${l}`.trim();
    return name || profileUser?.userName || "Your profile";
  }, [profileUser, isOwner]);

  const initials = useMemo(
    () =>
      fullName
        .split(" ")
        .filter(Boolean)
        .map((p) => p[0])
        .join("")
        .slice(0, 2)
        .toUpperCase(),
    [fullName]
  );

  const username = profileUser?.userName;
  const avatarUrl = profileUser?.profilePictureUrl || "";
  const country = profileUser?.country;
  const preferredLanguage = profileUser?.preferredLanguage;

  const mediaRailItems = useMemo(
    () => (savedMedia || []).map(m => ({
      linkId: m.mediaId ?? `${m.source}-${m.externalId}`,
      source: m.source,
      externalId: m.externalId,
      type: m.type,
      title: m.title ?? null,
      year: null,
      creator: null,
      posterPath: m.posterPath ?? null,
      overview: null,
      contextNote: null,
      mediaId: m.mediaId ?? null,
    })),
    [savedMedia]
  );

  if (authLoading || profileLoading) {
    return (
      <Box sx={{ p: 3 }}>
        <Skeleton variant="rectangular" height={160} sx={{ borderRadius: 3, mb: 3 }} />
        <Skeleton variant="text" width={220} />
        <Skeleton variant="text" width={160} />
      </Box>
    );
  }

  if (!profileUser) {
    return (
      <Box sx={{ p: 3 }}>
        <Typography variant="h6">Profile not found</Typography>
        <Typography sx={{ color: "text.secondary" }}>
          The link might be broken or this profile doesn’t exist.
        </Typography>
      </Box>
    );
  }

  return (
    <Box sx={{ pb: 6, height: "100%" }}>
      <ProfileHeader user={profileUser} canEdit={isOwner} />
      {/* Tabs for saved */}
      <Box sx={{ maxWidth: 1280, mx: "auto", px: { xs: 1.5, md: 3 }, mt: 2 }}>
        <Typography variant="h5" sx={{ fontWeight: 800, mb: 1 }}>Saved</Typography>
        <Divider sx={{ mt: 1.5, mb: 2 }} />
        <Tabs
          value={tab}
          onChange={(_, v) => setTab(v)}
          centered
          sx={{ mt: 2 }}
          slotProps={{
            indicator: { sx: { height: 3, borderRadius: 3 } }
          }}
        >
          <Tab label="Destinations" />
          <Tab label="Media" />
        </Tabs>

        {/* CONTENT GRID */}
        <Box sx={{ maxWidth: 1280, mx: "auto", px: { xs: 1.5, md: 3 }, mt: 2 }}>
          {favLoading ? (
            <Grid container spacing={2}>
              {Array.from({ length: 8 }).map((_, i) => (
                <Grid key={i} item xs={12} sm={6} md={4} lg={3}>
                  <Skeleton variant="rectangular" height={220} sx={{ borderRadius: 3 }} />
                </Grid>
              ))}
            </Grid>
          ) : tab === 0 ? (
            (savedDestinations?.length ? (
              <Grid container spacing={2}>
                {savedDestinations.map((d) => (
                  <Grid key={d.id} size={{ xs: 6, sm: 6, md: 4, lg: 3 }}>
                    <Card sx={{ borderRadius: 3, overflow: "hidden" }}>
                      <CardActionArea component={RouterLink} to={`/destinations/${d.id}`}>
                        <CardMedia
                          component="img"
                          image={d?.thumbUrl ?? null}
                          alt={d.name}
                          sx={{ aspectRatio: "4/3", objectFit: "cover" }}
                        />
                        <CardContent sx={{ pb: 2 }}>
                          <Typography variant="subtitle1" sx={{ fontWeight: 700 }} noWrap title={d.name}>
                            {d.name}
                          </Typography>
                          {!!d.shortDescription && (
                            <Typography variant="body2" sx={{ color: "text.secondary" }} noWrap title={d.shortDescription}>
                              {d.shortDescription}
                            </Typography>
                          )}
                        </CardContent>
                      </CardActionArea>
                    </Card>
                  </Grid>
                ))}
              </Grid>
            ) : (
              <Typography sx={{ color: "text.secondary" }}>You haven’t saved any destinations yet.</Typography>
            ))
          ) : (
            <Grid container spacing={2}>
              {mediaRailItems?.length ? (
                mediaRailItems.map((m) => (
                  <Grid key={m.linkId} size={{ xs: 6, sm: 6, md: 4, lg: 3 }}>
                    <MediaCard item={m} />
                  </Grid>
                ))
              ) : (
                <Grid size={12}>
                  <Typography sx={{ color: "text.secondary" }}>You haven’t saved any media yet.</Typography>

                </Grid>
              )}
            </Grid>
          )}
        </Box>
      </Box>
    </Box>
  );
}
