import {
    Box,
    Container,
    Typography,
    Stack,
    Grid,
    Chip,
    Divider,
    Paper,
    IconButton,
    Tooltip,
    Skeleton,
    Button,
} from "@mui/material";
import ShareIcon from "@mui/icons-material/Share";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import PlayArrowIcon from "@mui/icons-material/PlayArrow";
import InfoOutlinedIcon from "@mui/icons-material/InfoOutlined";
import { useParams } from "react-router-dom";
import DestinationSpotifyPlaylist from "../components/DestinationSpotifyPlaylist";
import QuickFactsCarousel from "../components/QuickFactsCarousel";
import { useEffect, useMemo, useState } from "react";
import Section from "../../../components/ui/Section";
import MediaRail from "../../media/components/MediaRail";
import { useDestinationsContext } from "../context/DestinationsContext";
import { useDestinationMedia } from "../../media/hooks/useDestinationMedia";

export default function DetailDestinationPage() {
    //console.log('🌀 DetailDestinationPage re-rendered');
    const { id } = useParams();
    const { byId, getById, loading: destinationLoading, error } = useDestinationsContext();
    const { media, loading: mediaLoading, error: mediaError } = useDestinationMedia(id);
    const [destination, setDestination] = useState(null);

    const numId = Number(id);

    useEffect(() => {
        const existing = byId.get(numId);
        if (existing) {
            setDestination(existing);
            return;
        }

        getById(numId).then((fetched) => {
            if (fetched) setDestination(fetched);
        });
    }, [id]);

    const movies = useMemo(
        () => media.filter((m) => (m.mediaType || "").toLowerCase() === "movie"),
        [media]
    );
    const tv = useMemo(
        () => media.filter((m) => (m.mediaType || "").toLowerCase() === "tv"),
        [media]
    );
    console.log('destination: ', destination);

    if (destinationLoading || mediaLoading) {
        return (
            <Container sx={{ py: 6 }}>
                <Skeleton variant="rectangular" height={360} sx={{ borderRadius: 3 }} />
                <Skeleton height={28} width="40%" sx={{ mt: 3 }} />
                <Skeleton height={18} width="70%" />
            </Container>
        );
    }
    if (error) {
        return (
            <Container sx={{ py: 6 }}>
                <Typography color="error">Couldn’t load destination.</Typography>
            </Container>
        );
    }
    if (!destination) {
        return (
            <Container sx={{ py: 6 }}>
                <Typography>Not found.</Typography>
            </Container>
        );
    }

    const slug = destination?.slug ?? "";
    const name = destination?.name ?? "";
    const description = destination?.description ?? "";
    const heroSrc = `/destinationPhotos/${encodeURIComponent(slug)}.jpeg`;

    return (
        <Box sx={{ pb: 6 }}>
            {/* HERO */}
            <Box
                sx={{
                    position: "relative",
                    height: { xs: 280, md: 480 },
                    borderRadius: { xs: 0, md: 3 },
                    overflow: "hidden",
                    mx: { xs: -2, md: 0 },
                }}
            >
                <Box
                    component="img"
                    src={heroSrc}
                    alt={`${name} photo`}
                    loading="lazy"
                    sx={{ width: "100%", height: "100%", objectFit: "cover", objectPosition: "100%" }}
                />
                <Box
                    sx={{
                        position: "absolute",
                        inset: 0,
                        background:
                            "linear-gradient(0deg, rgba(132, 132, 132, 0.35), rgba(246, 246, 246, 0.35))",
                    }}
                />
                {/* centred content */}
                <Box
                    sx={{
                        position: "absolute",
                        inset: 0,
                        display: "grid",
                        placeItems: "center",   // <— centres both axes
                        zIndex: 1,
                        px: 2,
                        textAlign: "center",
                    }}
                >
                    {/* Title on top */}
                    <Typography
                        variant="h1"
                        sx={{
                            color: "common.white",
                            fontWeight: 700,
                            textShadow: "0 2px 8px rgba(0,0,0,0.6)",
                        }}
                    >
                        {name}
                    </Typography>
                </Box>
                {/* Title + actions */}
                <Container
                    sx={{
                        position: "absolute",
                        inset: 0,
                        display: "flex",
                        alignItems: "flex-end",
                        pb: { xs: 2, md: 3 },
                    }}
                >
                    <Stack spacing={1.2} sx={{ color: "common.white" }}>
                        {/* <Typography variant="h2" sx={{ fontWeight: 900, textShadow: "0 4px 16px rgba(0,0,0,.4)" }}>
                            {name}
                        </Typography> */}
                        <Stack direction="row" spacing={1}>
                            <Chip
                                icon={<InfoOutlinedIcon />}
                                label="Essential info"
                                size="small"
                                sx={{ bgcolor: "rgba(255,255,255,.15)", color: "common.white" }}
                            />
                            {media?.length > 0 && (
                                <Chip
                                    icon={<PlayArrowIcon />}
                                    label={`${media.length} media picks`}
                                    size="small"
                                    sx={{ bgcolor: "rgba(255,255,255,.15)", color: "common.white" }}
                                />
                            )}
                        </Stack>
                    </Stack>
                    <Stack direction="row" spacing={1} sx={{ ml: "auto" }}>
                        <Tooltip title="Save">
                            <IconButton sx={{ bgcolor: "rgba(255,255,255,.2)" }}>
                                <FavoriteBorderIcon htmlColor="#fff" />
                            </IconButton>
                        </Tooltip>
                        <Tooltip title="Share">
                            <IconButton sx={{ bgcolor: "rgba(255,255,255,.2)" }}>
                                <ShareIcon htmlColor="#fff" />
                            </IconButton>
                        </Tooltip>
                    </Stack>
                </Container>
            </Box>

            {/* STICKY SECTION NAV */}
            <Container sx={{ position: "sticky", top: 0, zIndex: 8, py: 1, bgcolor: "background.default" }}>
                <Stack direction="row" spacing={1.5} sx={{ overflowX: "auto" }}>
                    <Button href="#overview" size="small" variant="text">Overview</Button>
                    <Button href="#soundtrack" size="small" variant="text">Soundtrack</Button>
                    {movies.length > 0 && <Button href="#films" size="small" variant="text">Films</Button>}
                    {tv.length > 0 && <Button href="#tv" size="small" variant="text">TV</Button>}
                </Stack>
                <Divider sx={{ mt: 1 }} />
            </Container>

            {/* CONTENT */}
            <Container sx={{ mt: 3 }}>
                <Grid container spacing={5}>
                    {/* Main */}
                    <Grid size={12}>
                        {/* Quick facts carousel (TripAdvisor vibe) */}
                        <Section id="quickFacts" title="Need to know">
                            {destination?.quickFacts && Object.keys(destination.quickFacts).length > 0 && (
                                <Box sx={{ mb: 3 }}>
                                    <QuickFactsCarousel facts={destination?.quickFacts} />
                                </Box>
                            )}
                        </Section>

                        <Divider sx={{ my: 3 }} />

                        <Section id="overview" title="Overview">
                            <Paper
                                variant="outlined"
                                sx={{
                                    p: { xs: 2, md: 3 },
                                    borderRadius: 3,
                                    borderColor: "divider",
                                }}
                            >
                                <Typography variant="body1" sx={{ lineHeight: 1.7 }}>
                                    {description}
                                </Typography>
                            </Paper>
                        </Section>

                        <Divider sx={{ my: 3 }} />

                        <Section
                            id="soundtrack"
                            title="Soundtrack"
                            subtitle="Plug in and set the mood for your trip"
                        >
                            <Paper
                                elevation={0}
                                sx={{
                                    p: 1.5,
                                    borderRadius: 3,
                                    border: (t) => `1px solid ${t.palette.divider}`,
                                }}
                            >
                                <DestinationSpotifyPlaylist destinationId={id} height={420} theme={0} />
                            </Paper>
                        </Section>

                        {movies.length > 0 && (
                            <>
                                <Divider sx={{ my: 3 }} />
                                <Section
                                    id="films"
                                    title="Films"
                                    subtitle="Stories shot in or set around this destination"
                                >
                                    <MediaRail items={movies} />
                                </Section>
                            </>
                        )}

                        {tv.length > 0 && (
                            <>
                                <Divider sx={{ my: 3 }} />
                                <Section
                                    id="tv"
                                    title="TV"
                                    subtitle="Series that bring the place to life"
                                >
                                    <MediaRail items={tv} />
                                </Section>
                            </>
                        )}
                    </Grid>

                    {/* Sidebar (TripAdvisor-style helpful info area) */}
                    <Grid item xs={12} md={4}>
                        <Stack spacing={2}>
                            <Paper
                                elevation={0}
                                sx={{
                                    p: 2,
                                    borderRadius: 3,
                                    border: (t) => `1px solid ${t.palette.divider}`,
                                }}
                            >
                                <Typography variant="subtitle1" sx={{ fontWeight: 700, mb: 1 }}>
                                    Good to know
                                </Typography>
                                <Stack spacing={1}>
                                    <Stack direction="row" spacing={1}>
                                        <Chip size="small" label="Save to trip" />
                                        <Chip size="small" label="Share" />
                                    </Stack>
                                    <Typography variant="body2" sx={{ color: "text.secondary" }}>
                                        Add this place to your plan and keep all your media & soundtrack picks together.
                                    </Typography>
                                </Stack>
                            </Paper>

                            {/* Optional placeholder for map/nearby – swap when you add a map */}
                            <Paper
                                elevation={0}
                                sx={{
                                    p: 2,
                                    borderRadius: 3,
                                    border: (t) => `1px solid ${t.palette.divider}`,
                                }}
                            >
                                <Typography variant="subtitle1" sx={{ fontWeight: 700, mb: 1 }}>
                                    Map preview
                                </Typography>
                                <Box
                                    sx={{
                                        height: 180,
                                        borderRadius: 2,
                                        bgcolor: "action.hover",
                                        display: "grid",
                                        placeItems: "center",
                                        color: "text.secondary",
                                    }}
                                >
                                    <Typography variant="caption">Map coming soon</Typography>
                                </Box>
                            </Paper>
                        </Stack>
                    </Grid>
                </Grid>
            </Container>
        </Box>
    );
}
