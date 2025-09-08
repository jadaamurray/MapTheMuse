import { Typography, Box, Stack, Container, Skeleton } from '@mui/material';
import FlightBoard from '../features/destinations/components/FlightBoard/FlightBoard';
import FeatureCard from '../components/ui/FeatureCard';
import EventNoteTwoTone from "@mui/icons-material/EventNoteTwoTone";
import LibraryMusicTwoTone from "@mui/icons-material/LibraryMusicTwoTone";
import AutoStoriesTwoTone from "@mui/icons-material/AutoStoriesTwoTone";
import MovieCreationTwoTone from "@mui/icons-material/MovieCreationTwoTone";
import PaletteTwoTone from "@mui/icons-material/PaletteTwoTone"
import CardRail from '../components/ui/CardRail';
import { useDestinationsContext } from '../features/destinations/context/DestinationsContext';

export default function Homepage() {
    const { items: destinations, loading, error } = useDestinationsContext();
    return (
        <Stack
            gap={5}
            sx={{
                flex: 1,
                justifyContent: 'center',
                alignItems: 'center',
                display: "flex",
                width: "100%",
                alignSelf: 'stretch'
            }}
        >
            {/* Hero Section */}
            <Box
                sx={{
                    position: "relative",
                    height: "100vh",
                    width: "100%",
                    backgroundImage: {
                        xs: "url('/homepage-hero-mobile.jpg')", // phones
                        md: "url('/homepage-hero-desktop.jpg')",
                    },
                    backgroundSize: "cover",
                    backgroundPosition: "center",
                    backgroundRepeat: "no-repeat",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    color: "white",
                    textAlign: "center",
                    p: 4
                }}
            >
                <Box
                    sx={{
                        position: "absolute",
                        inset: 0,
                        background:
                            "linear-gradient(rgba(0,0,0,0.5), rgba(0,0,0,0.5))",
                    }}
                />
                <Stack
                    direction='row'
                    sx={{
                        width: '100%',
                        justifyContent: 'center',
                        alignItems: 'stretch',
                        gap: 9,
                        width: '100%',
                        flexWrap: 'wrap',
                        px: 2,
                        zIndex: 1
                    }
                    }
                >
                    {/* Left text */}
                    < Box sx={{ flex: '1 1 480px', minWidth: 220, display: 'flex', flexDirection: 'column', justifyContent: 'center' }}>
                        <Typography variant='h1' sx={{ color: 'background.white' }}>
                            Boarding Pass to Inspiration
                        </Typography>
                        <Typography variant='body2'>
                            Choose your flight and we’ll connect you with the media that transforms travel into experience.
                        </Typography>

                    </Box>
                    {/* Flight board right */}
                    {loading ? (
                        <Container sx={{ py: 6 }}>
                            <Skeleton variant="rectangular" sx={{ borderRadius: 3 }} />
                            <Skeleton height={28} width="40%" sx={{ mt: 3 }} />
                            <Skeleton height={18} width="70%" />
                        </Container>
                    ) : (
                        <Box sx={{ flex: '1 1 480px', minWidth: 220, }}>
                            <FlightBoard
                                destinations={destinations ?? []} />
                        </Box>
                    )}
                </Stack>
            </Box>
            {/* Features Section */}
            <Stack
                alignItems={'center'}
                spacing={0}
                sx={{ display: 'flex', py: 10 }}
            >
                <Typography variant='h1' sx={{ color: 'primary.main' }}>
                    Immerse Yourself Before You Go
                </Typography>
                <Typography variant='body2'>
                    Plug in your destination to uncover the films, books and music that bring it to life.
                    Feel the journey before you even pack.
                </Typography>
                {/* Feature Cards */}
                <CardRail cardWidth={289 + 24} showArrows snap="start">
                    <FeatureCard
                        title='Film & TV'
                        subtitle="Watch the world's"
                        description="From blockbusters to hidden gems, explore films and shows that capture your destination's spirit."
                        Icon={MovieCreationTwoTone}
                    />
                    <FeatureCard
                        title='Music'
                        subtitle='Discover linked'
                        description='Discover the soundtrack to your journey. From global hits to local legends.'
                        Icon={LibraryMusicTwoTone}
                    />
                    <FeatureCard
                        title='Itinerary'
                        subtitle='Create your own'
                        description='Build your own trip plan, whether for one city or many.'
                        Icon={EventNoteTwoTone}
                    />
                    <FeatureCard
                        title='Books'
                        subtitle='Read your way through'
                        description='Pair your journey with stories that bring places to life.'
                        Icon={AutoStoriesTwoTone}
                    />
                    <FeatureCard
                        title='Artwork'
                        subtitle='Experience the'
                        description='From timeless masterpieces to vibrant local creations, explore art that captures the essence of your destination.'
                        Icon={PaletteTwoTone}
                    />
                </CardRail>
            </Stack>
            {/* Video in bottom section */}
            <Box
                component="video"
                src="/homepageVideo.mp4"  // public/videos/myvideo.mp4
                autoPlay
                muted
                loop
                sx={{
                    width: "100%",
                    height: "auto",
                    display: "block",
                }}
            />
        </Stack >
    )
}