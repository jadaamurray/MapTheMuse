import FlightBoardButton from "./FlightBoardButton";
import FlightBoardHeader from "./FlightBoardHeader";
import { Box, Stack, Typography } from "@mui/material";

export default function FlightBoard({
    destinations = [],
    onSelect,
}) {
    return (
        <Box sx={{
            display: "flex",
            //width: 636,
            height: 460,
            flexDirection: "column",
            alignItems: "flex-start",
            gap: 4,
            borderRadius: 8,
            padding: 4.5,
            background: "linear-gradient(270deg, rgba(19, 20, 24, 0.95) 0.12%, #0D1117 99.88%)",
            boxShadow: "0 -4px 8px 0 rgba(255, 255, 255, 0.08) inset, 0 4px 24px 0 rgba(0, 0, 0, 0.42), 0 4px 8px 0 rgba(0, 0, 0, 0.08) inset",
        }}
        >
            <FlightBoardHeader />
            <Stack spacing={1} sx={{ width: "100%" }}>
                {destinations.length === 0 ? (
                    <Typography variant="body2" sx={{ opacity: 0.7, px: 1 }}>
                        No destinations yet.
                    </Typography>
                ) : (
                    destinations.map((d, index) => {
                        const name = typeof d === "string" ? d : d?.name;
                        const key = typeof d === "string" ? name : d?.id ?? index;
                        if (!name) return null;

                        return (
                            <FlightBoardButton
                                key={key}
                                destination={name}
                                onClick={() => onSelect?.(d)}
                            />
                        );
                    })
                )}            </Stack>
        </Box>
    )
}