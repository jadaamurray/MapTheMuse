import { Typography, Box, Stack } from '@mui/material';
import FlightBoard from '../features/destinations/components/FlightBoard/FlightBoard';
import FeatureCard from '../components/ui/FeatureCard';
import EventNoteTwoTone from "@mui/icons-material/EventNoteTwoTone";
import LibraryMusicTwoTone from "@mui/icons-material/LibraryMusicTwoTone";
import AutoStoriesTwoTone from "@mui/icons-material/AutoStoriesTwoTone";
import MovieCreationTwoTone from "@mui/icons-material/MovieCreationTwoTone";
import PaletteTwoTone from "@mui/icons-material/PaletteTwoTone"
import CardRail from '../components/ui/CardRail';
import { useDestinations } from '../features/destinations/hooks/useDestinations';

export default function Homepage() {
    const { data: destinations } = useDestinations();
    return (
        <Stack
            gap={5}
            sx={{
                flex: 1,
                justifyContent: 'center',
                alignItems: 'center',
                p: 2,
                display: "flex",
                width: "100%",
                alignSelf: 'stretch'
            }}
        >
            {/* Hero Section */}
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
                }}
            >
                {/* Left text */}
                <Box sx={{ flex: '1 1 480px', minWidth: 220, display: 'flex', flexDirection: 'column', justifyContent: 'center' }}>
                    <Typography variant='h1' sx={{ color: 'primary.main' }}>
                        Where are you off to?
                    </Typography>
                    <Typography variant='body2'>
                        Pick your next adventure from our curated flight board
                    </Typography>

                </Box>
                {/* Flight board right */}
                <Box sx={{ flex: '1 1 480px', minWidth: 220, }}>
                    <FlightBoard
                        destinations={destinations ?? []} />
                </Box>
            </Stack>
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
                        title='Itinerary'
                        subtitle='Create your own'
                        description='Create a specialised plan for your trip. Whether that’s one destination or multiple cities!'
                        Icon={EventNoteTwoTone}
                        bg='secondary.main'
                    />
                    <FeatureCard
                        title='Music'
                        subtitle='Discover linked'
                        description='Discover the soundtrack to your journey. From local legends to global hits, match the music to every moment of your travels.'
                        Icon={LibraryMusicTwoTone}
                        bg='secondary.burntSienna'
                    />
                    <FeatureCard
                        title='Books'
                        subtitle='Read your way through'
                        description='Pair your journey with the perfect story. From local legends to world-renowned novels, let every page bring your destination to life.'
                        Icon={AutoStoriesTwoTone}
                        bg='secondary.apricot'
                    />
                    <FeatureCard
                        title='Film & TV'
                        subtitle="Watch the world's"
                        description='From blockbuster favourites to hidden gems, explore films and shows that capture the spirit of your destination.'
                        Icon={MovieCreationTwoTone}
                        bg='primary.main'
                    />
                    <FeatureCard
                        title='Artwork'
                        subtitle='Experience the'
                        description='From timeless masterpieces to vibrant local creations, explore art that captures the essence of your destination.'
                        bg='secondary.main'
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