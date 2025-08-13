import FlightBoardButton from "./FlightBoardButton";
import FlightBoardHeader from "./FlightBoardHeader";
import { Box, Stack, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function FlightBoard({ destinations = [] }) {
    //console.log("destinations sample", destinations?.[0]);
    const navigate = useNavigate()
    /*console.log("COMPONENT destinations prop ->", destinations);
    console.log(
  "dest keys",
  destinations.map(d => d?.id)
);
/onsole.log('dest names', destinations.map(d => d?.name))*/

    return (
        <Box
            sx={{
                display: "flex",
                height: 460,
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
                    destinations.map(({ id, name }) => (
                        <FlightBoardButton key={id} destination={name} onClick={() => navigate(`/destinations/${id}`)} />
                    ))
                )}
            </Stack>
        </Box>
    );
}
