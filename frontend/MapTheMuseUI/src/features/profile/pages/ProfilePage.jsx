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
} from "@mui/material";
import { useMemo, useState } from "react";
import { useAuthContext } from "../../auth/context/AuthContext";
import { useNavigate } from "react-router-dom";
import ProfileHeader from "../components/ProfileHeader";


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
  const navigate = useNavigate();

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
      <ProfileHeader user={user} />
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
