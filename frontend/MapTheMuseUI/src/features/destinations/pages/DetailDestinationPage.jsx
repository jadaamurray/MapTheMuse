import { Box, Typography, Stack, Container, Divider } from "@mui/material";
import { useParams } from "react-router-dom";
import { useDestination } from "../hooks/useDestinations";
import DestinationSpotifyPlaylist from "../components/DestinationSpotifyPlaylist";

export default function DetailDestinationPage() {
    const { id } = useParams();
    const { data, loading, error } = useDestination(id);
    const name = data?.name ?? "";
    const description = data?.description ?? "";
    const src = `/destinationPhotos/${encodeURIComponent(name)}.jpeg`;

    if (loading) return <Container sx={{ py: 6 }}><Typography>Loading…</Typography></Container>;
    if (error) return <Container sx={{ py: 6 }}><Typography colour="error.main">Couldn’t load destination.</Typography></Container>;
    if (!data) return <Container sx={{ py: 6 }}><Typography>Not found.</Typography></Container>;

    //console.log('data loaded', data);
    //console.log(`/destinationPhotos/${encodeURIComponent(data?.name ?? "")}.jpeg`)

    return (
        <Box
            sx={{
                width: '100%',
                py: 5,
                display: 'flex',
                gap: 8,
                flexDirection: 'column'
            }}
        >
            {/* Title section */}
            <Box
                sx={{
                    position: "relative",
                    width: "100%",
                    height: { xs: 240, md: 580 },
                    borderRadius: 2,
                    overflow: "hidden",
                }}
            >
                {/* Image */}
                <Box
                    component="img"
                    src={src}
                    alt={`${name} photo`}
                    loading="lazy"
                    sx={{
                        width: "100%",
                        height: "100%",
                        objectFit: "cover",
                        objectPosition: "90%",
                        display: "block",
                    }}
                />

                {/* Fade overlay */}
                <Box
                    sx={{
                        position: "absolute",
                        inset: 0,
                        background:
                            "linear-gradient(0deg, rgba(132, 132, 132, 0.35), rgba(246, 246, 246, 0.35))",
                        pointerEvents: "none",
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
            </Box>
            {/* Information Section */}
            <Stack
                borderRadius={4}
                spacing={15}
                sx={{
                    backgroundColor: "background.white",
                    width: '100%',
                    p: 10,
                }}
            >
                {/* Description */}
                <Typography variant="body2">
                    {description}
                </Typography>
                <Divider />
                {/* Spotify embed */}
                <Box display={"flex"} flexDirection={"column"} gap={5}>
                    <Typography variant="h2" color="black" textAlign={"left"}>Soundtrack</Typography>
                    <DestinationSpotifyPlaylist destinationId={id} height={500} theme={0} />
                </Box>
            </Stack>
        </Box >
    )
}