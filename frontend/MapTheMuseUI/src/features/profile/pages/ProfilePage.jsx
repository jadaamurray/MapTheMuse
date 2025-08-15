// src/features/profile/pages/ProfilePage.jsx
import {
  Box,
  Stack,
  Avatar,
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
} from "@mui/material";
import ShareIcon from "@mui/icons-material/Share";
import EditIcon from "@mui/icons-material/Edit";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import LanguageIcon from "@mui/icons-material/Language";
import { useMemo, useState } from "react";
import { useAuthContext } from "../../auth/context/AuthContext";

// Demo lists (swap to real data later)
const mockItineraries = [
  { id: 101, title: "Tokyo in 5 Days", stops: 12, cover: "https://images.unsplash.com/photo-1558981806-ec527fa84c39?q=80&w=1600&auto=format&fit=crop", lastUpdated: "2025-06-18" },
  { id: 102, title: "Cinematic Rome",  stops: 8,  cover: "https://unsplash.com/photos/white-concrete-building-under-blue-sky-during-daytime-bzItNUjmjpU?utm_content=creditShareLink&utm_medium=referral&utm_source=unsplash", lastUpdated: "2025-04-03" },
  { id: 103, title: "Art & Architecture in Paris",  stops: 10, cover: "https://images.unsplash.com/photo-1543340900-63b7344b53a7?q=80&w=1600&auto=format&fit=crop", lastUpdated: "2025-02-22" },
];

const mockSaved = [
  { id: "S1", type: "Film", title: "Before Sunrise", location: "Vienna, Austria", image: "https://images.unsplash.com/photo-1505764706515-aa95265c5abc?q=80&w=1600&auto=format&fit=crop" },
  { id: "S2", type: "Book", title: "Norwegian Wood", location: "Tokyo, Japan", image: "https://images.unsplash.com/photo-1513530534585-c7b1394c6d51?q=80&w=1600&auto=format&fit=crop" },
  { id: "S3", type: "Music", title: "Fado Classics", location: "Lisbon, Portugal", image: "https://images.unsplash.com/photo-1505761671935-60b3a7427bad?q=80&w=1600&auto=format&fit=crop" },
];

export default function ProfilePage() {
  const { user, loading: authLoading } = useAuthContext();
  const [tab, setTab] = useState(0);

  // Map AppUser → UI fields
  const fullName = useMemo(() => {
    const f = user?.firstName ?? "";
    const l = user?.lastName ?? "";
    const name = `${f} ${l}`.trim();
    return name || user?.userName || "Your profile";
  }, [user]);

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

  const username = user?.userName;
  const avatarUrl = user?.profilePictureUrl || "";
  const country = user?.country;
  const preferredLanguage = user?.preferredLanguage;

  return (
    <Box sx={{ pb: 6, height: "100%" }}>
      {/* HEADER (Pinterest-style: centred) */}
      <Box sx={{ maxWidth: 960, mx: "auto", px: 5, py: 10 }}>
        <Stack alignItems="center" spacing={4}>
          <Avatar
            src={avatarUrl || undefined}
            alt={`${fullName}'s avatar`}
            sx={{ width: 96, height: 96 }}
          >
            {initials}
          </Avatar>

          <Typography variant="h4" sx={{ fontWeight: 800, textAlign: "center" }}>
            {fullName}
          </Typography>

          {username && (
            <Typography variant="body2" sx={{ color: "text.secondary" }}>
              @{username}
            </Typography>
          )}

          {/* Meta line: country (instead of followers/following) + language */}
          <Stack direction="row" alignItems="center" spacing={2} sx={{ color: "text.secondary", mt: 0.5 }}>
            {country && (
              <Stack direction="row" spacing={0.75} alignItems="center">
                <LocationOnIcon fontSize="small" />
                <span>{country}</span>
              </Stack>
            )}
            {country && preferredLanguage && <Divider flexItem orientation="vertical" />}
            {preferredLanguage && (
              <Stack direction="row" spacing={0.75} alignItems="center">
                <LanguageIcon fontSize="small" />
                <span>{preferredLanguage}</span>
              </Stack>
            )}
          </Stack>

          {/* Actions */}
          <Stack direction="row" spacing={1.2} sx={{ mt: 1 }}>
            <Button variant="outlined" startIcon={<ShareIcon />} sx={{ borderRadius: 3 }}>
              Share
            </Button>
            <Button variant="contained" startIcon={<EditIcon />} sx={{ borderRadius: 3 }}>
              Edit profile
            </Button>
          </Stack>
        </Stack>
      </Box>
      {/* Tabs: Itineraries / Saved */}
          <Tabs
            value={tab}
            onChange={(_, v) => setTab(v)}
            centered
            sx={{ mt: 2 }}
            TabIndicatorProps={{ sx: { height: 3, borderRadius: 3 } }}
          >
            <Tab label="Itineraries" />
            <Tab label="Saved" />
          </Tabs>

      {/* CONTENT GRID (like Pinterest boards grid) */}
      <Box sx={{ maxWidth: 1280, mx: "auto", px: { xs: 1.5, md: 3 }, mt: 2 }}>
        {tab === 0 ? (
          <Grid container spacing={2}>
            {mockItineraries.map((it) => (
              <Grid key={it.id} size={4}>
                <Card sx={{ borderRadius: 3, overflow: "hidden" }}>
                  <CardActionArea>
                    <CardMedia
                      component="img"
                      // Slightly taller preview to mimic Pinterest tiles
                      sx={{ aspectRatio: "4/5", objectFit: "cover" }}
                      image={it.cover}
                      alt={it.title}
                    />
                  </CardActionArea>
                  <CardContent sx={{ pb: 2 }}>
                    <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                      {it.title}
                    </Typography>
                    <Typography variant="body2" sx={{ color: "text.secondary" }}>
                      {it.stops} stops · Updated {new Date(it.lastUpdated).toLocaleDateString()}
                    </Typography>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        ) : (
          <Grid container spacing={2}>
            {mockSaved.map((s) => (
              <Grid key={s.id} size={4}>
                <Card sx={{ borderRadius: 3, overflow: "hidden" }}>
                  <CardActionArea>
                    <CardMedia
                      component="img"
                      sx={{ aspectRatio: "4/5", objectFit: "cover" }}
                      image={s.image}
                      alt={s.title}
                    />
                  </CardActionArea>
                  <CardContent sx={{ pb: 2 }}>
                    <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                      {s.title}
                    </Typography>
                    <Typography variant="body2" sx={{ color: "text.secondary" }}>
                      {s.location}
                    </Typography>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        )}
      </Box>
    </Box>
  );
}
