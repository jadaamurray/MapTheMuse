import FlightBoard from "../components/FlightBoard/FlightBoard";
import { Box, Stack, Typography } from "@mui/material";
import { useDestinations } from "../hooks/useDestinations";

export default function DestinationsPage() {
    const { data: destinations, loading, error } = useDestinations();

    if (loading) return <Box py={4}>Loading…</Box>;
    if (error) return <Box py={4}>Failed to load destinations</Box>;

    /* console.log(
        " PAGE dest keys",
        destinations.map(d => d?.id)
    );
    console.log('PAGE dest names', destinations.map(d => d?.name)) */


    return (
        <Box
            sx={{
                width: '100%',
                py: 25,
                display: 'flex',
                gap: 15,
                flexDirection: 'column'
            }}
        >
            <Box sx={{ alignItems: 'center' }}>
                <Typography variant='h1' sx={{ color: 'primary.main' }}>Where are you off to?</Typography>
                <Typography variant='body2'>Pick your next adventure from our curated flight board</Typography>
            </Box>

            {/* want to change something about the scrolling postion here */}
            <Box position={'sticky'} >
                <FlightBoard destinations={destinations ?? []} />
            </Box>

        </Box>
    )
}