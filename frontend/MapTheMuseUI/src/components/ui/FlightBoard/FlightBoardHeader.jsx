import { Box, Stack, Typography } from "@mui/material"

export const FlightBoardHeader = () => {
    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: "column",
                alignItems: "flex-start",
                gap: 8,
                alignSelf: "stretch",
                borderRadius: 4,
            }}
        >
            {/* DEPARTURES and TIME */}
            <Box
                sx={{
                    display: "flex",
                    alignItems: "flex-start",
                    gap: 10,
                    alignSelf: "stretch",
                }}
            >
                <Stack direction={"row"} spacing={10} sx={{ width: "100%" }}>
                    <Typography
                        sx={{
                            fontFamily: "monospace",
                            fontSize: 32,
                            fontWeight: 700,
                            color: "#FFFFFF",
                        }}
                    >
                        DEPARTURES
                    </Typography>
                    <Box
                        sx={{
                            width: 258,
                            height: 32,
                        }}
                    ></Box>
                    <Typography
                        sx={{
                            fontFamily: "monospace",
                            fontSize: 32,
                            fontWeight: 700,
                            color: "#FFDF29",
                        }}
                    >
                        18:03
                    </Typography>
                </Stack>
            </Box>
            {/* Column Headers */}
            <Stack direction={"row"} sx={{ width: '100%', justifyContent: 'space-between'}}>
                <Typography
                    sx={{
                        color: "#FBF4E3",
                        fontFamily: "Inter-SemiBold, Helvetica",
                        fontSize: 18,
                        fontStyle: "normal",
                        fontWeight: 600,
                        lineHeight: "normal",
                        letterSpacing: 0.018,
                        flex: 1,
                        alignText: "left",
                    }}
                >
                    TIME
                </Typography>
                <Typography
                    sx={{
                        color: "#FBF4E3",
                        fontFamily: "Inter-SemiBold, Helvetica",
                        fontSize: 18,
                        fontStyle: "normal",
                        fontWeight: 600,
                        lineHeight: "normal",
                        letterSpacing: 0.018,
                        flex: 1,
                        alignText: "left",
                    }}
                >
                    DESTINATION
                </Typography>
                <Typography
                    sx={{
                        color: "#FBF4E3",
                        fontFamily: "Inter-SemiBold, Helvetica",
                        fontSize: 18,
                        fontStyle: "normal",
                        fontWeight: 600,
                        lineHeight: "normal",
                        letterSpacing: 0.018,
                        flex: 1,
                        alignText: "left",
                    }}
                >
                    STATUS
                </Typography>
            </Stack>
        </Box>
    )
}
export default FlightBoardHeader;