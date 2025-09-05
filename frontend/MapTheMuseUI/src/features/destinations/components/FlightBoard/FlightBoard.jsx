import FlightBoardButton from "./FlightBoardButton";
import FlightBoardHeader from "./FlightBoardHeader";
import { Box, Stack, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useRef, useMemo } from "react";

const addMinutes = (date, mins) => new Date(date.getTime() + mins * 60000);

// 24h time
const fmt = new Intl.DateTimeFormat(undefined, {
    hour: "2-digit",
    minute: "2-digit",
    hour12: false
});

export default function FlightBoard({ destinations = [] }) {
    const navigate = useNavigate()
    const startRef = useRef(new Date()); 
    // times for the rows
    const rows = useMemo(() => {
        return destinations.map((d, i) => {
            const t = addMinutes(startRef.current, (i+1) * 37);
            return {
                id: d.id,
                name: d.name,
                timeLabel: fmt.format(t)
            };
        });
    }, [destinations]);

    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: "column",
                alignItems: "flex-start",
                gap: 4,
                borderRadius: 2,      // 8px
                p: 4.5,
                background:
                    "linear-gradient(270deg, rgba(19, 20, 24, 0.95) 0.12%, #0D1117 99.88%)",
                boxShadow:
                    "0 -4px 8px 0 rgba(255, 255, 255, 0.08) inset, 0 4px 24px 0 rgba(0, 0, 0, 0.42), 0 4px 8px 0 rgba(0, 0, 0, 0.08) inset",
                width: "100%",
            }}
        >
            <FlightBoardHeader />

            <Stack spacing={1} sx={{ width: "100%", overflowY: "auto" }}>
                {destinations.length === 0 ? (
                    <Typography variant="body2" sx={{ opacity: 0.7, px: 1 }}>
                        No destinations yet.
                    </Typography>
                ) : (
                    rows.slice(0, 5).map(({ id, name, timeLabel }) => (
                        <FlightBoardButton
                            key={id}
                            destination={name}
                            onClick={() => navigate(`/destinations/${id}`)}
                            time={timeLabel}
                        />
                    ))
                )}
            </Stack>
        </Box>
    );
}
