import {
    Box,
    Stack,
    Grid,
    Paper,
    Avatar,
    Typography,
    IconButton,
    Button,
    Chip,
    Divider,
    Tabs,
    Tab,
    Card,
    CardMedia,
    CardContent,
    CardActions,
    Tooltip,
    Badge,
    Skeleton,
    TextField,
    Switch,
    FormControlLabel,
} from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import UploadIcon from "@mui/icons-material/Upload";
import SettingsIcon from "@mui/icons-material/Settings";
import ShareIcon from "@mui/icons-material/Share";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import LanguageIcon from "@mui/icons-material/Language";
import EmailIcon from "@mui/icons-material/Email";
import CalendarMonthIcon from "@mui/icons-material/CalendarMonth";
import StarIcon from "@mui/icons-material/Star";
import { useEffect, useMemo, useState, useCallback } from "react";

// Optional: wire these to your API when ready
// import apiClient from "../../../api/apiClient";
// const getProfile = () => apiClient.get("/me").then(r => r.data);
// const updateProfile = (payload) => apiClient.put("/me", payload).then(r => r.data);

const mockProfile = {
    id: 1,
    name: "Jada Murray",
    username: "jada",
    avatarUrl: "https://i.pravatar.cc/160?img=5",
    coverUrl:
        "https://images.unsplash.com/photo-1526778548025-fa2f459cd5c1?q=80&w=2000&auto=format&fit=crop",
    bio: "Traveller. Reader. Building MapTheMuse — connecting destinations with books, films and music.",
    location: "London, United Kingdom",
    languages: ["English", "French"],
    email: "jada@example.com",
    memberSince: "2024-01-15",
    interests: ["History", "Film", "Architecture", "Art", "Food"],
    stats: {
        itineraries: 7,
        reviews: 12,
        saved: 34,
        followers: 128,
    },
};

const mockItineraries = [
    {
        id: 101,
        title: "Tokyo in 5 Days",
        cover:
            "https://images.unsplash.com/photo-1558981806-ec527fa84c39?q=80&w=1600&auto=format&fit=crop",
        stops: 12,
        lastUpdated: "2025-06-18",
    },
    {
        id: 102,
        title: "Cinematic Rome",
        cover:
            "https://images.unsplash.com/photo-1520903920243-cf3152b1e9b8?q=80&w=1600&auto=format&fit=crop",
        stops: 8,
        lastUpdated: "2025-04-03",
    },
    {
        id: 103,
        title: "Art & Architecture in Paris",
        cover:
            "https://images.unsplash.com/photo-1543340900-63b7344b53a7?q=80&w=1600&auto=format&fit=crop",
        stops: 10,
        lastUpdated: "2025-02-22",
    },
];

const mockSaved = [
    {
        id: "S1",
        type: "Film",
        title: "Before Sunrise",
        location: "Vienna, Austria",
        image:
            "https://images.unsplash.com/photo-1505764706515-aa95265c5abc?q=80&w=1600&auto=format&fit=crop",
    },
    {
        id: "S2",
        type: "Book",
        title: "Norwegian Wood",
        location: "Tokyo, Japan",
        image:
            "https://images.unsplash.com/photo-1513530534585-c7b1394c6d51?q=80&w=1600&auto=format&fit=crop",
    },
    {
        id: "S3",
        type: "Music",
        title: "Fado Classics",
        location: "Lisbon, Portugal",
        image:
            "https://images.unsplash.com/photo-1505761671935-60b3a7427bad?q=80&w=1600&auto=format&fit=crop",
    },
];

function Stat({ icon: Icon, label, value }) {
    return (
        <Stack
            component={Paper}
            elevation={0}
            sx={{
                p: 2,
                borderRadius: 3,
                bgcolor: "background.paper",
                border: (t) => `1px solid ${t.palette.divider}`,
                minWidth: 140,
            }}
            spacing={0.5}
        >
            <Stack direction="row" alignItems="center" spacing={1}>
                <Icon fontSize="small" />
                <Typography variant="caption" sx={{ opacity: 0.7 }}>
                    {label}
                </Typography>
            </Stack>
            <Typography variant="h5" sx={{ fontWeight: 700 }}>
                {value}
            </Typography>
        </Stack>
    );
}

function TabPanel({ value, index, children }) {
    return (
        <Box
            role="tabpanel"
            hidden={value !== index}
            aria-labelledby={`profile-tab-${index}`}
            sx={{ mt: 2 }}
        >
            {value === index && <Box>{children}</Box>}
        </Box>
    );
}

export default function ProfilePage() {
    const [loading, setLoading] = useState(true);
    const [profile, setProfile] = useState(null);
    const [tab, setTab] = useState(0);

    // Settings (demo state)
    const [emailUpdates, setEmailUpdates] = useState(true);
    const [publicProfile, setPublicProfile] = useState(true);
    const [displayName, setDisplayName] = useState("");

    useEffect(() => {
        // Replace with your real fetch:
        // getProfile().then(setProfile).finally(() => setLoading(false));
        const t = setTimeout(() => {
            setProfile(mockProfile);
            setDisplayName(mockProfile.name);
            setLoading(false);
        }, 500);
        return () => clearTimeout(t);
    }, []);

    const handleTabChange = (_e, v) => setTab(v);

    const initials = useMemo(() => {
        const n = profile?.name ?? "";
        return n
            .split(" ")
            .map((p) => p[0])
            .join("")
            .slice(0, 2)
            .toUpperCase();
    }, [profile]);

    const handleSaveSettings = useCallback(() => {
        // await updateProfile({ name: displayName, emailUpdates, publicProfile });
        // You can toast/snackbar success here
        setProfile((p) => (p ? { ...p, name: displayName } : p));
    }, [displayName, emailUpdates, publicProfile]);

    return (
        <Box sx={{ pb: 6 }}>
            {/* Cover / Hero */}
            <Box
                sx={{
                    position: "relative",
                    height: { xs: 220, md: 300 },
                    borderBottomLeftRadius: { xs: 0, md: 24 },
                    borderBottomRightRadius: { xs: 0, md: 24 },
                    overflow: "hidden",
                    background: (t) =>
                        `linear-gradient(0deg, ${t.palette.background.default}, transparent), url('${profile?.coverUrl || mockProfile.coverUrl
                        }') center/cover no-repeat`,
                }}
            >
                {/* Overlay controls */}
                <Stack
                    direction="row"
                    spacing={1}
                    sx={{ position: "absolute", top: 12, right: 12 }}
                >
                    <Tooltip title="Upload cover">
                        <IconButton color="inherit" sx={{ bgcolor: "rgba(0,0,0,0.4)" }}>
                            <UploadIcon htmlColor="#fff" />
                        </IconButton>
                    </Tooltip>
                    <Tooltip title="Settings">
                        <IconButton color="inherit" sx={{ bgcolor: "rgba(0,0,0,0.4)" }}>
                            <SettingsIcon htmlColor="#fff" />
                        </IconButton>
                    </Tooltip>
                    <Tooltip title="Share profile">
                        <IconButton color="inherit" sx={{ bgcolor: "rgba(0,0,0,0.4)" }}>
                            <ShareIcon htmlColor="#fff" />
                        </IconButton>
                    </Tooltip>
                </Stack>

                {/* Avatar + name */}
                <Stack
                    direction="row"
                    spacing={2}
                    alignItems="flex-end"
                    sx={{
                        position: "absolute",
                        left: { xs: 16, md: 24 },
                        bottom: { xs: -40, md: -48 },
                    }}
                >
                    <Badge
                        overlap="circular"
                        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
                        badgeContent={
                            <IconButton
                                size="small"
                                sx={{
                                    bgcolor: "background.paper",
                                    border: (t) => `1px solid ${t.palette.divider}`,
                                }}
                                aria-label="Edit avatar"
                            >
                                <EditIcon fontSize="small" />
                            </IconButton>
                        }
                    >
                        {loading ? (
                            <Skeleton variant="circular" width={96} height={96} />
                        ) : (
                            <Avatar
                                src={profile?.avatarUrl}
                                alt={`${profile?.name}'s avatar`}
                                sx={{
                                    width: { xs: 96, md: 112 },
                                    height: { xs: 96, md: 112 },
                                    border: (t) => `3px solid ${t.palette.background.paper}`,
                                }}
                            >
                                {initials}
                            </Avatar>
                        )}
                    </Badge>

                    <Box sx={{ pb: { xs: 1, md: 2 } }}>
                        {loading ? (
                            <>
                                <Skeleton width={220} height={36} />
                                <Skeleton width={140} height={24} />
                            </>
                        ) : (
                            <>
                                <Typography variant="h4" sx={{ fontWeight: 800 }}>
                                    {profile?.name}
                                </Typography>
                                <Typography variant="body2" sx={{ opacity: 0.8 }}>
                                    @{profile?.username}
                                </Typography>
                            </>
                        )}
                    </Box>
                </Stack>
            </Box>

            {/* Main content */}
            <Box sx={{ px: { xs: 2, md: 3 }, mt: { xs: 7, md: 9 }, alignItems: "center" }}>
                <Grid container spacing={3}>
                    {/* Left column: bio + stats + interests */}
                    <Grid xs={12} md={4} lg={3.5}>
                        <Stack spacing={2}>
                            <Paper
                                elevation={0}
                                sx={{
                                    p: 2,
                                    borderRadius: 3,
                                    border: (t) => `1px solid ${t.palette.divider}`,
                                }}
                            >
                                {loading ? (
                                    <>
                                        <Skeleton height={22} width="60%" />
                                        <Skeleton height={16} width="90%" />
                                        <Skeleton height={16} width="70%" />
                                    </>
                                ) : (
                                    <>
                                        <Typography variant="body1">{profile?.bio}</Typography>
                                        <Divider sx={{ my: 1.5 }} />
                                        <Stack spacing={1.2}>
                                            <Stack direction="row" spacing={1.2} alignItems="center">
                                                <LocationOnIcon fontSize="small" />
                                                <Typography variant="body2">
                                                    {profile?.location}
                                                </Typography>
                                            </Stack>
                                            <Stack direction="row" spacing={1.2} alignItems="center">
                                                <LanguageIcon fontSize="small" />
                                                <Typography variant="body2">
                                                    {profile?.languages?.join(", ")}
                                                </Typography>
                                            </Stack>
                                            <Stack direction="row" spacing={1.2} alignItems="center">
                                                <EmailIcon fontSize="small" />
                                                <Typography variant="body2">{profile?.email}</Typography>
                                            </Stack>
                                            <Stack direction="row" spacing={1.2} alignItems="center">
                                                <CalendarMonthIcon fontSize="small" />
                                                <Typography variant="body2">
                                                    Member since{" "}
                                                    {new Date(
                                                        profile?.memberSince || ""
                                                    ).toLocaleDateString()}
                                                </Typography>
                                            </Stack>
                                        </Stack>
                                    </>
                                )}
                            </Paper>

                            {/* Stats */}
                            <Stack direction="row" flexWrap="wrap" gap={1.5}>
                                <Stat icon={CalendarMonthIcon} label="Itineraries" value={profile?.stats?.itineraries ?? 0} />
                                <Stat icon={StarIcon} label="Reviews" value={profile?.stats?.reviews ?? 0} />
                                <Stat icon={FavoriteBorderIcon} label="Saved" value={profile?.stats?.saved ?? 0} />
                                <Stat icon={ShareIcon} label="Followers" value={profile?.stats?.followers ?? 0} />
                            </Stack>

                            {/* Interests */}
                            <Paper
                                elevation={0}
                                sx={{
                                    p: 2,
                                    borderRadius: 3,
                                    border: (t) => `1px solid ${t.palette.divider}`,
                                }}
                            >
                                <Typography variant="subtitle2" sx={{ mb: 1 }}>
                                    Interests
                                </Typography>
                                <Stack direction="row" flexWrap="wrap" gap={1}>
                                    {(profile?.interests || []).map((k) => (
                                        <Chip key={k} label={k} sx={{ borderRadius: 2 }} />
                                    ))}
                                </Stack>
                            </Paper>
                        </Stack>
                    </Grid>

                    {/* Right column: tabs */}
                    <Grid item xs={12} md={8} lg={8.5}>
                        <Paper
                            elevation={0}
                            sx={{
                                borderRadius: 3,
                                border: (t) => `1px solid ${t.palette.divider}`,
                            }}
                        >
                            <Tabs
                                value={tab}
                                onChange={handleTabChange}
                                variant="scrollable"
                                scrollButtons="auto"
                                sx={{
                                    px: 1,
                                    borderBottom: (t) => `1px solid ${t.palette.divider}`,
                                }}
                            >
                                <Tab label="Overview" id="profile-tab-0" />
                                <Tab label="Itineraries" id="profile-tab-1" />
                                <Tab label="Saved" id="profile-tab-2" />
                                <Tab label="Settings" id="profile-tab-3" />
                            </Tabs>

                            {/* Overview */}
                            <Box sx={{ p: { xs: 2, md: 3 } }}>
                                <TabPanel value={tab} index={0}>
                                    <Typography variant="subtitle1" sx={{ mb: 1.5 }}>
                                        Recently updated itineraries
                                    </Typography>
                                    <Grid container spacing={2}>
                                        {mockItineraries.slice(0, 2).map((it) => (
                                            <Grid item xs={12} md={6} key={it.id}>
                                                <Card sx={{ borderRadius: 3, overflow: "hidden" }}>
                                                    <CardMedia
                                                        component="img"
                                                        height="140"
                                                        image={it.cover}
                                                        alt={it.title}
                                                    />
                                                    <CardContent>
                                                        <Typography variant="h6">{it.title}</Typography>
                                                        <Typography variant="body2" sx={{ opacity: 0.8 }}>
                                                            {it.stops} stops · Updated{" "}
                                                            {new Date(it.lastUpdated).toLocaleDateString()}
                                                        </Typography>
                                                    </CardContent>
                                                    <CardActions sx={{ px: 2, pb: 2 }}>
                                                        <Button size="small" variant="contained">
                                                            Open
                                                        </Button>
                                                        <Button size="small">Share</Button>
                                                    </CardActions>
                                                </Card>
                                            </Grid>
                                        ))}
                                    </Grid>
                                </TabPanel>

                                {/* Itineraries */}
                                <TabPanel value={tab} index={1}>
                                    <Grid container spacing={2}>
                                        {mockItineraries.map((it) => (
                                            <Grid item xs={12} sm={6} md={4} key={it.id}>
                                                <Card sx={{ borderRadius: 3, overflow: "hidden" }}>
                                                    <CardMedia
                                                        component="img"
                                                        height="160"
                                                        image={it.cover}
                                                        alt={it.title}
                                                    />
                                                    <CardContent>
                                                        <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                                                            {it.title}
                                                        </Typography>
                                                        <Typography variant="body2" sx={{ opacity: 0.8 }}>
                                                            {it.stops} stops · Updated{" "}
                                                            {new Date(it.lastUpdated).toLocaleDateString()}
                                                        </Typography>
                                                    </CardContent>
                                                    <CardActions sx={{ px: 2, pb: 2 }}>
                                                        <Button size="small" variant="contained">
                                                            Open
                                                        </Button>
                                                        <Button size="small">Share</Button>
                                                    </CardActions>
                                                </Card>
                                            </Grid>
                                        ))}
                                    </Grid>
                                </TabPanel>

                                {/* Saved */}
                                <TabPanel value={tab} index={2}>
                                    <Grid container spacing={2}>
                                        {mockSaved.map((s) => (
                                            <Grid item xs={12} sm={6} md={4} key={s.id}>
                                                <Card sx={{ borderRadius: 3, overflow: "hidden" }}>
                                                    <CardMedia
                                                        component="img"
                                                        height="160"
                                                        image={s.image}
                                                        alt={s.title}
                                                    />
                                                    <CardContent>
                                                        <Stack
                                                            direction="row"
                                                            alignItems="center"
                                                            justifyContent="space-between"
                                                            sx={{ mb: 0.5 }}
                                                        >
                                                            <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                                                                {s.title}
                                                            </Typography>
                                                            <Chip size="small" label={s.type} />
                                                        </Stack>
                                                        <Typography variant="body2" sx={{ opacity: 0.8 }}>
                                                            {s.location}
                                                        </Typography>
                                                    </CardContent>
                                                    <CardActions sx={{ px: 2, pb: 2 }}>
                                                        <IconButton aria-label="Save">
                                                            <FavoriteBorderIcon />
                                                        </IconButton>
                                                        <Button size="small">Open</Button>
                                                    </CardActions>
                                                </Card>
                                            </Grid>
                                        ))}
                                    </Grid>
                                </TabPanel>

                                {/* Settings */}
                                <TabPanel value={tab} index={3}>
                                    <Grid container spacing={2}>
                                        <Grid item xs={12} md={6}>
                                            <Paper
                                                elevation={0}
                                                sx={{
                                                    p: 2,
                                                    borderRadius: 3,
                                                    border: (t) => `1px solid ${t.palette.divider}`,
                                                }}
                                            >
                                                <Typography variant="subtitle1" sx={{ mb: 1 }}>
                                                    Profile
                                                </Typography>
                                                <Stack spacing={2}>
                                                    <TextField
                                                        label="Display name"
                                                        value={displayName}
                                                        onChange={(e) => setDisplayName(e.target.value)}
                                                        fullWidth
                                                    />
                                                    <FormControlLabel
                                                        control={
                                                            <Switch
                                                                checked={publicProfile}
                                                                onChange={(e) => setPublicProfile(e.target.checked)}
                                                            />
                                                        }
                                                        label="Public profile"
                                                    />
                                                    <FormControlLabel
                                                        control={
                                                            <Switch
                                                                checked={emailUpdates}
                                                                onChange={(e) => setEmailUpdates(e.target.checked)}
                                                            />
                                                        }
                                                        label="Email updates"
                                                    />
                                                    <Stack direction="row" spacing={1}>
                                                        <Button variant="contained" onClick={handleSaveSettings}>
                                                            Save changes
                                                        </Button>
                                                        <Button variant="outlined">Cancel</Button>
                                                    </Stack>
                                                </Stack>
                                            </Paper>
                                        </Grid>
                                        <Grid item xs={12} md={6}>
                                            <Paper
                                                elevation={0}
                                                sx={{
                                                    p: 2,
                                                    borderRadius: 3,
                                                    border: (t) => `1px solid ${t.palette.divider}`,
                                                }}
                                            >
                                                <Typography variant="subtitle1" sx={{ mb: 1 }}>
                                                    Danger zone
                                                </Typography>
                                                <Typography variant="body2" sx={{ opacity: 0.8, mb: 1 }}>
                                                    Delete your account and all associated data. This action can’t be undone.
                                                </Typography>
                                                <Button variant="outlined" colour="error">
                                                    Delete account
                                                </Button>
                                            </Paper>
                                        </Grid>
                                    </Grid>
                                </TabPanel>
                            </Box>
                        </Paper>
                    </Grid>
                </Grid>
            </Box>
        </Box>
    );
}
